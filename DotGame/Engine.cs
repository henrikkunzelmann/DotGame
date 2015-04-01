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

        private IGameWindow window;
        private Thread thread;
        private bool shouldStop = false;

        /// <summary>
        /// Wird aufgerufen wenn die Engine gestartet hat.
        /// </summary>
        private ManualResetEvent onStart = new ManualResetEvent(false);

        // TODO (henrik1235) Test, entfernen
        private IShader shader;
        private IConstantBuffer constantBuffer;
        private IVertexBuffer vertexBuffer;

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
            this.GraphicsDevice = window.CreateDevice();
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


            // TODO (henrik1235) Test, entfernen
            shader = GraphicsDevice.Factory.CompileShader("testShader", new ShaderCompileInfo("shader.fx", "VS", "vs_4_0"), new ShaderCompileInfo("shader.fx", "PS", "ps_4_0"));
            constantBuffer = shader.CreateConstantBuffer();
            vertexBuffer = GraphicsDevice.Factory.CreateVertexBuffer(new float[] {
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                
                -1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                
                -1.0f, 1.0f, -1.0f, 
                -1.0f, 1.0f,  1.0f, 
                 1.0f, 1.0f,  1.0f, 
                -1.0f, 1.0f, -1.0f, 
                 1.0f, 1.0f,  1.0f, 
                 1.0f, 1.0f, -1.0f, 
                
                -1.0f,-1.0f, -1.0f, 
                 1.0f,-1.0f,  1.0f, 
                -1.0f,-1.0f,  1.0f, 
                -1.0f,-1.0f, -1.0f, 
                 1.0f,-1.0f, -1.0f, 
                 1.0f,-1.0f,  1.0f, 
                
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
            }, Geometry.VertexPosition.Description);
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
            Log.FlushBuffer();
        }

        /// <summary>
        /// Wird jeden Tick aufgerufen. 
        /// </summary>
        private void Tick(GameTime gameTime)
        {
            float time = (float)gameTime.TotalTime.TotalSeconds;
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5f), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PI / 4f, GraphicsDevice.DefaultWindow.Width / (float)GraphicsDevice.DefaultWindow.Height, 0.1f, 100.0f);
            var worldViewProj = 
                  Matrix.CreateRotationX(time)
                * Matrix.CreateRotationY(time * 2)
                * Matrix.CreateRotationZ(time * .7f) * view * proj;
            worldViewProj.Transpose();

            GraphicsDevice.Clear(ClearOptions.ColorDepthStencil, Color.SkyBlue, 1f, 0);
            GraphicsDevice.RenderContext.SetShader(shader);
            shader.SetConstantBuffer(constantBuffer);
            constantBuffer.UpdateData(worldViewProj);
            GraphicsDevice.RenderContext.SetPrimitiveType(PrimitiveType.TriangleList);
            GraphicsDevice.RenderContext.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.RenderContext.Draw();
            GraphicsDevice.SwapBuffers();
        }

        public void Dispose()
        {
            if (IsRunning)
                Stop();
            Log.Info("Engine.Dispose() called!");
            Log.FlushBuffer();
        }
    }
}
