using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenGL4
{
    internal class RenderUpdateContext : GraphicsObject, IRenderUpdateContext
    {
        private RenderContext renderContext;
        public IRenderContext RenderContext
        {
            get
            {
                return renderContext;
            }
        }

        public RenderUpdateContext(GraphicsDevice device, RenderContext context) : base( device, new System.Diagnostics.StackTrace())
        {
            if (device == null)
                throw new ArgumentNullException("device");

            if (context == null)
                throw new ArgumentNullException("context");
            
            this.renderContext = context;
        }

        // TODO (henrik1235) Data* Update einbauen


        public void Update(IConstantBuffer constantBuffer, DataArray data)
        {
            var internalBuffer = graphicsDevice.Cast<ConstantBuffer>(constantBuffer, "constantBuffer");
            if (internalBuffer.IsDisposed)
                throw new ObjectDisposedException("constantBuffer");
            if (internalBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "constantBuffer");

            internalBuffer.UpdateData(data);
        }

        
        public void Update(IVertexBuffer vertexBuffer, DataArray data)
        {
            var internalBuffer = graphicsDevice.Cast<VertexBuffer>(vertexBuffer, "vertexBuffer");
            if (internalBuffer.IsDisposed)
                throw new ObjectDisposedException("vertexBuffer");
            if (internalBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "vertexBuffer");


            internalBuffer.UpdateData(data);
        }
        
        public void Update(IIndexBuffer indexBuffer, DataArray data, int count)
        {
            var internalBuffer = graphicsDevice.Cast<IndexBuffer>(indexBuffer, "indexBuffer");
            if (internalBuffer.IsDisposed)
                throw new ObjectDisposedException("indexBuffer");
            if (internalBuffer.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "indexBuffer");


            internalBuffer.UpdateData(data);
        }
        
        
        public void Update(ITexture2D texture, int mipLevel, DataRectangle data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (internalTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");

            internalTexture.SetData(data, mipLevel);
        }
        
                
        public void Update(ITexture2DArray textureArray, int arrayIndex, int mipLevel, DataRectangle data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            Texture2DArray internalTexture = graphicsDevice.Cast<Texture2DArray>(textureArray, "textureArray");
            if (internalTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");

            internalTexture.SetData(data, arrayIndex, mipLevel);
        }  

        public void Update(ITexture3D texture, int mipLevel, DataBox data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            Texture3D internalTexture = graphicsDevice.Cast<Texture3D>(texture, "textureArray");
            if (internalTexture.Usage == ResourceUsage.Immutable)
                throw new ArgumentException("Can't update immutable resource.", "texture");

            internalTexture.SetData(data, mipLevel);
        }


        public void GenerateMips(ITexture2D texture)
        {
            var internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (internalTexture.IsDisposed)
                throw new ObjectDisposedException("texture");

            internalTexture.GenerateMipMaps();
        }

        public void GenerateMips(ITexture2DArray textureArray)
        {
            var internalTexture = graphicsDevice.Cast<Texture2DArray>(textureArray, "texture");
            if (internalTexture.IsDisposed)
                throw new ObjectDisposedException("textureArray");

            internalTexture.GenerateMipMaps();
        }
        public void GenerateMips(ITexture3D texture)
        {
            var internalTexture = graphicsDevice.Cast<Texture3D>(texture, "texture");
            if (internalTexture.IsDisposed)
                throw new ObjectDisposedException("texture");

            internalTexture.GenerateMipMaps();
        }

        protected override void Dispose(bool isDisposing)
        {
        }

    }
}
