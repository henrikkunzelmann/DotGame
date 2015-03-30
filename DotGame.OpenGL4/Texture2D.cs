using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    class Texture2D : GraphicsObject, ITexture2D, IRenderTarget2D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }

        internal int TextureID { get; private set; }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, int mipLevels, TextureFormat format)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (mipLevels < 0)
                throw new ArgumentOutOfRangeException("mipLevels", "MipLevels must be not negative.");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("format is TextureFormat.Unkown.", "format");

            this.Width = width;
            this.Height = height;
            this.MipLevels = mipLevels == 0 ? OpenGL4.GraphicsDevice.MipLevels(width, height) : mipLevels;
            this.Format = format;

            this.TextureID = GL.GenTexture();

            //TODO (Robin) Replace by GraphicsDevice Method.
            GL.BindTexture(TextureTarget.Texture2D, TextureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, GraphicsFactory.TextureFormats[format], this.Width, this.Height, 0, PixelFormat.Alpha, PixelType.Byte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
        }

        internal override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
            {
                GL.DeleteTexture(TextureID);
            }

            base.Dispose(isDisposing);
        }
    }
}
