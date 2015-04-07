using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Utils;
using DotGame.Graphics;
using System.Windows.Forms;
using DotGame.Audio;

namespace DotGame
{
    public class Engine : IDisposable
    {
        /// <summary>
        /// Das GraphicsDevice welches die Engine nutzt.
        /// </summary>
        public IGraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Das AudioDevice welches die Engine nutzt.
        /// </summary>
        public IAudioDevice AudioDevice { get; private set; }

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

        /// <summary>
        /// Gibt an ob die Engine gerade läuft.
        /// </summary>
        public bool IsRunning { get; private set; }

        private object locker = new object();

        private IGameWindow window;
        private Thread thread;
        private bool shouldStop = false;

        /// <summary>
        /// Wird aufgerufen wenn die Engine gestartet hat.
        /// </summary>
        private ManualResetEvent onStart = new ManualResetEvent(false);

        // der Grund für drei Liste erlaubt es das man Komponenten flexibler hinzufügen bzw. entfernen kann
        private List<GameComponent> components = new List<GameComponent>();
        private List<GameComponent> componentsToAdd = new List<GameComponent>();
        private List<GameComponent> componentsToRemove = new List<GameComponent>();

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
            if (container != null && container.IsDisposed)
                throw new ArgumentException("Container is disposed.", "container");
            this.Settings = settings;

            Log.Info("");
            Log.Info("===========");
            Log.Info("DotGame {0}", Version);
            Log.Info("===========");
            Log.Info("Engine starting...");

            switch (Settings.GraphicsAPI)
            {
                case GraphicsAPI.OpenGL4:
                    if (container == null)
                        window = new DotGame.OpenGL4.Windows.GameWindow();
                    else
                        window = new DotGame.OpenGL4.Windows.GameControl(container);
                    break;

                case GraphicsAPI.DirectX11:
                    if (container == null)
                        window = new DotGame.DirectX11.Windows.GameWindow();
                    else
                        window = new DotGame.DirectX11.Windows.GameControl(container);
                    break;
                default:
                    throw new NotImplementedException("GraphicsAPI not implemented.");
            }

            // Engine initialisieren
            Init();

            thread = new Thread(new ThreadStart(Run));
            thread.Start();

            // auf Start warten
            onStart.WaitOne();

            Log.Info("Engine init done!");
            Log.Info("===========");
            Log.FlushBuffer();
        }

        private void Init()
        {
            this.GraphicsDevice = window.CreateDevice(DeviceCreationFlags.Debug);
            GraphicsDevice.MakeCurrent();
            Log.Debug("Got GraphicsDevice: " + GraphicsDevice.GetType().FullName);
            Log.Debug("Got window: [width: {0}, height: {1}]", GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height);
            Log.WriteFields(LogLevel.Verbose, Settings);

            switch (Settings.AudioAPI)
            {
                case AudioAPI.OpenAL:
                    this.AudioDevice = new DotGame.OpenAL.AudioDevice(Settings.AudioDevice);
                    break;
            }

            Log.Debug("Got AudioDevice: " + this.AudioDevice.GetType().FullName);
            GraphicsDevice.DetachCurrent();
        }

        private void Run()
        {
            GraphicsDevice.MakeCurrent();
            IsRunning = true;
            onStart.Set(); // allen anderen Threads sagen, dass der Init fertig ist

            Stopwatch gameTime = new Stopwatch();
            Stopwatch frameTime = new Stopwatch();
            long tickCount = 0;
            gameTime.Start();
            frameTime.Start();

            while(!shouldStop)
            {
                TimeSpan frame = frameTime.Elapsed;
                frameTime.Restart();

                Tick(new GameTime(gameTime.Elapsed, frame, tickCount));

                const int targetFPS = 60;
                //Thread.Sleep ist viel zu ungenau
                //int sleepTime = (1000 / targetFPS) - (int)frameTime.ElapsedMilliseconds;
                //if (sleepTime > 0)
                //    Thread.Sleep(sleepTime); 
                while (frameTime.ElapsedTicks < (1f / targetFPS) * Stopwatch.Frequency) ;
                tickCount++;
            }

            // Engine beenden
            Log.Info("Engine has stopped!");
            Log.FlushBuffer();
            gameTime.Stop();
            IsRunning = false;
        }

        /// <summary>
        /// Stoppt die Engine.
        /// </summary>
        /// <exception cref="InvalidOperationException">Wenn die Engine schon gestoppt wurde.</exception>
        public void Stop()
        {
            lock (locker)
            {
                if (!IsRunning)
                    throw new InvalidOperationException("Engine is not running.");

                Log.Info("Engine.Stop() called!");

                // Thread sagen er soll stoppen
                shouldStop = true;
                thread.Join(2000); // 2000ms warten
                // wenn Thread zu lange brauch (sich z.B. aufgehängt hat)
                if (thread.IsAlive)
                {
                    // per Gewalt stoppen
                    thread.Abort();
                    IsRunning = false;

                    Log.Warning("Engine force stopped!");
                }

                lock (components)
                {
                    foreach (GameComponent component in components)
                        component.Unload();
                }


                Log.FlushBuffer();
            }
        }

        /// <summary>
        /// Wird jeden Tick aufgerufen. 
        /// </summary>
        private void Tick(GameTime gameTime)
        {
            lock (components)
            {
                // alle Komponenten die noch nicht hinzugefügt wurden hinzufügen und initialisieren
                foreach (GameComponent component in componentsToAdd)
                    if (!components.Contains(component))
                    {
                        components.Add(component);
                        component.Init();
                    }

                // alle Komponenten die entfernt werden sollen entfernen und entladen
                foreach (GameComponent component in componentsToRemove)
                {
                    components.Remove(component);
                    component.Unload();
                }

                // alle Komponenten aktualisieren
                foreach (GameComponent component in components)
                    if (!componentsToRemove.Contains(component))
                        component.Update(gameTime);

                // alle Komponenten zeichnen
                foreach (GameComponent component in components)
                    if (!componentsToRemove.Contains(component))
                        component.Draw(gameTime);

                componentsToAdd.Clear();
                componentsToRemove.Clear();
            }

            GraphicsDevice.SwapBuffers();
        }

        public void Dispose()
        {
            lock (locker)
            {
                if (IsRunning)
                    Stop();

                Dispose(true);
                Log.Info("Engine.Dispose() called!");
                Log.FlushBuffer();

                GC.SuppressFinalize(this);
            }
        }

        public void AddComponent(GameComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            lock (components)
            {
                if (componentsToRemove.Contains(component))
                    componentsToRemove.Remove(component);
                if (!components.Contains(component))
                    componentsToAdd.Add(component);
            }
        }

        public void RemoveComponent(GameComponent component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            lock (components)
            {
                if (componentsToAdd.Contains(component))
                    componentsToAdd.Remove(component);
                if (components.Contains(component))
                    componentsToRemove.Add(component);
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (GraphicsDevice != null)
                GraphicsDevice.Dispose();
            if (AudioDevice != null)
                AudioDevice.Dispose();
            if (onStart != null)
                onStart.Dispose();
        }
    }
}
