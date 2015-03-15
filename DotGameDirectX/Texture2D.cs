using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace DotGame.Graphics.Direct3D11
{
    internal class Texture2D : ITexture2D
    {
        int width;
        int Width { get { return width; } }

        int height;
        public int Height { get { return height; } }

        internal Texture2D texture;

        /// <summary>
        /// Creates an empty texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        public Texture2D(int width, int height, Format format)
        {
            this.width = width;
            this.height = height;

            texture = new Texture2D(width, height, format);
        }
    }
}
