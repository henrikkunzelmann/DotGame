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
            ControlEventArgs controlEventArgs = (ControlEventArgs)e;

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(controlEventArgs.Control.Width, controlEventArgs.Control.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = controlEventArgs.Control.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.Debug, swapChainDescription, out device, out swapChain);

            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(controlEventArgs.Control.Handle, WindowAssociationFlags.IgnoreAll);


            backBuffer = swapChain.GetBackBuffer<Texture2D>(0);
            renderTargetView = new RenderTargetView(device, backBuffer);

            depthBuffer = new SharpDX.Direct3D11.Texture2D(device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = controlEventArgs.Control.Width,
                Height = controlEventArgs.Control.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
            });
            depthStencilView = new DepthStencilView(device, depthBuffer);

            device.ImmediateContext.OutputMerger.SetTargets(depthStencilView, renderTargetView);
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
            throw new NotImplementedException();
        }


        public void Clear(Color color)
        {
            throw new NotImplementedException();
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }
    }
}
