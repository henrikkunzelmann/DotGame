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

            graphicsDevice.StateManager.SetTexture(this, 0);
            GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverter.Convert(Format), this.Width, this.Height, 0, PixelFormat.Alpha, PixelType.Byte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, int mipLevels, TextureFormat format, IntPtr data)
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

            graphicsDevice.StateManager.SetTexture(this, 0);

            // TODO (Robin): Texturen mit Inhalt über ResourceManager laden
            //Tuple<OpenTK.Graphics.OpenGL4.PixelFormat, PixelType> tuple = EnumConverter.ConvertPixelDataFormat(Format);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverter.Convert(Format), Width, Height, 0, tuple.Item1, tuple.Item2, data);
            GL.TexImage2D(TextureTarget.Texture2D, 0, EnumConverter.Convert(Format), Width, Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
