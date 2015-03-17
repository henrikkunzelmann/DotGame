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
        Device device;
        internal readonly RenderControl Control;

        public GraphicsDevice(IGameWindow gameWindow, Control container)
        {
            if (gameWindow == null)
                throw new ArgumentNullException("gameWindow");
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.IsDisposed)
                throw new ArgumentException("container is disposed.", "container");

            this.Control = new RenderControl();
            this.Control.Name = "DotGameDirectX GameWindow";
            this.Control.Dock = DockStyle.Fill;
            this.Control.Load += Control_Load;
            container.Controls.Add(this.Control);

        }

        void Control_Load(object sender, EventArgs e)
        {
        }


        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public void SwapBuffers()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
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
