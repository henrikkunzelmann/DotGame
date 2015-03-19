using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using DotGame.Graphics;
using OpenTK.Graphics;

namespace DotGame.OpenGL4.Windows
{
    class GameWindow : OpenTK.GameWindow, IGameWindow
    {
        public GameWindow() : base() { }
        public GameWindow(int width, int height) : base(width, height) { }
        public GameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }


        public IGraphicsDevice CreateDevice()
        {
            GraphicsContext context = (GraphicsContext)this.Context;
            return new GraphicsDevice(this, context);
        }

        public bool IsFullScreen 
        { 
            get 
            {
                return WindowState == OpenTK.WindowState.Fullscreen;
            } 
            set 
            {
                if (value)
                {
                    WindowState = OpenTK.WindowState.Fullscreen;
                }
                else 
                {
                    WindowState = OpenTK.WindowState.Normal;
                }
            } 
        }

        public new bool VSync
        {
            get { return base.VSync == VSyncMode.On; }
            set
            {
                if (value)
                {
                    base.VSync = VSyncMode.On;
                }
                else
                {
                    base.VSync = VSyncMode.Off;
                }
            }
        }
    }
}
