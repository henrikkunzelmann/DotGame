using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using System.Diagnostics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class Texture2D : GraphicsObject, ITexture2D, ITexture2DArray, IRenderTarget2D, IRenderTarget2DArray
    {
        public int Width { get { return Handle.Description.Width; } }
        public int Height { get { return Handle.Description.Height; } }
        public int MipLevels { get { return Handle.Description.MipLevels; } }
        public TextureFormat Format { get { return EnumConverter.ConvertToTexture(Handle.Description.Format); } }
        public int ArraySize { get { return Handle.Description.ArraySize; } }

        internal SharpDX.Direct3D11.Texture2D Handle { get; private set; }
        internal ShaderResourceView ResourceView { get; private set; }
        internal RenderTargetView RenderView { get; private set; }
        internal DepthStencilView DepthView { get; private set; }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int arraySize, int mipLevels)
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

            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                Format = EnumConverter.Convert(format),
                ArraySize = arraySize,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                MipLevels = mipLevels > 0 ? mipLevels : 1,
                Usage = ResourceUsage.Default
            };

            desc.BindFlags |= BindFlags.ShaderResource; 

            this.Handle = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc);
            CreateViews();
        }
        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, TextureFormat format, int arraySize, bool isRenderTarget, bool generateMipMaps)
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

            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                Format = EnumConverter.Convert(format),
                ArraySize = arraySize,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                MipLevels = !generateMipMaps ? 1 : 0,
                Usage = ResourceUsage.Default
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

            this.Handle = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Device, desc);
            CreateViews();
        }

        internal Texture2D(GraphicsDevice graphicsDevice, SharpDX.Direct3D11.Texture2D handle)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (handle == null)
                throw new ArgumentNullException("handle");
            if (handle.IsDisposed)
                throw new ArgumentException("Handle is disposed.", "handle");

            this.Handle = handle;

            CreateViews();
        }

        private void CreateViews()
        {
            if (Handle.Description.BindFlags.HasFlag(BindFlags.DepthStencil))
                DepthView = new DepthStencilView(graphicsDevice.Device, Handle,
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
            if (Handle.Description.BindFlags.HasFlag(BindFlags.RenderTarget))
                RenderView = new RenderTargetView(graphicsDevice.Device, Handle);
            if (Handle.Description.BindFlags.HasFlag(BindFlags.ShaderResource))
                ResourceView = new ShaderResourceView(graphicsDevice.Device, Handle,
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
            if (Handle != null && !Handle.IsDisposed)
                Handle.Dispose();
        }
    }
}
