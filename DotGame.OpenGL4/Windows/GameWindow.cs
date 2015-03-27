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
    public class GameWindow : OpenTK.GameWindow, IGameWindow, IWindowContainer
    {
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

        public GameWindow() : base() { }
        public GameWindow(int width, int height) : base(width, height) { }
        public GameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }


        public IGraphicsDevice CreateDevice()
        {
            Run();
            GraphicsContext context = (GraphicsContext)this.Context;
            return new GraphicsDevice(this, this, context);
        }
    }
}
