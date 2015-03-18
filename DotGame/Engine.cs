using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Graphics;

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

        public Engine(IGraphicsDevice device)
            : this(device, new EngineSettings())
        {
        }

        public Engine(IGraphicsDevice device, EngineSettings settings)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            this.GraphicsDevice = device;
            this.Settings = settings;

            Log.Info("DotGame {0}", Version);
            Log.Info("===========");
            Log.Info("Engine starting...");

            Log.Debug("Got GraphicsDevice: " + device.GetType().FullName);
            Log.Debug("Got window: [width: {0}, height: {1}]", device.DefaultWindow.Width, device.DefaultWindow.Height);
            Log.WriteFields(LogLevel.Verbose, settings);
        }

        public void Dispose()
        {
            Log.Info("Engine.Dispose() called!");
        }
    }
}
