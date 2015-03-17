using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.OpenGL4
{
    public abstract class GraphicsObject : IGraphicsObject
    {
        public IGraphicsDevice GraphicsDevice { get; private set; }
        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal readonly GraphicsDevice GraphicsDeviceInternal;

        public GraphicsObject(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("graphicsDevice is disposed.", "graphicsDevice");

            this.GraphicsDevice = GraphicsDevice;
            this.GraphicsDeviceInternal = graphicsDevice;
        }

        ~GraphicsObject()
        {
            dispose(false);
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void assertNotDisposed()
        {
            if (GraphicsDevice.IsDisposed)
                throw new ObjectDisposedException(GraphicsDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected void assertCurrent()
        {
            assertNotDisposed();
            
            // TODO: Eigene Exception.
            if (!GraphicsDeviceInternal.IsCurrent)
                throw new Exception(string.Format("GraphicsDevice is not available on Thread {0}.", System.Threading.Thread.CurrentThread.Name));
        }

        protected virtual void dispose(bool isDisposing)
        {
            if (Disposing != null)
                Disposing.Invoke(this, EventArgs.Empty);

            IsDisposed = true;
        }
    }
}
