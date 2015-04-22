using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Windows;
using SharpDX.Direct3D;
using Device = SharpDX.Direct3D11.Device;

namespace DotGame.DirectX11.Windows
{
    public class GameWindow : RenderForm, IGameWindow
    {
        IGraphicsDevice graphicsDevice;
        SwapChain swapChain;
        
        public GameWindow() : base() { }

        public GameWindow(int width, int height) : base()
        {
            ClientSize = new System.Drawing.Size(width, height);
        }

        public GameWindow(int width, int height, string title) : base(title)
        {
            ClientSize = new System.Drawing.Size(width, height);
        }

        public IGraphicsDevice CreateDevice(DotGame.Graphics.DeviceCreationFlags flags)
        {
            if (graphicsDevice != null)
                return graphicsDevice;

            SwapChainDescription swapChainDescription = new SwapChainDescription()
            {
                BufferCount = 1,
                Flags = SwapChainFlags.AllowModeSwitch,
                IsWindowed = true,
                ModeDescription = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                OutputHandle = Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device device;
            Device.CreateWithSwapChain(DriverType.Hardware, EnumConverter.Convert(flags), swapChainDescription, out device, out swapChain);

            Factory factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAll);

            graphicsDevice = new GraphicsDevice(this, device, swapChain, flags);

            this.Show();
            return graphicsDevice;
        }

        public bool IsFullScreen
        {
            get
            {
                return swapChain.IsFullScreen;
            }
            set
            {
                swapChain.SetFullscreenState(value, null);
            }
        }
    }
}
