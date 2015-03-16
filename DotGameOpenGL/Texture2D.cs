using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.Graphics.OpenGL4
{
    internal class Texture2D : ITexture2D
    {
        int width;
        int Width { get { return width; } }

        int height;
        public int Height { get { return height; } }

        internal int textureId;

        /// <summary>
        /// Creates an empty texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        public Texture2D(int width, int height, PixelInternalFormat format)
        {
            this.width = width;
            this.height = height;

            textureId = GL.GenTexture();
            
            //To be replaced by graphics device call
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.TexImage2D(TextureTarget.Texture2D, 0, format, width, height, 0, PixelFormat.Alpha, PixelType.Byte, IntPtr.Zero);
        }
    }
}
