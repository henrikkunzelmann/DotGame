﻿using System;
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
        ITexture2D LoadTexture2D(string file, bool generateMipMaps);
        ITexture2D CreateTexture2D(int width, int height, TextureFormat format, bool generateMipMaps);
        ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format, bool generateMipMaps);
        ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize);
        ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize);
        IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format, bool generateMipMaps);
        IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format, bool generateMipMaps);
        IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, bool generateMipMaps, int arraySize);
        IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, bool generateMipMaps, int arraySize);

        IVertexBuffer CreateVertexBuffer(int vertexCount, VertexDescription description, BufferUsage usage);
        IVertexBuffer CreateVertexBuffer<T>(T[] vertices, VertexDescription description, BufferUsage usage) where T : struct;
        IIndexBuffer CreateIndexBuffer(int indexCount, IndexFormat format, BufferUsage usage);
        IIndexBuffer CreateIndexBuffer<T>(T[] indices, IndexFormat format, BufferUsage usage) where T : struct;
        IIndexBuffer CreateIndexBuffer(int[] indices, BufferUsage usage);
        IIndexBuffer CreateIndexBuffer(uint[] indices, BufferUsage usage);
        IIndexBuffer CreateIndexBuffer(short[] indices, BufferUsage usage);
        IIndexBuffer CreateIndexBuffer(ushort[] indices, BufferUsage usage);

        IConstantBuffer CreateConstantBuffer(int sizeBytes, BufferUsage usage);
        IConstantBuffer CreateConstantBuffer<T>(T data, BufferUsage usage) where T : struct; 

        IShader CompileShader(string name, ShaderCompileInfo vertexInfo, ShaderCompileInfo pixelInfo);
        IShader CreateShader(string name, byte[] code);

        IRenderState CreateRenderState(RenderStateInfo info);
        ISampler CreateSampler(SamplerInfo info);
        IRasterizerState CreateRasterizerState(RasterizerStateInfo info);
        IDepthStencilState CreateDepthStencilState(DepthStencilStateInfo info);
        IBlendState CreateBlendState(BlendStateInfo info);
    }
}
