using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt Methoden zum Erstellen von Ressourcen bereit.
    /// </summary>
    public interface IGraphicsFactory : IDisposable
    {
        ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataRectangle data = new DataRectangle());
        ITexture2D CreateTexture2D(int width, int height, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data);
        ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, DataBox data = new DataBox());
        ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data);
        ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data);
        ITexture2DArray CreateTexture2DArray(int width, int height, int arraySize, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data);
        ITexture3DArray CreateTexture3DArray(int width, int height, int length, int arraySize, TextureFormat format, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data);
        ITexture3DArray CreateTexture3DArray(int width, int height, int length, int arraySize, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data);
        IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format, bool generateMipMaps);
        IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps);
        IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize);
        IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize);

        IVertexBuffer CreateVertexBuffer(int vertexCount, VertexDescription description, ResourceUsage usage);
        IVertexBuffer CreateVertexBuffer<T>(T[] vertices, VertexDescription description, ResourceUsage usage) where T : struct;
        IIndexBuffer CreateIndexBuffer(int indexCount, IndexFormat format, ResourceUsage usage);
        IIndexBuffer CreateIndexBuffer<T>(T[] indices, IndexFormat format, ResourceUsage usage) where T : struct;
        IIndexBuffer CreateIndexBuffer(int[] indices, ResourceUsage usage);
        IIndexBuffer CreateIndexBuffer(uint[] indices, ResourceUsage usage);
        IIndexBuffer CreateIndexBuffer(short[] indices, ResourceUsage usage);
        IIndexBuffer CreateIndexBuffer(ushort[] indices, ResourceUsage usage);

        IConstantBuffer CreateConstantBuffer(int sizeBytes, ResourceUsage usage);
        IConstantBuffer CreateConstantBuffer<T>(T data, ResourceUsage usage) where T : struct; 

        IShader CompileShader(string name, ShaderCompileInfo vertexInfo, ShaderCompileInfo pixelInfo);
        IShader CreateShader(string name, byte[] code);

        IRenderState CreateRenderState(RenderStateInfo info);
        ISampler CreateSampler(SamplerInfo info);
        IRasterizerState CreateRasterizerState(RasterizerStateInfo info);
        IDepthStencilState CreateDepthStencilState(DepthStencilStateInfo info);
        IBlendState CreateBlendState(BlendStateInfo info);
    }
}
