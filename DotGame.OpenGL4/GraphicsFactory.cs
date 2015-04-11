using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DotGame.Utils;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : GraphicsObject, IGraphicsFactory
    {
        internal readonly List<GraphicsObject> DeferredDispose; // Wird zum disposen benutzt, um MakeCurrent Aufrufe zu vermeiden. Siehe DisposeUnused und Referenzen dazu.
        internal ReadOnlyCollection<WeakReference<GraphicsObject>> Objects { get { return objects.AsReadOnly(); } }
        private readonly List<WeakReference<GraphicsObject>> objects;

        internal GraphicsFactory(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {
            DeferredDispose = new List<GraphicsObject>();
            objects = new List<WeakReference<GraphicsObject>>();
        }
        
        public ITexture2D LoadTexture2D(string file, bool generateMipMaps)
        {
            // TODO (henrik1235) generateMipMaps benutzen
            AssertCurrent();

            Bitmap bitmap = new Bitmap(file);
            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Texture2D texture = Register(new Texture2D(graphicsDevice, bitmap.Width, bitmap.Height, 1, generateMipMaps, TextureFormat.RGBA8_UIntNorm));
            texture.SetData(bmpData.Scan0, 0, bmpData.Stride * bmpData.Width * bmpData.Height);
            bitmap.UnlockBits(bmpData);

            return texture;
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            Texture2D texture = Register(new Texture2D(graphicsDevice, width, height, generateMipMaps, format));
            texture.SetData(IntPtr.Zero, 0, 0);

            return texture;
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return Register(new Texture3D(graphicsDevice, width, height, length, generateMipMaps, format));
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            throw new NotSupportedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return (IRenderTarget2D)CreateTexture2D(width, height, format, generateMipMaps);
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return Register(new Texture3D(graphicsDevice, width, height,length, generateMipMaps, format));
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            throw new NotSupportedException();
        }

        public IVertexBuffer CreateVertexBuffer(int vertexCount, VertexDescription description, BufferUsage usage)
        {
            AssertCurrent();

            return Register(new VertexBuffer(graphicsDevice, description, usage));
        }

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description, BufferUsage usage) where T : struct
        {
            AssertCurrent();

            VertexBuffer buffer = Register(new VertexBuffer(graphicsDevice, description, usage));
            buffer.SetData<T>(data);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int indexCount, IndexFormat format, BufferUsage usage)
        {
            AssertCurrent();
            return Register(new IndexBuffer(graphicsDevice, usage, format));
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data, IndexFormat format, BufferUsage usage) where T : struct
        {
            AssertCurrent();
            IndexBuffer buffer = Register(new IndexBuffer(graphicsDevice, usage, format));
            buffer.SetData<T>(data);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int[] data, BufferUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.Int32, usage);
        }

        public IIndexBuffer CreateIndexBuffer(uint[] data, BufferUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.UInt32, usage);
        }

        public IIndexBuffer CreateIndexBuffer(short[] data, BufferUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.Short16, usage);
        }

        public IIndexBuffer CreateIndexBuffer(ushort[] data, BufferUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.UShort16, usage);
        }

        public IConstantBuffer CreateConstantBuffer(int size, BufferUsage usage)
        {
            return new ConstantBuffer(graphicsDevice, size, usage);
        }

        public IConstantBuffer CreateConstantBuffer<T>(T data, BufferUsage usage) where T : struct
        {
            throw new NotImplementedException();
        }

        public IShader CompileShader(string name, ShaderCompileInfo vertex, ShaderCompileInfo pixel)
        {
            AssertCurrent();

            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is empty or whitespace.", "name");
            
            if (vertex.File == null)
                throw new ArgumentException("File of info is null.", "vertex");
            if (pixel.File == null)
                throw new ArgumentException("File of info is null.", "pixel");
            
            string vertexCode = File.ReadAllText(vertex.File);
            vertexCode = CheckShaderVersion(name, vertexCode, vertex);

            string pixelCode = File.ReadAllText(pixel.File);
            pixelCode = CheckShaderVersion(name, pixelCode, pixel);

            return Register(new Shader(graphicsDevice, vertexCode, pixelCode));
        }

        private string CheckShaderVersion(string shaderName, string shaderCode, ShaderCompileInfo info)
        {
            string code = shaderCode.TrimStart('\r', '\n', ' ');

            string[] shaderVersion = code.Substring(0, shaderCode.IndexOf(Environment.NewLine)).Split(' ');
            if (shaderVersion.Length > 1 && shaderVersion[0] == "#version")
            {
                //GLSL Version string auslesen
                string glslVersionString = shaderVersion[1];
                if (string.IsNullOrWhiteSpace(glslVersionString) || glslVersionString.Length < 2)
                    throw new Exception("Could not determine GLSL version for shader " + shaderName);

                string glslVersionStringMajor = glslVersionString.Substring(0, 1);
                string glslVersionStringMinor = glslVersionString.Substring(1, 1);

                int glslVersionMajor;
                int glslVersionMinor;
                if (!int.TryParse(glslVersionStringMajor, out glslVersionMajor) || !int.TryParse(glslVersionStringMinor, out glslVersionMinor))
                    throw new Exception("Could not determine GLSL version for shader " + shaderName);

                Version shaderGLSLVersion = new Version(glslVersionMajor, glslVersionMinor);

                //Überprüfen, ob angegebene GLSL version vom Treiber unterstützt wird
                if (graphicsDevice.OpenGLCapabilities.GLSLVersion < shaderGLSLVersion)
                    throw new PlatformNotSupportedException(string.Format("GLSL version of shader {0} is not supported by the driver.", shaderName));
            }
            else
            {
                //Keine Version im Sourcecode angegeben => Version aus info benutzen

                if (string.IsNullOrWhiteSpace(info.Version))
                    throw new ArgumentException("Neither the shader source nor info provide a version for the compiler");

                code = "#version " + EnumConverter.ConvertToGLSLVersion(info.Version) + " core" + Environment.NewLine + code;
            }

            return code;
        }

        public IShader CreateShader(string name, byte[] code)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (code == null)
                throw new ArgumentNullException("code");

            if (!GraphicsDevice.Capabilities.SupportsBinaryShaders)
                throw new NotSupportedException("Creating shaders by byte code is not supported.");

            return new Shader(graphicsDevice, code);
        }

        public IRenderState CreateRenderState(RenderStateInfo info)
        {
            return new RenderState(graphicsDevice, info);
        }

        public IRasterizerState CreateRasterizerState(RasterizerStateInfo info)
        {
            return new RasterizerState(graphicsDevice, info);
        }

        public ISampler CreateSampler(SamplerInfo info)
        {
            return Register(new Sampler(graphicsDevice, info));
        }

        public IDepthStencilState CreateDepthStencilState(DepthStencilStateInfo info)
        {
            return new DepthStencilState(graphicsDevice, info);
        }

        public IBlendState CreateBlendState(BlendStateInfo info)
        {
            return new BlendState(graphicsDevice, info);
        }

        internal Fbo CreateFbo(int depth, params int[] color)
        {
            return Register(new Fbo(graphicsDevice, depth, color));
        }

        internal static void CheckGLError()
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                throw new InvalidOperationException(error.ToString());
            }
        }

        private T Register<T>(T obj) where T : GraphicsObject
        {
            objects.Add(new WeakReference<GraphicsObject>(obj));
            return obj;
        }

        internal void DisposeUnused()
        {
            // TODO (Joex3): Wieder eigene Exception? 
            if (!graphicsDevice.IsDisposed && !graphicsDevice.IsCurrent)
                throw new Exception("DisposeUnused must be called in the render thread, or after the GraphicsDevice has been disposed.");

            if (DeferredDispose.Count > 0)
            {
                foreach (var obj in DeferredDispose)     
                {
                    // TODO: (henrik1235) Klären ob die Liste nicht in IGraphicsDevice verschieben können
                    if (obj is GraphicsFactory)
                        return;

                    // Das sollte eigentlich nie passieren.
                    if (obj.IsDisposed)
                    {
                        Log.Debug("{0} is in DeferredDispose queue, but its already disposed.", obj.GetType().FullName);
                        continue;
                    }

                    DotGame.Utils.Log.Warning("{0} is not disposed! {1}", obj.GetType().FullName, obj);

                    obj.Dispose(); // TODO: (henrik1235) Überprüfen ob wir nicht obj.Dispose(bool isDisposing) aufrufen sollten, inbesondere weil dann die DeferredDispose Liste verändert werden kann
                }
                DeferredDispose.Clear();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            DisposeUnused();
            // TODO (Joex3): Alle restlichen Objekte disposen.
        }
    }
}