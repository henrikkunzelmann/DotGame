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
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }

        internal readonly GLControl Control;
        internal IGraphicsContext Context { get { return Control.Context; } }
        internal bool IsCurrent { get { return Control.Context.IsCurrent; } }

        private Color clearColor;
        private float clearDepth;
        private int clearStencil;

        public GraphicsDevice(Control container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (container.IsDisposed)
                throw new ArgumentException("container is disposed.", "container");

            Log.Info("Initializing OpenGL GraphicsDevice on Container {0}...", container.Name);

            this.Control = new GLControl();
            this.Control.Name = "DotGame OpenGL GameWindow";
            this.Control.Dock = DockStyle.Fill;
            this.Control.Load += Control_Load;
            container.Controls.Add(this.Control);

            this.DefaultWindow = new GameWindow(this, this.Control);
        }
        ~GraphicsDevice()
        {
            dispose(false);
            GC.SuppressFinalize(this);
        }

        void Control_Load(object sender, EventArgs e)
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

            Control.Paint += Control_Paint;
        }

        void Control_Paint(object sender, PaintEventArgs e)
        {
            Control.SwapBuffers();
        }

        public void Clear(Color color)
        {
            setClearColor(ref color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            ClearBufferMask mask = ClearBufferMask.None;

            if (options.HasFlag(ClearOptions.Color))
            {
                setClearColor(ref color);
                mask |= ClearBufferMask.ColorBufferBit;
            }

            if (options.HasFlag(ClearOptions.Depth))
            {
                setClearDepth(ref depth);
                mask |= ClearBufferMask.DepthBufferBit;
            }

            if (options.HasFlag(ClearOptions.Stencil))
            {
                setClearStencil(ref stencil);
                mask |= ClearBufferMask.StencilBufferBit;
            }
        }

        public void SwapBuffers()
        {
            Control.Invalidate();
        }

        private void setClearColor(ref Color color)
        {
            if (color != clearColor)
            {
                clearColor = color;
                GL.ClearColor(color.R, color.G, color.B, color.A);
            }
        }

        private void setClearDepth(ref float depth)
        {
            if (depth != clearDepth)
            {
                clearDepth = depth;
                GL.ClearDepth(depth);
            }
        }

        private void setClearStencil(ref int stencil)
        {
            if (stencil != clearStencil)
            {
                clearStencil = stencil;
                GL.ClearStencil(stencil);
            }
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
