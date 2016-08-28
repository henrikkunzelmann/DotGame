using DotGame.Graphics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using ResourceUsage = DotGame.Graphics.ResourceUsage;

namespace DotGame.DirectX11
{
    public class Texture2D : GraphicsObject, ITexture2D, ITexture2DArray, IRenderTarget2D, IRenderTarget2DArray, ITextureCubemap
    {
        public int Width { get { return Texture.Description.Width; } }
        public int Height { get { return Texture.Description.Height; } }
        public int MipLevels { get { return Texture.Description.MipLevels; } }
        public TextureFormat Format { get { return EnumConverter.ConvertToTexture(Texture.Description.Format); } }
        public int ArraySize { get { return Texture.Description.ArraySize; } }
        public Graphics.ResourceUsage Usage { get { return EnumConverter.Convert(Texture.Description.Usage); } }

        internal SharpDX.Direct3D11.Texture2D Texture { get; private set; }
        internal ShaderResourceView ResourceView { get; private set; }
        internal RenderTargetView RenderView { get; private set; }
        internal DepthStencilView DepthView { get; private set; }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int arraySize, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (mipLevels < 0)
                throw new ArgumentOutOfRangeException("mipLevels", "MipLevels must be not negative.");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("Format must be not TextureFormat.Unkown.", "format");
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("arraySize", "ArraySize must be positive.");
            if (usage == ResourceUsage.Immutable && (data == null || data.Length == 0))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");
            if (data != null && data.Length != 0 && data.Length < mipLevels * arraySize)
                throw new ArgumentOutOfRangeException("data", data.Length, string.Format("data Lenght is too small for specified arraySize and mipLevels, expected: {0}", mipLevels * arraySize));
            
            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                Format = EnumConverter.Convert(format),
                ArraySize = arraySize,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                MipLevels = mipLevels > 0 ? mipLevels : 1,
                Usage = EnumConverter.Convert(usage)
            };
            
            desc.BindFlags |= BindFlags.ShaderResource;
            
            if (data != null && data.Length > 1)
            {
                SharpDX.DataRectangle[] rects = new SharpDX.DataRectangle[data.Length];
                for (int i = 0; i < data.Length; i++)
                    rects[i] = new SharpDX.DataRectangle(data[i].Pointer, data[i].Pitch);

                this.Texture = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc, rects);
            }
            else
                this.Texture = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc);

            CreateViews();
        }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int arraySize, bool isRenderTarget, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("Format must be not TextureFormat.Unkown.", "format");
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("arraySize", "ArraySize must be positive.");
            if (usage == ResourceUsage.Immutable && (data == null || data.Length == 0))
                throw new ArgumentException("data", "Immutable textures must be initialized with data.");

            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                Format = EnumConverter.Convert(format),
                ArraySize = arraySize,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                MipLevels = !generateMipMaps ? 1 : 0,
                Usage = EnumConverter.Convert(usage)
            };

            if (isRenderTarget)
                if (format.IsDepth())
                    desc.BindFlags |= BindFlags.DepthStencil;
                else
                    desc.BindFlags |= BindFlags.RenderTarget;
            if (generateMipMaps)
            {
                desc.BindFlags |= BindFlags.RenderTarget;
                desc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
            }
            desc.BindFlags |= BindFlags.ShaderResource;

            if (data != null && data.Length > 1)
            {
                SharpDX.DataRectangle[] rects = new SharpDX.DataRectangle[data.Length];
                for (int i = 0; i < data.Length; i++)
                    rects[i] = new SharpDX.DataRectangle(data[i].Pointer, data[i].Pitch);

                this.Texture = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc, rects);
            }
            else
                this.Texture = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc);

            CreateViews();
        }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int mipLevels, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
            : this(graphicsDevice, width, height, format, 1, mipLevels, usage, data)
        { }

        public Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, bool isRenderTarget, bool generateMipMaps, ResourceUsage usage = ResourceUsage.Normal, params DataRectangle[] data)
            : this(graphicsDevice, width, height, format, 1, isRenderTarget, generateMipMaps, usage, data)
        { }

        internal Texture2D(GraphicsDevice graphicsDevice, SharpDX.Direct3D11.Texture2D handle)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (handle == null)
                throw new ArgumentNullException("handle");
            if (handle.IsDisposed)
                throw new ArgumentException("Handle is disposed.", "handle");

            this.Texture = handle;

            CreateViews();
        }

        private void CreateViews()
        {
            if (Texture.Description.BindFlags.HasFlag(BindFlags.DepthStencil))
                DepthView = new DepthStencilView(graphicsDevice.Device, Texture,
                    new DepthStencilViewDescription()
                    {
                        Format = EnumConverter.ConvertDepthView(Format),
                        Dimension = ArraySize > 0 ? DepthStencilViewDimension.Texture2DArray : DepthStencilViewDimension.Texture2D,
                        Texture2D = new DepthStencilViewDescription.Texture2DResource() {
                            MipSlice = 0
                        },
                        Texture2DArray = new DepthStencilViewDescription.Texture2DArrayResource()
                        {
                            ArraySize = this.ArraySize,
                            FirstArraySlice = 0,
                        }
                    });
            if (Texture.Description.BindFlags.HasFlag(BindFlags.RenderTarget))
                RenderView = new RenderTargetView(graphicsDevice.Device, Texture);
            if (Texture.Description.BindFlags.HasFlag(BindFlags.ShaderResource))
                ResourceView = new ShaderResourceView(graphicsDevice.Device, Texture,
                    new ShaderResourceViewDescription()
                    {
                        Format = Format.IsDepth() ? EnumConverter.ConvertDepthShaderView(Format) : EnumConverter.Convert(Format),
                        Dimension = ArraySize > 0 ? ShaderResourceViewDimension.Texture2DArray : ShaderResourceViewDimension.Texture2D,
                        Texture2D = new ShaderResourceViewDescription.Texture2DResource()
                        {
                            MipLevels = this.MipLevels,
                            MostDetailedMip = 0
                        },
                        Texture2DArray = new ShaderResourceViewDescription.Texture2DArrayResource()
                        {
                            ArraySize = this.ArraySize,
                            FirstArraySlice = 0,
                            MipLevels = this.MipLevels,
                            MostDetailedMip = 0
                        }
                    });
        }


        protected override void Dispose(bool isDisposing)
        {
            if (ResourceView != null && !ResourceView.IsDisposed)
                ResourceView.Dispose();
            if (RenderView != null && !RenderView.IsDisposed)
                RenderView.Dispose();
            if (Texture != null && !Texture.IsDisposed)
                Texture.Dispose();
        }
    }
}
