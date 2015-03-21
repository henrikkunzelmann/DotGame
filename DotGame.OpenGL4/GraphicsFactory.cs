using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : GraphicsObject, IGraphicsFactory
    {
        internal GraphicsFactory(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format)
        {
            AssertCurrent();

            if (format == TextureFormat.Unknown)
                throw new ArgumentException("format must not be TextureFormat.Unknown", "format");
            if (width <= 0)
                throw new ArgumentException("width must not be zero or negative", "width");
            if (height <= 0)
                throw new ArgumentException("weight must not be zero or negative", "height");

            return new Texture2D((GraphicsDevice)GraphicsDevice, width, height, format);
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }
    }
}
