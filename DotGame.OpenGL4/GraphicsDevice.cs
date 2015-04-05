using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.Graphics;
using DotGame.Utils;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using DotGame.OpenGL4.Windows;

namespace DotGame.OpenGL4
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }
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

        internal StateManager StateManager { get; private set; }

        private IWindowContainer container;

        internal int GLSLVersionMajor { get; private set; }
        internal int GLSLVersionMinor { get; private set; }
        internal int OpenGLVersionMajor { get; private set; }
        internal int OpenGLVersionMinor { get; private set; }
        internal bool HasAnisotropicFiltering { get; private set; }
        internal bool HasS3TextureCompression { get; private set; }

        internal static int MipLevels(int width, int height, int depth = 0)
        {
            var max = Math.Max(width, Math.Max(height, depth));
            return (int)Math.Ceiling(Math.Log(max, 2));
        }

        public GraphicsDevice(IGameWindow window, IWindowContainer container, GraphicsContext context)
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
            
            Log.Debug("Got context: [ColorFormat: {0}, Depth: {1}, Stencil: {2}, FSAA Samples: {3}, AccumulatorFormat: {4}, Buffers: {5}, Stereo: {6}]",
                Context.GraphicsMode.ColorFormat,
                Context.GraphicsMode.Depth,
                Context.GraphicsMode.Stencil,
                Context.GraphicsMode.Samples,
                Context.GraphicsMode.AccumulatorFormat,
                Context.GraphicsMode.Buffers,
                Context.GraphicsMode.Stereo);

            Context.LoadAll();

            CheckVersion();

            Factory = new GraphicsFactory(this);
            StateManager = new OpenGL4.StateManager(this);
            this.RenderContext = new RenderContext(this);

            Context.MakeCurrent(null);
        }
        ~GraphicsDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void MakeCurrent()
        {
            Context.MakeCurrent(container.WindowInfo);
        }
        public void DetachCurrent()
        {
            Context.MakeCurrent(null);
        }

        private void CheckVersion()
        {
            OpenGLVersionMajor = GL.GetInteger(GetPName.MajorVersion);
            OpenGLVersionMinor = GL.GetInteger(GetPName.MinorVersion);

            Log.Debug(string.Format("OpenGL Version: {0}.{1}",OpenGLVersionMajor,OpenGLVersionMinor));

            //GLSL Version string auslesen
            string glslVersionString = GL.GetString(StringName.ShadingLanguageVersion);
            if(string.IsNullOrWhiteSpace(glslVersionString))
                throw new Exception("Could not determine supported GLSL version");

            string glslVersionStringMajor = glslVersionString.Substring(0, glslVersionString.IndexOf('.'));
            string glslVersionStringMinor = glslVersionString.Substring(glslVersionString.IndexOf('.') + 1, 1);

            int glslVersionMajor;
            int glslVersionMinor;
            if (!int.TryParse(glslVersionStringMajor, out glslVersionMajor) || !int.TryParse(glslVersionStringMinor, out glslVersionMinor))
                throw new Exception("Could not determine supported GLSL version");

            GLSLVersionMajor = glslVersionMajor;
            GLSLVersionMinor = glslVersionMinor;

            Log.Debug(string.Format("GLSL Version: {0}.{1}",GLSLVersionMajor,GLSLVersionMinor));

            //Extensions überprüfen
            int extensionCount = GL.GetInteger(GetPName.NumExtensions);
            for (int i = 0; i < extensionCount; i++)
            {
                string extension = GL.GetString(StringNameIndexed.Extensions, i);
                if (extension == "GL_EXT_texture_compression_s3tc")
                {
                    HasS3TextureCompression = true;
                }

                if (extension == "GL_EXT_texture_filter_anisotropic")
                {
                    HasAnisotropicFiltering = true;
                }
            }
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public T Cast<T>(IGraphicsObject obj, string parameterName) where T : class, IGraphicsObject
        {
            T ret = obj as T;
            if (ret == null)
                throw new ArgumentException("GraphicsObject is not part of this api.", parameterName);
            if (obj.GraphicsDevice != this)
                throw new ArgumentException("GraphicsObject is not part of this graphics device.", parameterName);
            return ret;
        }

        public int GetSizeOf(TextureFormat format)
        {
            throw new NotImplementedException();
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
            // TODO (Joex3): Evtl. woanders hinschieben.
            ((GraphicsFactory)Factory).DisposeUnused();

            Context.SwapBuffers();
        }

        internal static void CheckGLError()
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                throw new InvalidOperationException(error.ToString());
            }
        }

        public void Dispose()
        {
            Log.Info("GraphicsDevice.Dispose() called!");
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            Factory.Dispose();
            Context.Dispose();
            IsDisposed = true;
        }
    }
}
