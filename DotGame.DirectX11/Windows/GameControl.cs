using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.Graphics;
using SharpDX.Win32;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using SharpDX.DXGI;

namespace DotGame.DirectX11.Windows
{
    public class GameControl : IGameWindow
    {
        private GraphicsDevice graphicsDevice;
        private Control control;

        public int Width
        {
            get { return control.Width; }
            set { control.Width = value; }
        }

        public int Height
        {
            get { return control.Height; }
            set { control.Height = value; }
        }

        public bool IsFullScreen
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public GameControl(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            if (control.IsDisposed)
                throw new ArgumentException("control is disposed", "control");

            this.control = control;
        }

        public IGraphicsDevice CreateDevice(DotGame.Graphics.DeviceCreationFlags flags)
        {
            if (graphicsDevice != null)
                return graphicsDevice;

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                Flags = SwapChainFlags.None,
                IsWindowed = true,
                ModeDescription = new ModeDescription(control.Width, control.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = control.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device device;
            SwapChain swapChain;
            Device.CreateWithSwapChain(DriverType.Hardware, EnumConverter.Convert(flags), swapChainDescription, out device, out swapChain);

            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(control.Handle, WindowAssociationFlags.IgnoreAll);

            graphicsDevice = new GraphicsDevice(this, device, swapChain, flags);
            return graphicsDevice;
        }
    }
}
