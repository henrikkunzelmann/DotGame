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

        public GameWindow() : base(800, 600, GraphicsMode.Default, "DotGame", GameWindowFlags.Default, DisplayDevice.Default, 4,7, GraphicsContextFlags.Default) { }
        public GameWindow(int width, int height) : base(width, height, GraphicsMode.Default, "DotGame", GameWindowFlags.Default, DisplayDevice.Default, 4, 7, GraphicsContextFlags.Default) { }
        public GameWindow(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 7, GraphicsContextFlags.Default) { }


        public IGraphicsDevice CreateDevice(DeviceCreationFlags flags)
        {
            this.Visible = true;
            GraphicsContext context = (GraphicsContext)this.Context;
            return new GraphicsDevice(this, this, context, flags);
        }
    }
}
