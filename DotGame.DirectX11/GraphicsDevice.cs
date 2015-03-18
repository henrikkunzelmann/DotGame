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

        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }

        public GraphicsDevice(Control container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.IsDisposed)
                throw new ArgumentException("container is disposed.", "container");

            Factory = new GraphicsFactory(this);
            DefaultWindow = new GameWindow(this, container);
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
