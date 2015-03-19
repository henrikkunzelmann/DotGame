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

        internal GraphicsContext Context { get; private set; }
        internal bool IsCurrent { get { return Context.IsCurrent; } }

        private Color clearColor;
        private float clearDepth;
        private int clearStencil;

        public GraphicsDevice(IGameWindow window, GraphicsContext context)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.IsDisposed)
                throw new ArgumentException("context is disposed.", "context");


            this.DefaultWindow = window;
            this.Context = context;

            Log.Debug("Got context: [ColorFormat: {0}, Depth: {1}, Stencil: {2}, FSAA Samples: {3}, AccumulatorFormat: {4}, Buffers: {5}, Stereo: {6}]",
                Context.GraphicsMode.ColorFormat,
                Context.GraphicsMode.Depth,
                Context.GraphicsMode.Stencil,
                Context.GraphicsMode.Samples,
                Context.GraphicsMode.AccumulatorFormat,
                Context.GraphicsMode.Buffers,
                Context.GraphicsMode.Stereo);

            Context.LoadAll();

            Factory = new GraphicsFactory(this);
        }
        ~GraphicsDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Clear(Color color)
        {
            SetClearColor(ref color);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            ClearBufferMask mask = ClearBufferMask.None;

            if (options.HasFlag(ClearOptions.Color))
            {
                SetClearColor(ref color);
                mask |= ClearBufferMask.ColorBufferBit;
            }

            if (options.HasFlag(ClearOptions.Depth))
            {
                SetClearDepth(ref depth);
                mask |= ClearBufferMask.DepthBufferBit;
            }

            if (options.HasFlag(ClearOptions.Stencil))
            {
                SetClearStencil(ref stencil);
                mask |= ClearBufferMask.StencilBufferBit;
            }

            GL.Clear(mask);
        }

        public void SwapBuffers()
        {
            Context.SwapBuffers();
        }

        private void SetClearColor(ref Color color)
        {
            if (color != clearColor)
            {
                clearColor = color;
                GL.ClearColor(color.R, color.G, color.B, color.A);
            }
        }

        private void SetClearDepth(ref float depth)
        {
            if (depth != clearDepth)
            {
                clearDepth = depth;
                GL.ClearDepth(depth);
            }
        }

        private void SetClearStencil(ref int stencil)
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
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            Factory.Dispose();
            Context.Dispose();
            IsDisposed = true;
        }

        public bool VSync
        {
            get
            {
                return Context.SwapInterval > 1;
            }
            set
            {
                //AssertCurrent
                if (value)
                {
                    int interval = (int)(1000f / DisplayDevice.Default.RefreshRate);
                    Context.SwapInterval = interval;
                }
                else
                    Context.SwapInterval = 1;
            }
        }
    }
}
