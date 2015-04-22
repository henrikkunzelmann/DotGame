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
    class Texture2DArray : GraphicsObject, ITexture2DArray, IRenderTarget2DArray
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int ArraySize { get; private set; }
        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }

        internal int TextureID { get; private set; }

        internal TextureTarget TextureTarget { get; private set; }
        
        internal Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, bool isCubeMap, bool generateMipMaps, TextureFormat format)
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
        }

        internal Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, bool isCubeMap, int mipLevels, bool generateMipMaps, TextureFormat format)
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

            graphicsDevice.CheckGLError();
        }

        internal void SetData<T>(T[] data, int mipLevel)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");

            GCHandle arrayHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = arrayHandle.AddrOfPinnedObject();
                SetData(ptr, mipLevel, data.Length * Marshal.SizeOf(typeof(T)));
            }
            finally
            {
                arrayHandle.Free();
            }
        }

        internal void SetData(IntPtr data, int mipLevel, int imageSize)
        {
            var format = EnumConverter.Convert(Format);

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                
                if (!TextureFormatHelper.IsCompressed(Format))
                    GL.TexImage2D(TextureTarget, mipLevel, format.Item1, Width, Height, 0, format.Item2, format.Item3, data);
                else
                    GL.CompressedTexImage2D(TextureTarget, mipLevel, format.Item1, Width, Height, 0, imageSize, data);

                GL.TexParameter(TextureTarget, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                if (!TextureFormatHelper.IsCompressed(Format))
                    Ext.TextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget, mipLevel, (int)format.Item1, Width, Height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data);
                else
                    Ext.CompressedTextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget, mipLevel, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Format).Item1, Width, Height, 0, Marshal.SizeOf(data), data);
                
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            { 
                //OpenGL 4.5
            }

            graphicsDevice.CheckGLError();
        }

        internal void GenerateMipMaps()
        {
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.GenerateTextureMipmap(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2DArray);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
