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
            BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Texture2D texture = Register(new Texture2D(graphicsDevice, bitmap.Width, bitmap.Height, 1, TextureFormat.RGBA16_UIntNorm, bmpData.Scan0));
            bitmap.UnlockBits(bmpData);

            return texture;
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return Register(new Texture2D(graphicsDevice, width, height, 0, format));
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            return Register(new Texture2D(graphicsDevice, width,height, 1, format));
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description) where T : struct
        {
            AssertCurrent();

            VertexBuffer buffer = Register(new VertexBuffer(graphicsDevice, description));
            buffer.SetData<T>(data);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data, IndexFormat format) where T : struct
        {
            AssertCurrent();

            IndexBuffer buffer = Register(new IndexBuffer(graphicsDevice));
            buffer.SetData<T>(data, format);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.Int32);
        }

        public IIndexBuffer CreateIndexBuffer(uint[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.UInt32);
        }

        public IIndexBuffer CreateIndexBuffer(short[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.Short16);
        }

        public IIndexBuffer CreateIndexBuffer(ushort[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.UShort16);
        }

        public IConstantBuffer CreateConstantBuffer(int size)
        {
            return new ConstantBuffer(graphicsDevice, size);
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

                //Überprüfen, ob angegebene GLSL version vom Treiber unterstützt wird
                GraphicsDevice internalGraphicsDevice = (GraphicsDevice)GraphicsDevice;
                if (internalGraphicsDevice.GLSLVersionMajor < glslVersionMajor || (internalGraphicsDevice.GLSLVersionMajor == glslVersionMajor && internalGraphicsDevice.GLSLVersionMinor < glslVersionMinor))
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

            return new Shader(graphicsDevice, code);
        }

        public IRenderState CreateRenderState(RenderStateInfo info)
        {
            return new RenderState(graphicsDevice, info);
        }

        public IRasterizerState CreateRasterizerState(RasterizerStateInfo info)
        {
            return Register(new RasterizerState(graphicsDevice, info));
        }

        public ISampler CreateSampler(SamplerInfo info)
        {
            return Register(new Sampler(graphicsDevice, info));
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