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
using DotGame.OpenGL4.Windows;

namespace DotGame.OpenGL4
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }
        public IRenderContext RenderContext
        {
            get { throw new NotImplementedException(); }
        }

        public IGameWindow DefaultWindow { get; private set; }
        public bool VSync
        {
            get { return Context.SwapInterval > 1; }
            set
            {
                //AssertCurrent
                Context.SwapInterval = value ? 1 : 0;
            }
        }

        internal GraphicsContext Context { get; private set; }
        internal bool IsCurrent { get { return Context.IsCurrent; } }

        private IWindowContainer container;
        private Color clearColor;
        private float clearDepth;
        private int clearStencil;

        internal static int MipLevels(int width, int height, int depth = 0)
        {
            var max = Math.Max(width, Math.Max(height, depth));
            return (int)Math.Ceiling(Math.Log(max, 2));
        }

        public GraphicsDevice(IGameWindow window, IWindowContainer container, GraphicsContext context)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (container == null)
                throw new ArgumentNullException("container");
            if (context == null)
                throw new ArgumentNullException("context");
            if (context.IsDisposed)
                throw new ArgumentException("context is disposed.", "context");


            this.DefaultWindow = window;
            this.container = container;
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
            Context.MakeCurrent(null);
        }
        ~GraphicsDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void MakeCurrent()
        {
            Context.MakeCurrent(container.WindowInfo);
        }

        public int GetSizeOf(TextureFormat format)
        {
            throw new NotImplementedException();
        }

        public int GetSizeOf(VertexElementType format)
        {
            switch (format)
            {
                case VertexElementType.Single:
                    return 4;
                case VertexElementType.Vector2:
                    return 8;
                case VertexElementType.Vector3:
                    return 12;
                case VertexElementType.Vector4:
                    return 16;
                default:
                    return 0;
            }
        }

        public int GetSizeOf(VertexDescription description)
        {
            int size = 0;
            VertexElement[] elements = description.GetElements();
            for (int i = 0; i < elements.Length; i++)
                size += GetSizeOf(elements[i].Type);

            return size;
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
            // TODO (Joex3): Evtl. woanders hinschieben.
            ((GraphicsFactory)Factory).DisposeUnused();

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

        internal static void CheckGLError()
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                throw new InvalidOperationException(error.ToString());
            }
        }

        public void Dispose()
        {
            Log.Info("GraphicsDevice.Dispose() called!");
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            Context.Dispose();
            IsDisposed = true;
            // Wird hinter IsDisposed = true; aufgerufen, damit die GraphicsObject Dispose Implementationen bescheid wissen, dass nicht mehr auf das GraphicsDevice zugegriffen werden kann.
            Factory.Dispose();
        }
    }
}
