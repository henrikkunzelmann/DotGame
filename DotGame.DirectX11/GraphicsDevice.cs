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

        internal readonly RenderControl Control;

        internal DeviceContext Context { get; private set; } 

        public GraphicsDevice(Control container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.IsDisposed)
                throw new ArgumentException("container is disposed.", "container");

            this.Control = new RenderControl();
            this.Control.Name = "DotGame DirectX GameWindow";
            this.Control.Dock = DockStyle.Fill;
            this.Control.Load += Control_Load;
            container.Controls.Add(this.Control);


            Factory = new GraphicsFactory(this);
            DefaultWindow = new GameWindow(this, container);


        }

        void Control_Load(object sender, EventArgs e)
        {
            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(Control.Width, Control.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Control.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.Debug, swapChainDescription, out device, out swapChain);
            Context = device.ImmediateContext;

            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Control.Handle, WindowAssociationFlags.IgnoreAll);
            
            backBuffer = swapChain.GetBackBuffer<Texture2D>(0);
            renderTargetView = new RenderTargetView(device, backBuffer);

            depthBuffer = new SharpDX.Direct3D11.Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = Control.Width,
                Height = Control.Height,
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
