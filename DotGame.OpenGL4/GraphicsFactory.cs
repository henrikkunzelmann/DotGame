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
using System.Runtime.InteropServices;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : IGraphicsFactory
    {
        private readonly List<GraphicsObject> deferredDispose = new List<GraphicsObject>();
        internal List<GraphicsObject> DeferredDispose { get { return deferredDispose; } } // Wird zum disposen benutzt, um MakeCurrent Aufrufe zu vermeiden. Siehe DisposeUnused und Referenzen dazu.
        private readonly List<WeakReference<GraphicsObject>> objects;
        internal ReadOnlyCollection<WeakReference<GraphicsObject>> Objects { get { return objects.AsReadOnly(); } }

        private GraphicsDevice graphicsDevice;
        public bool IsDisposed { get; private set; }
        public event EventHandler<EventArgs> OnDisposing;
        public event EventHandler<EventArgs> OnDisposed;

        internal GraphicsFactory(GraphicsDevice graphicsDevice)
        {
            objects = new List<WeakReference<GraphicsObject>>();
            this.graphicsDevice = graphicsDevice;
        }
        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataRectangle data = new DataRectangle())
        {
            AssertCurrent();
            return Register(new Texture2D(graphicsDevice, width, height, format, generateMipMaps, usage, data));
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            AssertCurrent();
            return Register(new Texture2D(graphicsDevice, width, height, format, mipLevels, usage, data));
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataBox data = new DataBox())
        {
            AssertCurrent();
            return Register(new Texture3D(graphicsDevice, width, height, length, format, generateMipMaps, usage, data));
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data)
        {
            AssertCurrent();
            return Register(new Texture3D(graphicsDevice, width, height, length, format, mipLevels, usage, data));
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            AssertCurrent();
            return Register(new Texture2DArray(graphicsDevice, width, height, arraySize, format, generateMipMaps, usage, data));
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            AssertCurrent();
            return Register(new Texture2DArray(graphicsDevice, width, height, arraySize, format, mipLevels, usage, data));
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, int arraySize, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data)
        {
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, int arraySize, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return (IRenderTarget2D)CreateTexture2D(width, height, format, generateMipMaps);
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return (IRenderTarget3D)CreateTexture3D(width, height, length, format, generateMipMaps); ;
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            return Register(new Texture2DArray(graphicsDevice, width, height, arraySize,format, false));
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            throw new NotSupportedException();
        }

        public IVertexBuffer CreateVertexBuffer(int vertexCount, VertexDescription description, ResourceUsage usage)
        {
            AssertCurrent();
            return Register(new VertexBuffer(graphicsDevice, description, usage, vertexCount));
        }

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description, ResourceUsage usage) where T : struct
        {
            AssertCurrent();

            VertexBuffer buffer = null;
            if (data != null)
            {
                GCHandle handle;
                DataArray dataArray = DataArray.FromArray(data, out handle);
                try
                {
                    buffer = Register(new VertexBuffer(graphicsDevice, description, usage, dataArray));
                }
                finally
                {
                    handle.Free();
                }
            }
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int indexCount, IndexFormat format, ResourceUsage usage)
        {
            AssertCurrent();
            return Register(new IndexBuffer(graphicsDevice, format, usage, indexCount));
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data, IndexFormat format, ResourceUsage usage) where T : struct
        {
            AssertCurrent();
            IndexBuffer buffer = null;
            if (data != null)
            {
                GCHandle handle;
                DataArray dataArray = DataArray.FromArray(data, out handle);
                try
                {
                    buffer = Register(new IndexBuffer(graphicsDevice, format, usage, dataArray));
                }
                finally
                {
                    handle.Free();
                }
            }
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int[] data, ResourceUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.Int32, usage);
        }

        public IIndexBuffer CreateIndexBuffer(uint[] data, ResourceUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.UInt32, usage);
        }

        public IIndexBuffer CreateIndexBuffer(short[] data, ResourceUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.Short16, usage);
        }

        public IIndexBuffer CreateIndexBuffer(ushort[] data, ResourceUsage usage)
        {
            return CreateIndexBuffer(data, IndexFormat.UShort16, usage);
        }

        public IConstantBuffer CreateConstantBuffer(int size, ResourceUsage usage)
        {
            return Register(new ConstantBuffer(graphicsDevice, size, usage));
        }

        public IConstantBuffer CreateConstantBuffer<T>(T data, ResourceUsage usage) where T : struct
        {
            AssertCurrent();
            ConstantBuffer constantBuffer = null;
            GCHandle handle;
            DataArray dataArray = DataArray.FromObject<T>(data, out handle);
            try
            {
                constantBuffer = Register(new ConstantBuffer(graphicsDevice, usage, dataArray));
            }
            finally
            {
                handle.Free();
            }
            return constantBuffer;
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
                    throw new GraphicsException("Could not determine GLSL version for shader " + shaderName);

                string glslVersionStringMajor = glslVersionString.Substring(0, 1);
                string glslVersionStringMinor = glslVersionString.Substring(1, 1);

                int glslVersionMajor;
                int glslVersionMinor;
                if (!int.TryParse(glslVersionStringMajor, out glslVersionMajor) || !int.TryParse(glslVersionStringMinor, out glslVersionMinor))
                    throw new GraphicsException("Could not determine GLSL version for shader " + shaderName);

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

            if (!graphicsDevice.Capabilities.SupportsBinaryShaders)
                throw new NotSupportedException("Creating shaders by byte code is not supported.");

            return Register(new Shader(graphicsDevice, code));
        }

        public IRenderState CreateRenderState(RenderStateInfo info)
        {
            return Register(new RenderState(graphicsDevice, info));
        }

        public IRasterizerState CreateRasterizerState(RasterizerStateInfo info)
        {
            return Register(new RasterizerState(graphicsDevice, info));
        }

        public ISampler CreateSampler(SamplerInfo info)
        {
            return Register(new Sampler(graphicsDevice, info));
        }

        public IDepthStencilState CreateDepthStencilState(DepthStencilStateInfo info)
        {
            return Register(new DepthStencilState(graphicsDevice, info));
        }

        public IBlendState CreateBlendState(BlendStateInfo info)
        {
            return Register(new BlendState(graphicsDevice, info));
        }

        internal FrameBuffer CreateFrameBuffer(FrameBufferDescription description)
        {
            return Register(new FrameBuffer(graphicsDevice, description));
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
                throw new InvalidOperationException("DisposeUnused must be called in the render thread, or after the GraphicsDevice has been disposed.");

            if (DeferredDispose.Count > 0)
            {
                foreach (var obj in DeferredDispose)     
                {
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

        internal void DisposeAll()
        {
            // TODO (Robin): Auch hier wieder eigene Exception? 
            if (!graphicsDevice.IsDisposed && !graphicsDevice.IsCurrent)
                throw new InvalidOperationException("DisposeAll must be called in the render thread, or after the GraphicsDevice has been disposed.");

            if (objects.Count > 0)
            {
                foreach (var obj in objects)
                {
                    GraphicsObject graphicsObject;

                    if(!obj.TryGetTarget(out graphicsObject) || graphicsObject.IsDisposed)
                        continue;   

                    graphicsObject.Dispose(); // TODO: (henrik1235) Überprüfen ob wir nicht obj.Dispose(bool isDisposing) aufrufen sollten, inbesondere weil dann die DeferredDispose Liste verändert werden kann
                }
                objects.Clear();
            }
        }

        protected void AssertNotDisposed()
        {
            if (graphicsDevice.IsDisposed)
                throw new ObjectDisposedException(graphicsDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected void AssertCurrent()
        {
            AssertNotDisposed();

            if (!graphicsDevice.IsCurrent)
                throw new GraphicsDeviceNotCurrentException(graphicsDevice);
        }

        private void Dispose(bool isDisposing)
        {
            DisposeAll();
            DeferredDispose.Clear();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            if (OnDisposing != null)
                OnDisposing(this, EventArgs.Empty);

            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);

            if (OnDisposed != null)
                OnDisposed(this, EventArgs.Empty);
        }        
    }
}