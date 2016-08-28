using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
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
        public ResourceUsage Usage { get; private set; }

        internal int TextureID { get; private set; }
        
        internal Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, TextureFormat format, bool generateMipMaps,  ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
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
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("Array Size must be at least one", "arraySize");
            if (usage == ResourceUsage.Immutable && (data == null || data.Length == 0))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");
            if (data != null && data.Length != 0 && data.Length < arraySize)
                throw new ArgumentOutOfRangeException("data", data.Length, string.Format("data Lenght is too small for specified arraySize, expected: {0}", arraySize));

            this.Width = width;
            this.Height = height;
            this.ArraySize = arraySize;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (!TextureFormatHelper.IsCompressed(Format))
                    if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                        GL.TexStorage3D(TextureTarget3d.Texture2DArray, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, ArraySize);
                    else
                        GL.TexImage3D(TextureTarget.Texture2DArray, 0, internalFormat.Item1, width, height, arraySize, 0, internalFormat.Item2, internalFormat.Item3, IntPtr.Zero);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (!TextureFormatHelper.IsCompressed(Format))
                    if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                        Ext.TextureStorage3D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget3d.Texture2DArray, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, ArraySize);
                    else
                        Ext.TextureImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture3D, 0, (int)internalFormat.Item1, width, height, arraySize, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, IntPtr.Zero);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    SetData(data[i], i, 0);
                }
            }

            graphicsDevice.CheckGLError();
        }

        internal Texture2DArray(GraphicsDevice graphicsDevice, int width, int height, int arraySize, TextureFormat format, int mipLevels,  ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
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
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("Array Size must be at least one", "arraySize");
            if (data != null && data.Length != 0 && data.Length < mipLevels * arraySize)
                throw new ArgumentOutOfRangeException("data", data.Length, string.Format("data Lenght is too small for specified arraySize and mipLevels, expected: {0}", mipLevels * arraySize));

            this.Width = width;
            this.Height = height;
            this.ArraySize = arraySize;
            this.MipLevels = mipLevels > 0 ? mipLevels : 1;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (!TextureFormatHelper.IsCompressed(Format))
                    if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                        GL.TexStorage3D(TextureTarget3d.Texture2DArray, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, ArraySize);
                    else
                        GL.TexImage3D(TextureTarget.Texture2DArray, 0, internalFormat.Item1, width, height, arraySize, 0, internalFormat.Item2, internalFormat.Item3, IntPtr.Zero);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (!TextureFormatHelper.IsCompressed(Format))
                    if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                        Ext.TextureStorage3D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget3d.Texture2DArray, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, ArraySize);
                    else
                        Ext.TextureImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture3D, 0, (int)internalFormat.Item1, width, height, arraySize, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, IntPtr.Zero);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    int arrayIndex = i / MipLevels;
                    int mipIndex = i - arrayIndex * MipLevels; //Macht Sinn weil int gerundet wird
                    SetData(data[i], arrayIndex, 0);
                }
            }

            graphicsDevice.CheckGLError();
        }

        internal void SetData(DataRectangle data, int arrayIndex, int mipLevel)
        {
            var format = EnumConverter.Convert(Format);
            
                if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
                {
                    graphicsDevice.BindManager.SetTexture(this, 0);

                    if (!TextureFormatHelper.IsCompressed(Format))
                        GL.TexSubImage3D(TextureTarget.Texture2DArray, mipLevel, 0,0,0, Width, Height, ArraySize, format.Item2, format.Item3, data.Pointer);
                    else
                        GL.CompressedTexImage3D(TextureTarget.Texture2DArray, mipLevel, format.Item1, Width, Height, ArraySize, 0, data.Size, data.Pointer);

                    GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                }
                else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                {
                    if (!TextureFormatHelper.IsCompressed(Format))
                        Ext.TextureSubImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2DArray, mipLevel, 0,0,0, Width, Height, ArraySize, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data.Pointer);
                    else
                        Ext.CompressedTextureImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2DArray, mipLevel, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Format).Item1, Width, Height, ArraySize, 0, Marshal.SizeOf(data), data.Pointer);

                    OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2DArray, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                }
                else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
                {
                    //OpenGL 4.5
                }

            graphicsDevice.CheckGLError("Texture2DArray SetData");
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

            graphicsDevice.CheckGLError("Texture2DArray GenerateMipMaps");
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
