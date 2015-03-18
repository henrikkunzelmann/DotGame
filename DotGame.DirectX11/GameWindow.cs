using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.Graphics;

namespace DotGame.DirectX11
{
    public class GameWindow : IGameWindow
    {
        public IGraphicsDevice GraphicsDevice { get; private set; }

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

        public GameWindow(GraphicsDevice device, Control control)
        {
            if (device == null)
                throw new ArgumentNullException("device");
            if (device.IsDisposed)
                throw new ArgumentException("device is disposed", "device");
            if (control == null)
                throw new ArgumentNullException("control");
            if (control.IsDisposed)
                throw new ArgumentException("control is disposed", "control");

            this.GraphicsDevice = device;
            this.control = control;
        }
    }
}
