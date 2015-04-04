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
    public interface IGraphicsFactory : IGraphicsObject
    {
        ITexture2D LoadTexture2D(string file);
        ITexture2D CreateTexture2D(int width, int height, TextureFormat format);
        ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format);
        ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, int arraySize);
        ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, int arraySize);
        IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format);
        IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format);
        IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, int arraySize);
        IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, int arraySize);

        IVertexBuffer CreateVertexBuffer<T>(T[] vertices, VertexDescription description) where T : struct;
        IIndexBuffer CreateIndexBuffer<T>(T[] indices, IndexFormat format) where T : struct;
        IIndexBuffer CreateIndexBuffer(int[] indices);
        IIndexBuffer CreateIndexBuffer(uint[] indices);
        IIndexBuffer CreateIndexBuffer(short[] indices);
        IIndexBuffer CreateIndexBuffer(ushort[] indices);

        IConstantBuffer CreateConstantBuffer(int size);

        IShader CompileShader(string name, ShaderCompileInfo vertexInfo, ShaderCompileInfo pixelInfo);
        IShader CreateShader(string name, byte[] vertexCode, byte[] pixelCode);

        IRenderState CreateRenderState(RenderStateInfo info);
        ISampler CreateSampler(SamplerInfo info);
        IRasterizerState CreateRasterizerState(RasterizerStateInfo info);
    }
}
