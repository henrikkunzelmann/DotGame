using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Runtime.InteropServices;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;

namespace DotGame.OpenGL4
{
    class Texture3D : GraphicsObject, ITexture3D, IRenderTarget3D
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Length { get; private set; }

        public int MipLevels { get; private set; }
        public TextureFormat Format { get; private set; }
        public ResourceUsage Usage { get; private set; }

        //Ob man TexSubImageXD verwenden kann
        private bool isInitialized = false;

        internal int TextureID { get; private set; }internal Texture3D(GraphicsDevice graphicsDevice, int width, int height, int length, TextureFormat format, bool generateMipMaps,  ResourceUsage usage = ResourceUsage.Normal, DataBox data = new DataBox())
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
            if ((width % 2 != 0 || height % 2 != 0 ||length % 2 != 0) && graphicsDevice.OpenGLCapabilities.SupportsNonPowerOf2Textures)
                throw new PlatformNotSupportedException("Driver doesn't support non power of two textures");
            if (length > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("length exceeds the maximum texture size");
            if (usage == ResourceUsage.Immutable && (data.IsNull))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");

            this.Width = width;
            this.Height = height;
            this.Length = length;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                //Höchstes Mipmap-Level setzen
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                //Die Textur erstellen (Null Daten funktionieren nicht mit Compression)
                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                    GL.TexStorage3D(TextureTarget3d.Texture3D, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, Length);
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (!data.IsNull)
                    {
                        size = data.Size;
                        ptr = data.Pointer;
                    }
                    if (!TextureFormatHelper.IsCompressed(Format))
                        GL.TexImage3D(TextureTarget.Texture3D, 0, internalFormat.Item1, width, height, length, 0, internalFormat.Item2, internalFormat.Item3, ptr);
                    else if (ptr != IntPtr.Zero)
                    {
                        GL.CompressedTexImage3D(TextureTarget.Texture3D, 0, internalFormat.Item1, width, height, length, 0, size, ptr);
                        isInitialized = true;
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                //Höchstes Mipmap-Level setzen
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                //Die Textur erstellen (Null Daten funktionieren nicht mit Compression)

                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format))
                    Ext.TextureStorage3D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget3d.Texture3D, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, Length);
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (!data.IsNull)
                    {
                        size = data.Size;
                        ptr = data.Pointer;
                    }
                    if (!TextureFormatHelper.IsCompressed(Format))
                        Ext.TextureImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture3D, 0, (int)internalFormat.Item1, width, height, length, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, ptr);
                    else if (ptr != IntPtr.Zero)
                    {
                        Ext.CompressedTextureImage3D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)internalFormat.Item1, width, height, length, 0, size, ptr);
                        isInitialized = true;
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError("Texture3D Constructor");            
        }

        internal Texture3D(GraphicsDevice graphicsDevice, int width, int height, int length, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataBox[] data)
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
            if ((width % 2 != 0 || height % 2 != 0 || length % 2 != 0) && graphicsDevice.OpenGLCapabilities.SupportsNonPowerOf2Textures)
                throw new PlatformNotSupportedException("Driver doesn't support non power of two textures");
            if (length > graphicsDevice.OpenGLCapabilities.MaxTextureSize)
                throw new PlatformNotSupportedException("length exceeds the maximum texture size");
            if (usage == ResourceUsage.Immutable && (data == null || data.Length == 0))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");
            if (data != null && data.Length != 0 && data.Length < mipLevels)
                throw new ArgumentOutOfRangeException("data", data.Length, string.Format("data Lenght is too small for specified mipLevels, expected: {0}", mipLevels));

            this.Width = width;
            this.Height = height;
            this.Length = length;
            this.MipLevels = mipLevels > 0 ? mipLevels : 1;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                //Höchstes Mipmap-Level setzen
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                //Die Textur erstellen (Null Daten funktionieren nicht mit Compression)
                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2))
                    GL.TexStorage3D(TextureTarget3d.Texture3D, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, Length);
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (data != null && data.Length > 0)
                    {
                        size = data[0].Size;
                        ptr = data[0].Pointer;
                    }
                    if (!TextureFormatHelper.IsCompressed(Format))
                        GL.TexImage3D(TextureTarget.Texture3D, 0, internalFormat.Item1, width, height, length, 0, internalFormat.Item2, internalFormat.Item3, ptr);
                    else if (ptr != IntPtr.Zero)
                    {
                        GL.CompressedTexImage3D(TextureTarget.Texture3D, 0, internalFormat.Item1, width, height, length, 0, size, ptr);
                        isInitialized = true;
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                //Höchstes Mipmap-Level setzen
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                //Die Textur erstellen (Null Daten funktionieren nicht mit Compression)

                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format))
                    Ext.TextureStorage3D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget3d.Texture3D, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height, Length);
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (data != null && data.Length > 0)
                    {
                        size = data[0].Size;
                        ptr = data[0].Pointer;
                    }
                    if (!TextureFormatHelper.IsCompressed(Format))
                        Ext.TextureImage3D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture3D, 0, (int)internalFormat.Item1, width, height, length, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, ptr);
                    else if (ptr != IntPtr.Zero)
                    {
                        Ext.CompressedTextureImage3D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, 0, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)internalFormat.Item1, width, height, length, 0, size, ptr);
                        isInitialized = true;
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError("Texture3D Constructor");

            if (data != null && data.Length > 1)
                for (int i = 1; i < data.Length; i++)
                    SetData(data[i], i);
        }

        internal void SetData(DataBox data, int mipLevel)
        {
            var format = EnumConverter.Convert(Format);

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);

                if (!TextureFormatHelper.IsCompressed(Format))
                    GL.TexSubImage3D(TextureTarget.Texture3D, mipLevel, 0, 0, 0, Width, Height, Length, format.Item2, format.Item3, data.Pointer);
                else
                {
                    if (isInitialized)
                        GL.CompressedTexSubImage3D(TextureTarget.Texture3D, mipLevel,0,0,0, Width, Height, Length, format.Item2, data.Size, data.Pointer);
                    else
                        GL.CompressedTexImage3D(TextureTarget.Texture3D, mipLevel, format.Item1, Width, Height, Length, 0, data.Size, data.Pointer);
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                if (!TextureFormatHelper.IsCompressed(Format))
                    Ext.TextureSubImage3D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, mipLevel, 0, 0, 0, Width, Height, Length, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data.Pointer);
                else
                {
                    if(isInitialized)
                        Ext.CompressedTextureSubImage3D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, mipLevel,0,0,0, Width, Height, Length, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, Marshal.SizeOf(data), data.Pointer);
                    else
                        Ext.CompressedTextureImage3D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D, mipLevel, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)format.Item1, Width, Height, Length, 0, Marshal.SizeOf(data), data.Pointer);
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            { 
                //OpenGL 4.5
            }

            graphicsDevice.CheckGLError("Texture3D GenerateMipMaps");
        }



        internal void GenerateMipMaps()
        {
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.GenerateTextureMipmap(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture3D);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError("Texture3D GenerateMipMaps");
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
