using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;

namespace DotGame.OpenGL4
{
    internal class Texture2D : GraphicsObject, ITexture2D, IRenderTarget2D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }

        internal int TextureID { get; private set; }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, bool generateMipMaps)
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
            if ((width % 2 != 0 || height % 2 != 0) && graphicsDevice.OpenGLCapabilities.SupportsNonPowerOf2Textures)
                throw new PlatformNotSupportedException("Driver doesn't support non power of two textures");

            this.Width = width;
            this.Height = height;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }   
        }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int mipLevels)
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

            this.Width = width;
            this.Height = height;
            this.Format = format;

            this.MipLevels = mipLevels == 0 ? OpenGL4.GraphicsDevice.MipLevels(width, height) : mipLevels;

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError("Texture2D Constructor");
        }
                
        internal void SetData(IntPtr data, int mipLevel, int imageSize)
        {
            if (mipLevel >= this.MipLevels)
                throw new ArgumentOutOfRangeException("MipLevel exceeds the amount of mip levels.");
            if (mipLevel < 0)
                throw new ArgumentOutOfRangeException("MipLevel must not be smaller than zero.");
            if (imageSize <= 0)
                throw new ArgumentOutOfRangeException("ImageSize must not be smaller than zero.");

            var format = EnumConverter.Convert(Format);
            int width = Width / (int)Math.Pow(2, mipLevel);
            int height = Height / (int)Math.Pow(2, mipLevel);

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None || true)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                
                if (!TextureFormatHelper.IsCompressed(Format))
                    GL.TexImage2D(TextureTarget.Texture2D, mipLevel, format.Item1, width, height, 0, format.Item2, format.Item3, data);
                else
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, mipLevel, format.Item1, width, height, 0, imageSize, data);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                if (!TextureFormatHelper.IsCompressed(Format))
                    Ext.TextureImage2D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, mipLevel, (int)format.Item1, width, height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data);
                else
                    Ext.CompressedTextureImage2D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, mipLevel, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Format).Item1, width, height, 0, imageSize, data);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            { 
                //OpenGL 4.5
            }

            graphicsDevice.CheckGLError("Texture2D SetData");
        }

        internal void GenerateMipMaps()
        {
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.GenerateTextureMipmap(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }
            graphicsDevice.CheckGLError("Texture2D GenerateMipMaps");
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
