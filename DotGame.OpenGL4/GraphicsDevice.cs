using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.Graphics;
using DotGame.Utils;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;
using DotGame.OpenGL4.Windows;

namespace DotGame.OpenGL4
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }

        public DeviceCreationFlags CreationFlags { get; private set; }

        private GraphicsCapabilities capabilities;
        public GraphicsCapabilities Capabilities
        {
            get { return capabilities; }
        }
        private OpenGLGraphicsCapabilities openGLCapabilities;
        public OpenGLGraphicsCapabilities OpenGLCapabilities
        {
            get { return openGLCapabilities; }
        }

        public IGraphicsFactory Factory { get; private set; }
        public IRenderContext RenderContext { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }
        public bool VSync
        {
            get { return Context.SwapInterval > 1; }
            set
            {
                //AssertCurrent
                Context.SwapInterval = value ? 1 : 0;
            }
        }

        internal GraphicsContext Context { get; private set; }
        internal bool IsCurrent { get { return Context.IsCurrent; } }

        internal BindManager BindManager { get; private set; }

        private IWindowContainer container;

        private Dictionary<FrameBufferDescription, FrameBuffer> fboPool = new Dictionary<FrameBufferDescription, FrameBuffer>();

        private Dictionary<VertexDescription, int> inputLayoutPool = new Dictionary<VertexDescription, int>();
        
        private DebugProc onDebugMessage;

        internal static int MipLevels(int width, int height, int depth = 0)
        {
            var max = Math.Max(width, Math.Max(height, depth));
            return (int)Math.Ceiling(Math.Log(max, 2));
        }

        public GraphicsDevice(IGameWindow window, IWindowContainer container, GraphicsContext context, DeviceCreationFlags creationFlags)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (container == null)
                throw new ArgumentNullException("container");
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.IsDisposed)
                throw new ArgumentException("context is disposed.", "context");

            this.DefaultWindow = window;
            this.container = container;
            this.Context = context;
            this.CreationFlags = creationFlags;
            
            Log.Debug("Got context: [ColorFormat: {0}, Depth: {1}, Stencil: {2}, FSAA Samples: {3}, AccumulatorFormat: {4}, Buffers: {5}, Stereo: {6}]",
                Context.GraphicsMode.ColorFormat,
                Context.GraphicsMode.Depth,
                Context.GraphicsMode.Stencil,
                Context.GraphicsMode.Samples,
                Context.GraphicsMode.AccumulatorFormat,
                Context.GraphicsMode.Buffers,
                Context.GraphicsMode.Stereo);

            Context.LoadAll();

            capabilities = new GraphicsCapabilities();

            CheckGraphicsCapabilities(LogLevel.Verbose);

            Factory = new GraphicsFactory(this);
            BindManager = new OpenGL4.BindManager(this);
            this.RenderContext = new RenderContext(this);

            Context.MakeCurrent(null);
        }
        ~GraphicsDevice()
        {
            Dispose(false);
        }

        public void MakeCurrent()
        {
            Context.MakeCurrent(container.WindowInfo);
        }
        public void DetachCurrent()
        {
            Context.MakeCurrent(null);
        }

        private void CheckGraphicsCapabilities(LogLevel logLevel)
        {
            openGLCapabilities= new OpenGLGraphicsCapabilities();
            capabilities = new GraphicsCapabilities();

            openGLCapabilities.OpenGLVersion = new Version(GL.GetInteger(GetPName.MajorVersion), GL.GetInteger(GetPName.MinorVersion));

            Log.Write(logLevel, "OpenGL Diagnostics:");
            Log.Write(logLevel, "\tOpenGL Version: {0}", openGLCapabilities.OpenGLVersion.ToString());

            //GLSL Version string auslesen
            string glslVersionString = GL.GetString(StringName.ShadingLanguageVersion);
            int glslVersionMajor;
            int glslVersionMinor;
            if (GL.GetError() != ErrorCode.NoError || string.IsNullOrWhiteSpace(glslVersionString))
            {
                glslVersionMajor = 3;
                glslVersionMinor = 3;
            }
            else
            {
                string glslVersionStringMajor = glslVersionString.Substring(0, glslVersionString.IndexOf('.'));
                string glslVersionStringMinor = glslVersionString.Substring(glslVersionString.IndexOf('.') + 1, 1);

                if (!int.TryParse(glslVersionStringMajor, out glslVersionMajor) || !int.TryParse(glslVersionStringMinor, out glslVersionMinor))
                    throw new Exception("Could not determine supported GLSL version");
            }
            CheckGLError("Init Version");

            //Nicht Version mit String initialisieren da 4.40 als Major 4 und Minor 40 aufgefasst wird
            openGLCapabilities.GLSLVersion = new Version(glslVersionMajor, glslVersionMinor);

            Log.Write(logLevel, "\tGLSL Version: {0}", openGLCapabilities.GLSLVersion.ToString());
            Log.Write(logLevel, "\tExtensions supported:");

            //Extensions überprüfen
            int extensionCount = GL.GetInteger(GetPName.NumExtensions);
            var extensions = new List<string>();
            if (GL.GetError() == ErrorCode.NoError)
            {
                for (int i = 0; i < extensionCount; i++)
                    extensions.Add(GL.GetString(StringNameIndexed.Extensions, i));
            }
            else
            {
                var extensionString = GL.GetString(StringName.Extensions);
                extensions.AddRange(extensionString.Split(' '));
            }
            foreach (var extension in extensions)
            {
                switch (extension)
                {
                    case "GL_EXT_texture_compression_s3tc":
                        openGLCapabilities.SupportsS3TextureCompression = true;
                        Log.Write(logLevel, "\t\t" + "GL_EXT_texture_compression_s3tc");
                        break;

                    case "GL_EXT_texture_filter_anisotropic":
                        openGLCapabilities.SupportsAnisotropicFiltering = true;
                        openGLCapabilities.MaxAnisotropicFiltering = (int)GL.GetFloat((GetPName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt);
                        Log.Write(logLevel, "\t\t" + "GL_EXT_texture_filter_anisotropic");
                        CheckGLError("Init GL_EXT_texture_filter_anisotropic");
                        break;


                    case "GL_ARB_debug_output":
                        openGLCapabilities.SupportsDebugOutput = true;

                        if (CreationFlags.HasFlag(DeviceCreationFlags.Debug))
                        {
                            onDebugMessage = new DebugProc(OnDebugMessage);
                            GL.Enable(EnableCap.DebugOutput);
                            GL.DebugMessageCallback(onDebugMessage, IntPtr.Zero);

                            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DontCare, 0, new int[0], true);
                        }
                        Log.Write(logLevel, "\t\t" + "GL_ARB_debug_output");
                        CheckGLError("Init GL_ARB_debug_output");
                        break;

                    case "GL_ARB_get_program_binary":
                        capabilities.SupportsBinaryShaders = true;
                        Log.Write(logLevel, "\t\t" + "GL_ARB_get_program_binary");
                        break;

                    case "GL_ARB_texture_storage":
                        openGLCapabilities.SupportsTextureStorage = true;
                        Log.Write(logLevel, "\t\t" + "GL_ARB_texture_storage");
                        break;

                    case "GL_ARB_buffer_storage":
                        openGLCapabilities.SupportsBufferStorage = true;
                        Log.Write(logLevel, "\t\t" + "GL_ARB_buffer_storage");
                        break;

                    case "GL_ARB_invalidate_subdata":
                        openGLCapabilities.SupportsResourceValidation = true;
                        Log.Write(logLevel, "\t\t" + "GL_ARB_invalidate_subdata");
                        break;

                    case "GL_EXT_direct_state_access":
                        if(openGLCapabilities.DirectStateAccess == DirectStateAccess.None)
                            openGLCapabilities.DirectStateAccess = DirectStateAccess.Extension;

                        Log.Write(logLevel, "\t\t" + "GL_EXT_direct_state_access");
                        break;

                    case "GL_ARB_direct_state_access":
                        //openGLCapabilities.DirectStateAccess = DirectStateAccess.Core;
                        Log.Write(logLevel, "\t\t" + "GL_ARB_direct_state_access");
                        break;

                    case "GL_ARB_vertex_attrib_binding":
                        openGLCapabilities.VertexAttribBinding = true;
                        openGLCapabilities.MaxVertexAttribBindings = GL.GetInteger((GetPName)All.MaxVertexAttribBindings);
                        openGLCapabilities.MaxVertexAttribBindingOffset = GL.GetInteger((GetPName)All.MaxVertexAttribRelativeOffset);
                        Log.Write(logLevel, "\t\t" + "GL_ARB_vertex_attrib_binding");
                        CheckGLError("Init GL_ARB_vertex_attrib_binding");
                        break;
                }
            }

            openGLCapabilities.TextureUnits = GL.GetInteger(GetPName.MaxCombinedTextureImageUnits);
            CheckGLError("Init MaxCombinedTextureImageUnits");
            openGLCapabilities.MaxTextureLoDBias = GL.GetInteger(GetPName.MaxTextureLodBias);
            CheckGLError("Init MaxTextureLodBias");
            openGLCapabilities.MaxTextureSize = GL.GetInteger(GetPName.MaxTextureSize);
            CheckGLError("Init MaxTextureSize");
            openGLCapabilities.MaxVertexAttribs = GL.GetInteger(GetPName.MaxVertexAttribs);
            CheckGLError("Init MaxVertexAttribs");
        }

        private void OnDebugMessage(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr user)
        {
            string sourceString = source.ToString();
            if (sourceString.StartsWith("DebugSource"))
                sourceString = sourceString.Remove(0, "DebugSource".Length);

            string typeString = type.ToString();
            if (typeString.StartsWith("DebugType"))
                typeString = typeString.Remove(0, "DebugType".Length);

            string severityString = severity.ToString();
            if (severityString.StartsWith("DebugSeverity"))
                severityString = severityString.Remove(0, "DebugSeverity".Length);

            string messageString = string.Format("(OpenGL) ({0}) ({1}) ({2}) {3}", severityString, sourceString, typeString, Marshal.PtrToStringAnsi(message, length));

            if (severityString != "Notification")
                throw new Exception(messageString);

            Log.Debug(messageString);
            Log.FlushBuffer();
        }

        public T Cast<T>(IGraphicsObject obj, string parameterName) where T : class, IGraphicsObject
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
            if (obj.IsDisposed)
                throw new ObjectDisposedException(parameterName);
            T ret = obj as T;
            if (ret == null)
                throw new ArgumentException("GraphicsObject is not part of this api.", parameterName);
            if (obj.GraphicsDevice != this)
                throw new ArgumentException("GraphicsObject is not part of this graphics device.", parameterName);
            return ret;
        }

        public int GetSizeOf(TextureFormat format)
        {
            switch (format)
            {
                case TextureFormat.RGB32_Float:
                    return 32 * 3;
                case TextureFormat.RGBA32_Float:
                    return 32 * 4;
                case TextureFormat.RGBA16_UIntNorm:
                    return 16 * 16;
                case TextureFormat.RGBA8_UIntNorm:
                    return 8 * 4;
                case TextureFormat.Depth16:
                    return 16;
                case TextureFormat.Depth24Stencil8:
                    return 24 + 8;
                case TextureFormat.Depth32:
                    return 32;
                default:
                    throw new NotSupportedException("Format is not supported.");
            }
        }

        public int GetSizeOf(VertexElementType format)
        {
            switch (format)
            {
                case VertexElementType.Single:
                    return 4;
                case VertexElementType.Vector2:
                    return 8;
                case VertexElementType.Vector3:
                    return 12;
                case VertexElementType.Vector4:
                    return 16;
                default:
                    throw new NotSupportedException("Format is not supported.");
            }
        }

        public int GetComponentsOf(VertexElementType format)
        {
            switch (format)
            {
                case VertexElementType.Single:
                    return 1;
                case VertexElementType.Vector2:
                    return 2;
                case VertexElementType.Vector3:
                    return 3;
                case VertexElementType.Vector4:
                    return 4;
                default:
                    throw new NotSupportedException("Format is not supported.");
            }
        }

        public int GetSizeOf(IndexFormat format)
        {
            switch (format)
            {
                case IndexFormat.Int32:
                case IndexFormat.UInt32:
                    return 4;
                case IndexFormat.Short16:
                case IndexFormat.UShort16:
                    return 2;
                default:
                    throw new NotSupportedException("Format is not supported.");
            }
        }

        public int GetSizeOf(VertexDescription description)
        {
            int size = 0;
            VertexElement[] elements = description.GetElements();
            for (int i = 0; i < elements.Length; i++)
                size += GetSizeOf(elements[i].Type);

            return size;
        }

        public void SwapBuffers()
        {
            ((GraphicsFactory)Factory).DisposeUnused();
            Context.SwapBuffers();
        }

        //RenderTarget
        internal FrameBuffer GetFBO(FrameBufferDescription description)
        {
            if(!description.HasAttachments)
                return null;

            FrameBuffer fbo;
            if (fboPool.TryGetValue(description, out fbo))
                return fbo;

            GraphicsFactory factory = (GraphicsFactory) Factory;

            fbo = factory.CreateFrameBuffer(description);
            fboPool.Add(description, fbo);
            return fbo;
        }

        internal FrameBuffer GetFBO(int depthAttachmentID)
        {
            return GetFBO(new FrameBufferDescription(depthAttachmentID, new int[] { }));
        }

        internal FrameBuffer GetFBO(int depthAttachmentID, params int[] colorAttachmentIDs)
        {
            return GetFBO(new FrameBufferDescription(depthAttachmentID, colorAttachmentIDs));
        }

        internal int GetLayout(VertexDescription description, Shader shader)
        {
            if (!OpenGLCapabilities.VertexAttribBinding)
                throw new PlatformNotSupportedException("VertexAttribBinding (separat vertex attributes) are not supported by the driver");

            int layout;
            if (inputLayoutPool.TryGetValue(description, out layout))
                return layout;
            
            layout = GL.GenVertexArray();

            if (OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                BindManager.VertexArray = layout;

                int offset = 0;
                VertexElement[] elements = description.GetElements();
                for (int i = 0; i < description.ElementCount; i++)
                {
                    if (offset > OpenGLCapabilities.MaxVertexAttribBindingOffset)
                        throw new PlatformNotSupportedException("offset is higher than maximum supportet offset.");

                    GL.EnableVertexAttribArray(i);

                    GL.VertexAttribBinding(i, 0);
                    GL.VertexAttribFormat(i, GetComponentsOf(elements[i].Type), VertexAttribType.Float, false, offset);
                    offset += GetSizeOf(elements[i].Type);
                }
            }
            else if (OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                int offset = 0;
                VertexElement[] elements = description.GetElements();
                for (int i = 0; i < description.ElementCount; i++)
                {
                    if (offset > OpenGLCapabilities.MaxVertexAttribBindingOffset)
                        throw new PlatformNotSupportedException("offset is higher than maximum supportet offset.");
                    
                    Ext.EnableVertexArrayAttrib(layout, i);

                    Ext.VertexArrayVertexAttribBinding(layout, i, 0);
                    Ext.VertexArrayVertexAttribFormat(layout, i, GetComponentsOf(elements[i].Type), (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)VertexAttribType.Float, false, offset);
                    offset += GetSizeOf(elements[i].Type);
                }
            }

            inputLayoutPool.Add(description, layout);
            return layout;
        }

        internal void CheckGLError(string msg = "N/A")
        {
            if (!CreationFlags.HasFlag(DeviceCreationFlags.Debug))
                return;

            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                throw new InvalidOperationException(string.Format("OpenGL threw an error at \"{0}\": {1}", msg, error.ToString()));
            }
        }

        public void Dispose()
        {
            Log.Info("GraphicsDevice.Dispose() called!");
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (Factory != null)
                Factory.Dispose();
            if (Context != null)
                Context.Dispose();
            IsDisposed = true; 
            GC.SuppressFinalize(this);
        }
    }
}
