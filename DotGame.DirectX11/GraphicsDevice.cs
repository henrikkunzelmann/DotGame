using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX.DXGI;

namespace DotGame.DirectX11
{
    public class GraphicsDevice : IGraphicsDevice
    {
        private Device device;
        private SwapChain swapChain;

        private RenderTargetView renderTargetView;
        private Texture2D backBuffer;
        private DepthStencilView depthStencilView;
        private Texture2D depthBuffer;

        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }
        
        internal DeviceContext Context { get; private set; } 

        internal GraphicsDevice(IGameWindow window, Device device, SwapChain swapChain)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (device == null)
                throw new ArgumentNullException("device");
            if (device.IsDisposed)
                throw new ArgumentException("device is disposed", "device");
            if (swapChain == null)
                throw new ArgumentNullException("swapChain");
            if (swapChain.IsDisposed)
                throw new ArgumentException("swapChain is disposed", "swapChain");

            this.DefaultWindow = window;
            this.device = device;
            this.swapChain = swapChain;
            this.Context = device.ImmediateContext;

            this.Factory = new GraphicsFactory(this);

            InitBackbuffer();
        }

        private void InitBackbuffer()
        {
            backBuffer = swapChain.GetBackBuffer<Texture2D>(0);
            renderTargetView = new RenderTargetView(device, backBuffer);

            depthBuffer = new SharpDX.Direct3D11.Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = backBuffer.Description.Width,
                Height = backBuffer.Description.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
            });
            depthStencilView = new DepthStencilView(device, depthBuffer);

            Context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            if (Factory != null && !Factory.IsDisposed)
                Factory.Dispose();

            IsDisposed = true;
        }

        public void SwapBuffers()
        {
            swapChain.Present(0, PresentFlags.None);
        }


        public void Clear(Color color)
        {
            Context.ClearRenderTargetView(renderTargetView, new SharpDX.Color4(color.R, color.B, color.G, color.A));
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            if (clearOptions.HasFlag(ClearOptions.Color))
            {
                Clear(color);
            }
            else if (clearOptions.HasFlag(ClearOptions.Depth) && clearOptions.HasFlag(ClearOptions.Depth))
            {
                Context.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, depth, (byte)stencil);
            }
            else if (clearOptions.HasFlag(ClearOptions.Depth))
            {
                Context.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, depth, (byte)stencil);
            }
            else if (clearOptions.HasFlag(ClearOptions.Depth))
            {
                Context.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Stencil, depth, (byte)stencil);
            } 
        }
    }
}
