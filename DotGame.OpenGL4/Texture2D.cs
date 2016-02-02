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
        public ResourceUsage Usage { get; private set; }

        internal int TextureID { get; private set; }

        //Ob man TexSubImageXD verwenden kann
        private bool isInitialized = false;

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, bool generateMipMaps, bool isRenderTarget, ResourceUsage usage = ResourceUsage.Normal, DataRectangle data = new DataRectangle())
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
            if ((width % 2 != 0 || height % 2 != 0) && !graphicsDevice.OpenGLCapabilities.SupportsNonPowerOf2Textures)
                throw new PlatformNotSupportedException("Driver doesn't support non power of two textures");
            if (usage == ResourceUsage.Immutable && (data.IsNull))
                throw new ArgumentException("Immutable textures must be initialized with data.", "data");
            if (isRenderTarget && TextureFormatHelper.IsCompressed(format))
                throw new ArgumentException("Can't render to compressed texture.", "format");

            this.Width = width;
            this.Height = height;
            this.MipLevels = generateMipMaps ? OpenGL4.GraphicsDevice.MipLevels(width, height) : 1;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.TextureID = GL.GenTexture();

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format) && !isRenderTarget)
                {
                    GL.TexStorage2D(TextureTarget2d.Texture2D, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height);
                    isInitialized = true;
                }
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (!data.IsNull)
                    {
                        size = data.Size;
                        ptr = data.Pointer;
                    }
                    if (ptr != IntPtr.Zero || isRenderTarget)
                    {
                        if (!TextureFormatHelper.IsCompressed(Format))
                        {
                            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat.Item1, width, height, 0, internalFormat.Item2, internalFormat.Item3, ptr);
                            isInitialized = true;
                        }
                        else
                        {
                            GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, internalFormat.Item1, width, height, 0, size, ptr);
                            isInitialized = true;
                        }
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format) && !isRenderTarget)
                {
                    Ext.TextureStorage2D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget2d.Texture2D, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height);
                    isInitialized = true;
                }
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (!data.IsNull)
                    {
                        size = data.Size;
                        ptr = data.Pointer;
                    }

                    if (ptr != IntPtr.Zero || isRenderTarget)
                    {
                        if (!TextureFormatHelper.IsCompressed(Format))
                        {
                            Ext.TextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2D, 0, (int)internalFormat.Item1, width, height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, ptr);
                            isInitialized = true;
                        }
                        else
                        {
                            Ext.CompressedTextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2D, 0, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)internalFormat.Item1, width, height, 0, size, ptr);
                            isInitialized = true;
                        }
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError(ToString());
       }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
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
            if (usage == ResourceUsage.Immutable && (data == null || data.Length == 0))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");
            if(data != null && data.Length != 0 && data.Length < mipLevels)
                throw new ArgumentOutOfRangeException("data", data.Length, string.Format("data Lenght is too small for specified mipLevels, expected: {0}", mipLevels));

            this.Width = width;
            this.Height = height;
            this.Format = format;
            this.Usage = usage;
            var internalFormat = EnumConverter.Convert(Format);

            this.MipLevels = mipLevels > 0 ? mipLevels : 1;

            this.TextureID = GL.GenTexture();
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, this.MipLevels - 1);

                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format))
                {
                    GL.TexStorage2D(TextureTarget2d.Texture2D, MipLevels, EnumConverter.ConvertSizedInternalFormat(Format), Width, Height);
                    isInitialized = true;
                }
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (data != null && data.Length > 0)
                    {
                        size = data[0].Size;
                        ptr = data[0].Pointer;
                    }
                    if (ptr != IntPtr.Zero)
                    {
                        if (!TextureFormatHelper.IsCompressed(Format))
                        {
                            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat.Item1, width, height, 0, internalFormat.Item2, internalFormat.Item3, ptr);
                            isInitialized = true;
                        }
                        else
                        {
                            GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, internalFormat.Item1, width, height, 0, size, ptr);
                            isInitialized = true;
                        }
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.TextureParameter(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, this.MipLevels - 1);
                if (graphicsDevice.OpenGLCapabilities.SupportsTextureStorage && graphicsDevice.OpenGLCapabilities.OpenGLVersion > new Version(4, 2) && !TextureFormatHelper.IsCompressed(Format))
                {
                    Ext.TextureStorage2D(TextureID, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)TextureTarget2d.Texture2D, MipLevels, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.ConvertSizedInternalFormat(Format), Width, Height);
                    isInitialized = true;
                }
                else
                {
                    int size = 0;
                    IntPtr ptr = IntPtr.Zero;
                    if (data != null && data.Length > 0)
                    {
                        size = data[0].Size;
                        ptr = data[0].Pointer;
                    }
                    if (ptr != IntPtr.Zero)
                    {
                        if (!TextureFormatHelper.IsCompressed(Format))
                        {
                            Ext.TextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2D, 0, (int)internalFormat.Item1, width, height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)internalFormat.Item2, (OpenTK.Graphics.OpenGL.PixelType)internalFormat.Item3, ptr);
                            isInitialized = true;
                        }
                        else
                        {
                            Ext.CompressedTextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2D, 0, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)internalFormat.Item1, width, height, 0, size, ptr);
                            isInitialized = true;
                        }
                    }
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            {
            }

            graphicsDevice.CheckGLError(ToString());

            if (data != null && data.Length > 1)
                for (int i = 1; i < data.Length; i++)
                    SetData(data[i], i);
        }
                
        internal void SetData(DataRectangle data, int mipLevel)
        {
            if (mipLevel >= this.MipLevels)
                throw new ArgumentOutOfRangeException("MipLevel exceeds the amount of mip levels.");
            if (mipLevel < 0)
                throw new ArgumentOutOfRangeException("MipLevel must not be smaller than zero.");
            if (data.Size < 0)
                throw new ArgumentOutOfRangeException("ImageSize must not be smaller than zero.");

            var format = EnumConverter.Convert(Format);
            int width = Width / (int)Math.Pow(2, mipLevel);
            int height = Height / (int)Math.Pow(2, mipLevel);

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.SetTexture(this, 0);

                if (!TextureFormatHelper.IsCompressed(Format))
                {
                    if (isInitialized)
                        GL.TexSubImage2D(TextureTarget.Texture2D, mipLevel, 0, 0, width, height, format.Item2, format.Item3, data.Pointer);
                    else
                    {
                        GL.TexImage2D(TextureTarget.Texture2D, mipLevel, format.Item1, width, height, 0, format.Item2, format.Item3, data.Pointer);
                        isInitialized = true;
                    }
                }
                else
                {
                    /*                    
                    The required paletted formats do not allow subimage updates, but other formats defined by extensions may. -> Betrifft DXT Formate
                    if (isInitialized)
                        GL.CompressedTexSubImage2D(TextureTarget.Texture2D, mipLevel, 0, 0, width, height, format.Item2, data.Size, data.Pointer);
                    */
                    GL.CompressedTexImage2D(TextureTarget.Texture2D, mipLevel, format.Item1, width, height, 0, data.Size, data.Pointer);
                    isInitialized = true;
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                if (!TextureFormatHelper.IsCompressed(Format))
                {
                    if (isInitialized)
                        Ext.TextureSubImage2D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, mipLevel, 0, 0, width, height, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data.Pointer);
                    else
                    {
                        Ext.TextureImage2D(TextureID, (OpenTK.Graphics.OpenGL.TextureTarget)TextureTarget.Texture2D, mipLevel, (int)format.Item1, width, height, 0, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item2, (OpenTK.Graphics.OpenGL.PixelType)format.Item3, data.Pointer);
                        isInitialized = true;
                    }
                }
                else
                {
                    /*
                    The required paletted formats do not allow subimage updates, but other formats defined by extensions may. -> Betrifft DXT Formate
                    if (isInitialized)
                        Ext.CompressedTextureSubImage2D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, mipLevel, 0, 0, width, height, (OpenTK.Graphics.OpenGL.PixelFormat)format.Item1, data.Size, data.Pointer);
                    */


                    Ext.CompressedTextureImage2D(TextureID, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, mipLevel, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)format.Item1, width, height, 0, data.Size, data.Pointer);
                    isInitialized = true;                    
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Core)
            { 
                //OpenGL 4.5
            }

            graphicsDevice.CheckGLError(ToString());
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
            graphicsDevice.CheckGLError(ToString());
        }

        public override string ToString()
        {
            return string.Format("Texture2D Format: {0}; Width: {1}; Height: {2}; MipLevels: {3}; ResourceUsage: {4}; Internal OpenGL ID: {5}", Format, Width, Height, MipLevels, Usage.ToString(), TextureID);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(TextureID);
        }
    }
}
