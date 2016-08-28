using DotGame.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Windows.Forms;
using Utilities = OpenTK.Platform.Utilities;

namespace DotGame.OpenGL4.Windows
{
    public class GameControl : DotGame.Graphics.IGameWindow, IWindowContainer
    {
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
            get { return false; }
            set { throw new NotSupportedException("GameControl can't be fullscreen. Change the form to borderless fullscreen instead."); }
        }

        public IWindowInfo WindowInfo { get; private set; }

        public GameControl(Control control)
        {
            if (control == null)
                throw new ArgumentNullException("control");
            if (control.IsDisposed)
                throw new ArgumentException("control is disposed", "control");

            this.control = control;
        }

        public IGraphicsDevice CreateDevice(DeviceCreationFlags flags)
        {
            //SDL2 supported das Einbinden in ein Fenster nicht
            Toolkit toolkit = Toolkit.Init(new ToolkitOptions() { Backend = PlatformBackend.PreferNative });
            
            if (OpenTK.Configuration.RunningOnWindows)
                WindowInfo = Utilities.CreateWindowsWindowInfo(control.Handle);
            else if (OpenTK.Configuration.RunningOnMacOS)
                WindowInfo = Utilities.CreateMacOSWindowInfo(control.Handle);
            

            GraphicsContextFlags contextFlags = GraphicsContextFlags.Default;
            if (flags.HasFlag(DeviceCreationFlags.Debug))
                contextFlags = GraphicsContextFlags.Debug;

            GraphicsContext context = new GraphicsContext(GraphicsMode.Default, WindowInfo, 4, 5, contextFlags);
            context.LoadAll();
            return new GraphicsDevice(this, this, context, flags);
        }
    }
}
