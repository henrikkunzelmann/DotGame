using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    class Texture3D : GraphicsObject, ITexture3D, IRenderTarget3D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Length { get; private set; }

        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }

        internal int TextureID { get; private set; }

        public Texture3D(GraphicsDevice graphicsDevice, int width, int height,int length, bool generateMipMaps, TextureFormat format)
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
            if (length > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("length exceeds the maximum texture size");
            
            this.Width = width;
            this.Height = height;
            this.Length = length;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;

            this.TextureID = GL.GenTexture();

            graphicsDevice.BindManager.SetTexture(this, 0);

            var tuple = EnumConverter.Convert(Format);
            GL.TexImage3D(TextureTarget.Texture3D, 0, tuple.Item1, this.Width, this.Height, this.Length, 0, tuple.Item2, tuple.Item3, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

            graphicsDevice.CheckGLError();
        }

        public Texture3D(GraphicsDevice graphicsDevice, int width, int height, int length, int mipLevels, bool generateMipMaps, TextureFormat format, IntPtr data)
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
            if (length > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("length exceeds the maximum texture size");

            this.Width = width;
            this.Height = height;
            this.Length = length;
            this.MipLevels = mipLevels == 0 ? OpenGL4.GraphicsDevice.MipLevels(width, height) : mipLevels;
            this.Format = format;

            this.TextureID = GL.GenTexture();

            graphicsDevice.BindManager.SetTexture(this, 0);
            /*
            // TODO (Robin): Texturen mit Inhalt über ResourceManager laden
            //Tuple<OpenTK.Graphics.OpenGL4.PixelFormat, PixelType> tuple = EnumConverter.ConvertPixelDataFormat(Format);
            //GL.TexImage3D(TextureTarget.Texture3D, 0, EnumConverter.Convert(Format), this.Width, this.Height, length, 0, tuple.Item1, tuple.Item2, IntPtr.Zero);
            GL.TexImage3D(TextureTarget.Texture3D, 0, EnumConverter.Convert(Format), this.Width, this.Height, this.Length, 0, PixelFormat.Bgr, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

            if (generateMipMaps)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
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
