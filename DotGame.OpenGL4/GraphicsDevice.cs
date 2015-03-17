using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.Graphics;
using DotGame.Utils;
using OpenTK.Graphics;
using OpenTK;

namespace DotGame.OpenGL4
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }

        internal readonly GLControl Control;
        internal IGraphicsContext Context { get { return Control.Context; } }
        internal bool IsCurrent { get { return Control.Context.IsCurrent; } }

        /// <summary>
        /// TODO: GameWindow speichern?
        /// </summary>
        public GraphicsDevice(IGameWindow gameWindow, Control container)
        {
            if (gameWindow == null)
                throw new ArgumentNullException("gameWindow");
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.IsDisposed)
                throw new ArgumentException("container is disposed.", "container");

            Log.Info("Initializing OpenGL GraphicsDevice on Container {0}...", container.Name);

            this.Control = new GLControl();
            this.Control.Name = "DotGame OpenGL GameWindow";
            this.Control.Dock = DockStyle.Fill;
            container.Controls.Add(this.Control);
        }

        ~GraphicsDevice()
        {
            dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Init()
        {
            this.Factory = new GraphicsFactory(this);

            Log.Debug("Got context: [ColorFormat: {0}, Depth: {1}, Stencil: {2}, FSAA Samples: {3}, AccumulatorFormat: {4}, Buffers: {5}, Stereo: {6}]",
                Context.GraphicsMode.ColorFormat,
                Context.GraphicsMode.Depth,
                Context.GraphicsMode.Stencil,
                Context.GraphicsMode.Samples,
                Context.GraphicsMode.AccumulatorFormat,
                Context.GraphicsMode.Buffers,
                Context.GraphicsMode.Stereo);

            Log.WriteFields(LogLevel.Info, Context.GraphicsMode);

            Control.Paint += Control_Paint;
        }

        void Control_Paint(object sender, PaintEventArgs e)
        {
            var argb = Color.CornflowerBlue.ToArgb();
            var color = Color.FromArgb(argb);
            OpenTK.Graphics.OpenGL4.GL.ClearColor(color.R, color.G, color.B, color.A);
            OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit);
            Control.SwapBuffers();
        }

        public void SwapBuffers()
        {
            Control.Invalidate();
        }

        public void Dispose()
        {
            Log.Info("GraphicsDevice.Dispose() called!");
            dispose(true);
        }

        private void dispose(bool isDisposing)
        {
            Factory.Dispose();
            Control.Dispose();
            IsDisposed = true;
        }
    }
}
