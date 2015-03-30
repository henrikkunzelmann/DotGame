using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class Texture2D : ITexture2D, ITexture2DArray, IRenderTarget2D, IRenderTarget2DArray
    {
        private GraphicsDevice graphicsDevice;

        public IGraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public int Width { get { return Handle.Description.Width; } }
        public int Height { get { return Handle.Description.Height; } }
        public int MipLevels { get { return Handle.Description.MipLevels; } }
        public TextureFormat Format { get { return FormatConverter.ConvertToTexture(Handle.Description.Format); } }
        public int ArraySize { get { return Handle.Description.ArraySize; } }

        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }

        public object Tag { get; set; }

        internal SharpDX.Direct3D11.Texture2D Handle { get; private set; }
        internal SharpDX.Direct3D11.ShaderResourceView ResourceView { get; private set; }
        internal SharpDX.Direct3D11.RenderTargetView RenderView { get; private set; }
        internal SharpDX.Direct3D11.DepthStencilView DepthView { get; private set; }

        internal Texture2D(GraphicsDevice graphicsDevice, int width, int height, int mipLevels, TextureFormat format, int arraySize, bool isRenderTarget)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width", "Width must be positive.");
            if (height <= 0)
                throw new ArgumentOutOfRangeException("height", "Height must be positive.");
            if (mipLevels < 0)
                throw new ArgumentOutOfRangeException("mipLevels", "MipLevels must be not negative.");
            if (isRenderTarget && mipLevels != 0)
                throw new ArgumentException("MipLevels can not be set with isRenderTarget.", "mipLevels");
            if (format == TextureFormat.Unknown)
                throw new ArgumentException("Format must be not TextureFormat.Unkown.", "format");
            if (arraySize <= 0)
                throw new ArgumentOutOfRangeException("arraySize", "ArraySize must be positive.");

            this.graphicsDevice = graphicsDevice;

            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                Format = FormatConverter.Convert(format),
                ArraySize = arraySize,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                CpuAccessFlags = CpuAccessFlags.None,
                MipLevels = isRenderTarget ? 1 : 0,
                Usage = ResourceUsage.Default
            };
            if (isRenderTarget)
                if (format.IsDepth())
                    desc.BindFlags |= BindFlags.DepthStencil;
                else
                    desc.BindFlags |= BindFlags.RenderTarget;
            else
            {
                desc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps; // TODO (Robin) GenerateMipMaps benötigt das RenderTarget BindFlag.
                desc.BindFlags |= BindFlags.ShaderResource; // TODO (henrik1235) ShaderResource auch für RenderTargets und DepthBuffer erlauben
            }

            this.Handle = new SharpDX.Direct3D11.Texture2D(graphicsDevice.Context.Device, desc);
            CreateViews();
        }

        internal Texture2D(GraphicsDevice graphicsDevice, SharpDX.Direct3D11.Texture2D handle)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (handle == null)
                throw new ArgumentNullException("handle");

            this.graphicsDevice = graphicsDevice;
            this.Handle = handle;

            CreateViews();
        }

        private void CreateViews()
        {
            if (Handle.Description.BindFlags.HasFlag(BindFlags.DepthStencil))
                DepthView = new DepthStencilView(graphicsDevice.Context.Device, Handle);
            if (Handle.Description.BindFlags.HasFlag(BindFlags.RenderTarget))
                RenderView = new RenderTargetView(graphicsDevice.Context.Device, Handle);
            if (Handle.Description.BindFlags.HasFlag(BindFlags.ShaderResource))
                ResourceView = new ShaderResourceView(graphicsDevice.Context.Device, Handle);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            if (Disposing != null)
                Disposing(this, EventArgs.Empty);

            if (ResourceView != null && !ResourceView.IsDisposed)
                ResourceView.Dispose();
            if (RenderView != null && !RenderView.IsDisposed)
                RenderView.Dispose();
            if (Handle != null && !Handle.IsDisposed)
                Handle.Dispose();

            IsDisposed = true;
        }
    }
}
