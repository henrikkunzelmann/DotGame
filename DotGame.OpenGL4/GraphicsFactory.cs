using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : GraphicsObject, IGraphicsFactory
    {
        internal GraphicsFactory(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            assertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            assertCurrent();
            throw new NotImplementedException();
        }
    }
}
