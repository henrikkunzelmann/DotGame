using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    class Texture2DArray : GraphicsObject, ITexture2DArray, IRenderTarget2D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int ArraySize { get; private set; }
        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }

        internal int TextureID { get; private set; }

        internal TextureTarget TextureTarget { get; private set; }

        public Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, bool isCubeMap, bool generateMipMaps, TextureFormat format)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("Format must be not TextureFormat.Unkown.", "format");
            if (width > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("width exceeds the maximum texture size");
            if (height > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("height exceeds the maximum texture size");
            if (width != height)
                throw new PlatformNotSupportedException("Texture arrays must be quadratic");
            if (width != height)
                throw new PlatformNotSupportedException("Texture arrays must be quadratic");
            if (arraySize != 0)
                throw new ArgumentOutOfRangeException("Array Size must be at least one", "arraySize");

            this.Width = width;
            this.Height = height;
            this.ArraySize = arraySize;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;
            this.TextureTarget = isCubeMap ? TextureTarget.Texture2DArray : OpenTK.Graphics.OpenGL4.TextureTarget.TextureCubeMap;

            this.TextureID = GL.GenTexture();

            graphicsDevice.BindManager.SetTexture(this, 0);
            /*
            Tuple<OpenTK.Graphics.OpenGL4.PixelFormat, PixelType> tuple = EnumConverter.ConvertPixelDataFormat(Format);
            GL.TexImage2D(TextureTarget.Texture2DArray, 0, EnumConverter.Convert(Format), this.Width, this.Height, 0, tuple.Item1, tuple.Item2, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            */
            graphicsDevice.CheckGLError();
        }

        public Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, bool isCubeMap, int mipLevels, bool generateMipMaps, TextureFormat format, IntPtr data)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (mipLevels < 0)
                throw new ArgumentOutOfRangeException("mipLevels", "MipLevels must be not negative.");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("Format must be not TextureFormat.Unkown.", "format");
            if (width > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("width exceeds the maximum texture size");
            if (height > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("height exceeds the maximum texture size");
            if (width != height)
                throw new PlatformNotSupportedException("Texture arrays must be quadratic");
            if (arraySize != 0)
                throw new ArgumentOutOfRangeException("Array Size must be at least one", "arraySize");

            this.Width = width;
            this.Height = height;
            this.ArraySize = arraySize;
            this.MipLevels = mipLevels == 0 ? OpenGL4.GraphicsDevice.MipLevels(width, height) : mipLevels;
            this.Format = format;
            this.TextureTarget = isCubeMap ? TextureTarget.Texture2DArray : OpenTK.Graphics.OpenGL4.TextureTarget.TextureCubeMap;

            this.TextureID = GL.GenTexture();

            graphicsDevice.BindManager.SetTexture(this, 0);
            /*
            // TODO (Robin): Texturen mit Inhalt über ResourceManager laden
            //Tuple<OpenTK.Graphics.OpenGL4.PixelFormat, PixelType> tuple = EnumConverter.ConvertPixelDataFormat(Format);
            //GL.TexImage2D(TextureTarget.Texture2DArray, 0, EnumConverter.Convert(Format), Width, Height, 0, tuple.Item1, tuple.Item2, data);
            GL.TexImage2D(TextureTarget.Texture2DArray, 0, EnumConverter.Convert(Format), Width, Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, data);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

            if (generateMipMaps)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            */
            graphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
