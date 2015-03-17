using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotGame.Windows
{
    /// <summary>
    /// Die Windows-Forms Implementation für das IGameWindow.
    /// </summary>
    public class GameWindow : IGameWindow
    {
        private Control control;

        public Graphics.IGraphicsDevice GraphicsDevice { get; private set; }

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

        public GameWindow(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            if (control.IsDisposed)
                throw new ArgumentException("control is disposed.", "control");
            this.control = control;

            // TODO: EngineSettings beachten; Das hier in Engine.cs verschieben;
            GraphicsDevice = new DotGame.OpenGL4.GraphicsDevice(this, control);
        }
    }
}
