using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.DirectX11
{
    public class GraphicsFactory : IGraphicsFactory
    {
        public IGraphicsDevice GraphicsDevice { get; private set; }
        public bool IsDisposed { get; private set; }

        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal GraphicsFactory(IGraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("graphicsDevice is disposed.", "graphicsDevice");
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format)
        {
            throw new NotImplementedException();
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format)
        {
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            throw new NotImplementedException();
        }

        

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
