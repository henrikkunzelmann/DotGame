using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Graphics;
using System.Windows.Forms;

namespace DotGame
{
    public class Engine : IDisposable
    {
        /// <summary>
        /// Das GraphicsDevice welches die Engine nutzt.
        /// </summary>
        public IGraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Die Einstellungen mit dem die Engine gestartet wurde.
        /// </summary>
        public EngineSettings Settings { get; private set; }

        /// <summary>
        /// Die Version der Engine.
        /// </summary>
        public string Version
        {
            get { return "dev"; }
        }

        public Engine()
            : this(new EngineSettings())
        {
        }

        public Engine(EngineSettings settings)
            :this(settings, null)
        {
        }

        public Engine(EngineSettings settings, Control container)
        {
            this.Settings = settings;

            Log.Info("DotGame {0}", Version);
            Log.Info("===========");
            Log.Info("Engine starting...");

            switch (settings.GraphicsAPI)
            {
                case GraphicsAPI.OpenGL4:
                    this.GraphicsDevice = new DotGame.OpenGL4.Windows.GameControl(container).CreateDevice();
                    break;

                case GraphicsAPI.DirectX11:
                    this.GraphicsDevice = new DotGame.DirectX11.Windows.GameWindow(container).CreateDevice();
                    break;
            }


            Log.Debug("Got GraphicsDevice: " + GraphicsDevice.GetType().FullName);
            Log.Debug("Got window: [width: {0}, height: {1}]", GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height);
            Log.WriteFields(LogLevel.Verbose, settings);
        }

        public void Dispose()
        {
            Log.Info("Engine.Dispose() called!");
        }
    }
}
