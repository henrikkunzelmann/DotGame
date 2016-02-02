using DotGame.Graphics;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceUsage = DotGame.Graphics.ResourceUsage;

namespace DotGame.DirectX11
{
    public class RenderUpdateContext : GraphicsObject, IRenderUpdateContext
    {
        private RenderContext renderContext;
        public IRenderContext RenderContext
        {
            get
            {
                return renderContext;
            }
        }

        public RenderUpdateContext(GraphicsDevice device, RenderContext context) : base(device, new System.Diagnostics.StackTrace())
        {
            if (device == null)
                throw new ArgumentNullException("device");

            if (context == null)
                throw new ArgumentNullException("context");
            
            this.renderContext = context;
        }

        public void Update(IConstantBuffer buffer, DataArray data)
        {
            var dxBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");
            if (dxBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "buffer");

            if (data.Size != buffer.SizeBytes)
                throw new ArgumentException("Data does not match ConstantBuffer size.", "data");

            if (dxBuffer.Usage == ResourceUsage.Normal)
                renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, 0, 0), dxBuffer.Buffer);
            else
            {
                MapMode mapMode = dxBuffer.Usage == ResourceUsage.Dynamic ? MapMode.WriteDiscard : MapMode.Write;

                SharpDX.DataBox box = renderContext.Context.MapSubresource(dxBuffer.Buffer, 0, mapMode, MapFlags.None);
                SharpDX.Utilities.CopyMemory(box.DataPointer, data.Pointer, data.Size);
                renderContext.Context.UnmapSubresource(dxBuffer.Buffer, 0);
            }
        }
                        

        public void Update(IVertexBuffer buffer, DataArray data)
        {
            var dxBuffer = graphicsDevice.Cast<VertexBuffer>(buffer, "buffer");
            if (dxBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "buffer");

            if (data.Size != buffer.SizeBytes)
                throw new ArgumentException("Data does not match VertexBuffer size.", "data");
            
            if (dxBuffer.Usage == ResourceUsage.Normal)
                renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, 0, 0), dxBuffer.Buffer);
            else
            {
                MapMode mapMode = dxBuffer.Usage == ResourceUsage.Dynamic ? MapMode.WriteDiscard : MapMode.Write;

                SharpDX.DataBox box = renderContext.Context.MapSubresource(dxBuffer.Buffer, 0, mapMode, MapFlags.None);
                SharpDX.Utilities.CopyMemory(box.DataPointer, data.Pointer, data.Size);
                renderContext.Context.UnmapSubresource(dxBuffer.Buffer, 0);
            }
        }

        public void Update(IIndexBuffer buffer, DataArray data, int count)
        {
            var dxBuffer = graphicsDevice.Cast<IndexBuffer>(buffer, "buffer");
            if (dxBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "buffer");

            if (data.Size != buffer.SizeBytes)
                throw new ArgumentException("Data does not match IndexBuffer size.", "data");

            if (dxBuffer.Usage == ResourceUsage.Normal)
                renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, 0, 0), dxBuffer.Buffer);
            else
            {
                MapMode mapMode = dxBuffer.Usage == ResourceUsage.Dynamic ? MapMode.WriteDiscard : MapMode.Write;

                SharpDX.DataBox box = renderContext.Context.MapSubresource(dxBuffer.Buffer, 0, mapMode, MapFlags.None);
                SharpDX.Utilities.CopyMemory(box.DataPointer, data.Pointer, data.Size);
                renderContext.Context.UnmapSubresource(dxBuffer.Buffer, 0);
            }
        }

        public void Update(ITexture2D texture, int mipLevel, DataRectangle data)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (dxTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");
            if (mipLevel < 0 || mipLevel >= texture.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");
            
            renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, data.Pitch, 0), dxTexture.Texture, Resource.CalculateSubResourceIndex(mipLevel, 0, texture.MipLevels));
        }
        
        public void Update(ITexture2DArray textureArray, int arrayIndex, int mipLevel, DataRectangle data)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(textureArray, "texture");
            if (dxTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");

            if (arrayIndex < 0 || arrayIndex >= textureArray.ArraySize)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (mipLevel < 0 || mipLevel >= textureArray.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, data.Pitch, 0), dxTexture.Texture, Resource.CalculateSubResourceIndex(mipLevel, arrayIndex, textureArray.MipLevels));
        }
                
        public void Update(ITexture3D texture, int mipLevel, DataBox data)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (dxTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");
            if (data == null)
                throw new ArgumentNullException("data");
            if (mipLevel < 0 || mipLevel >= texture.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            renderContext.Context.UpdateSubresource(new SharpDX.DataBox(data.Pointer, data.Pitch, data.Slice), dxTexture.Texture, Resource.CalculateSubResourceIndex(mipLevel, 0, texture.MipLevels));
        }


        public void GenerateMips(ITexture2D texture)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (!dxTexture.Texture.Description.OptionFlags.HasFlag(SharpDX.Direct3D11.ResourceOptionFlags.GenerateMipMaps))
                throw new ArgumentException("Texture does not have the GenerateMipMaps flag.", "texture");

            renderContext.Context.GenerateMips(dxTexture.ResourceView);
        }

        public void GenerateMips(ITexture2DArray textureArray)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(textureArray, "textureArray");
            if (!dxTexture.Texture.Description.OptionFlags.HasFlag(SharpDX.Direct3D11.ResourceOptionFlags.GenerateMipMaps))
                throw new ArgumentException("TextureArray does not have the GenerateMipMaps flag.", "textureArray");

            renderContext.Context.GenerateMips(dxTexture.ResourceView);
        }

        public void GenerateMips(ITexture3D texture)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool isDisposing)
        {
        }

    }
}
