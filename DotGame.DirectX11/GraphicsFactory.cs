using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using SharpDX.D3DCompiler;
using DotGame.Graphics;
using System.Runtime.InteropServices;

namespace DotGame.DirectX11
{
    public class GraphicsFactory : GraphicsObject, IGraphicsFactory
    {
        internal GraphicsFactory(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("graphicsDevice is disposed.", "graphicsDevice");
            this.graphicsDevice = graphicsDevice;
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataRectangle data = new DataRectangle())
        {
            return new Texture2D(graphicsDevice, width, height, format, false, generateMipMaps, usage, data);
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            return new Texture2D(graphicsDevice, width, height, format, mipLevels, usage, data);
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataBox data = new DataBox())
        {
            throw new NotImplementedException();
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data)
        {
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            return new Texture2D(graphicsDevice, width, height, format, arraySize, false, generateMipMaps, usage, data);
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
        {
            return new Texture2D(graphicsDevice, width, height, format, arraySize, mipLevels, usage, data);
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
            return new Texture2D(graphicsDevice, width, height, format, 1, true, generateMipMaps);
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            return new Texture2D(graphicsDevice, width, height, format, arraySize, true, generateMipMaps);
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize)
        {
            throw new NotImplementedException();
        }

        public IVertexBuffer CreateVertexBuffer(int vertexCount, VertexDescription description, ResourceUsage usage) 
        {
            return new VertexBuffer(graphicsDevice, vertexCount, description, usage);
        }

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description, ResourceUsage usage) where T : struct
        {
            VertexBuffer buffer = null;
            if (data != null)
            {
                GCHandle handle;
                DataArray dataArray = DataArray.FromArray(data, out handle);
                try
                {
                    buffer = new VertexBuffer(graphicsDevice, description, usage, dataArray);
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
            return new IndexBuffer(graphicsDevice, format, usage, indexCount);
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data, IndexFormat format, ResourceUsage usage) where T : struct
        {
            IndexBuffer buffer = null;
            if (data != null)
            {
                GCHandle handle;
                DataArray dataArray = DataArray.FromArray(data, out handle);
                try
                {
                    buffer = new IndexBuffer(graphicsDevice, format, usage, dataArray);
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
            return new ConstantBuffer(graphicsDevice, usage, size);
        }

        public IConstantBuffer CreateConstantBuffer<T>(T data, ResourceUsage usage) where T : struct
        {
            ConstantBuffer buffer = null;
            GCHandle handle;
            DataArray dataArray = DataArray.FromObject(data, out handle);
            try
            {
                buffer = new ConstantBuffer(graphicsDevice, usage, dataArray);
            }
            finally
            {
                handle.Free();
            }

            return buffer;
        }

        public IShader CompileShader(string name, ShaderCompileInfo vertexInfo, ShaderCompileInfo pixelInfo)
        { 
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is empty or whitespace.", "name");
            CheckCompileInfo(vertexInfo, "vertexInfo");
            CheckCompileInfo(pixelInfo, "pixelInfo");

            ShaderBytecode vertexCode, pixelCode;
            using (IncludeHandler include = IncludeHandler.CreateForShader(vertexInfo.File))
                vertexCode = ShaderBytecode.CompileFromFile(vertexInfo.File, vertexInfo.Function, vertexInfo.Version, ShaderFlags.None, EffectFlags.None, null, include);
            using (IncludeHandler include = IncludeHandler.CreateForShader(pixelInfo.File))
                pixelCode = ShaderBytecode.CompileFromFile(pixelInfo.File, pixelInfo.Function, pixelInfo.Version, ShaderFlags.None, EffectFlags.None, null, include);
            return new Shader(graphicsDevice, name, vertexCode, pixelCode);
        }

        private void CheckCompileInfo(ShaderCompileInfo info, string parameterName)
        {
            if (info.File == null)
                throw new ArgumentException("File of info is null.", parameterName);
            if (info.Function == null)
                throw new ArgumentException("Function of info is null.", parameterName);
            if (info.Version == null)
                throw new ArgumentException("Version of info is null.", parameterName);
        }

        public IShader CreateShader(string name, byte[] binaryCode)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is empty or whitespace.", "name");
            if (binaryCode == null)
                throw new ArgumentNullException("binaryCode");

            using(MemoryStream stream = new MemoryStream(binaryCode))
            using(BinaryReader reader = new BinaryReader(stream))
            {
                if (reader.ReadString() != "DIRECTX11")
                    throw new ArgumentException("Invalid header of binary code.", "binaryCode");
                int vertexSize = reader.ReadInt32();
                byte[] vertex = reader.ReadBytes(vertexSize);
                int pixelSize = reader.ReadInt32();
                byte[] pixel = reader.ReadBytes(pixelSize);
                return new Shader(graphicsDevice, name, new ShaderBytecode(vertex), new ShaderBytecode(pixel));
            }
        }

        public IRenderState CreateRenderState(RenderStateInfo info)
        {
            return new RenderState(graphicsDevice, info);
        }

        public ISampler CreateSampler(SamplerInfo info)
        {
            return new Sampler(graphicsDevice, info);  
        }

        public IRasterizerState CreateRasterizerState(RasterizerStateInfo info)
        {
            return new RasterizerState(graphicsDevice, info);
        }

        public IDepthStencilState CreateDepthStencilState(DepthStencilStateInfo info)
        {
            return new DepthStencilState(graphicsDevice, info);
        }

        public IBlendState CreateBlendState(BlendStateInfo info)
        {
            return new BlendState(graphicsDevice, info);
        }

        protected override void Dispose(bool isDisposing)
        {
        }

        private class IncludeHandler : Include
        {
            public string Directory { get; private set; }

            public IncludeHandler(string directory)
            {
                if (directory == null)
                    throw new ArgumentNullException("directory");
                this.Directory = directory;
            }

            public static IncludeHandler CreateForShader(string path)
            {
                if (path == null)
                    throw new ArgumentNullException("path");
                return new IncludeHandler(Path.GetDirectoryName(path));
            }

            public void Close(Stream stream)
            {
                stream.Close();
            }

            public Stream Open(IncludeType type, string fileName, Stream parentStream)
            {
                return new FileStream(Path.Combine(Directory, fileName), FileMode.Open, FileAccess.Read);
            }

            public void Dispose()
            {
            }

            public IDisposable Shadow 
            { 
                get { return null; } 
                set { } 
            }
        }
    }
}