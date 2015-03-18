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



        public static Dictionary<TextureFormat, PixelInternalFormat> TextureFormats = new Dictionary<TextureFormat, PixelInternalFormat>() 
        {
            {TextureFormat.RGB32_Float, PixelInternalFormat.Rgb32f},
            {TextureFormat.DXT1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext},
            {TextureFormat.DXT3, PixelInternalFormat.CompressedRgbaS3tcDxt3Ext},
            {TextureFormat.DXT5, PixelInternalFormat.CompressedRgbaS3tcDxt5Ext},
            {TextureFormat.RGBA16_UIntNorm, PixelInternalFormat.Rgba16},
            {TextureFormat.RGBA8_UIntNorm, PixelInternalFormat.Rgba8},
            {TextureFormat.Depth16, PixelInternalFormat.DepthComponent16},
            {TextureFormat.Depth32, PixelInternalFormat.DepthComponent32},
            {TextureFormat.Depth24Stencil8, PixelInternalFormat.Depth24Stencil8},
        };
    }
}
