using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    class Texture2D : GraphicsObject, ITexture2D
    {
        readonly int textureId;
        readonly int width;
        readonly int height;
        int mipLevels;
        readonly TextureFormat format;

        public Texture2D(GraphicsDevice device, int width, int height, TextureFormat format)
            : base(device)
        {
            this.format = format;
            this.width = width;
            this.height = height;
            mipLevels = 1;

            textureId = GL.GenTexture();

            //TODO Replace by GraphicsDevice Method
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, GraphicsFactory.TextureFormats[format], width, height, 0, PixelFormat.Alpha, PixelType.Byte, IntPtr.Zero);  

        }

        public TextureFormat Format { get { return format; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int MipLevels { get { return mipLevels; } }

        internal int TextureId { get { return textureId; } }
    }
}
