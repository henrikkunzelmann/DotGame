using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;

namespace DotGame
{
    public class Engine : IDisposable
    {
        /// <summary>
        /// Das IGameWindow in dem die Engine zeichnet.
        /// </summary>
        public IGameWindow Window { get; private set; }

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

        public Engine(IGameWindow window)
            : this(window, new EngineSettings())
        {
        }

        public Engine(IGameWindow window, EngineSettings settings)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            this.Window = window;
            this.Settings = settings;

            Log.Info("DotGame {0}", Version);
            Log.Info("===========");
            Log.Info("Engine starting...");

            Log.Debug("Got window: [width: {0}, height: {1}]", window.Width, window.Height);
            Log.WriteFields(LogLevel.Verbose, settings);
        }

        public void Dispose()
        {
            Log.Info("Engine.Dispose() called!");
        }
    }
}
