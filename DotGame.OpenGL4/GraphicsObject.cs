using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using System.Diagnostics;

namespace DotGame.OpenGL4
{
    public abstract class GraphicsObject : IGraphicsObject
    {
        public IGraphicsDevice GraphicsDevice { get; private set; }
        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal readonly GraphicsDevice GraphicsDeviceInternal;
        internal readonly StackTrace CreationTrace;

        public GraphicsObject(GraphicsDevice graphicsDevice, StackTrace creationTrace)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("graphicsDevice is disposed.", "graphicsDevice");

            this.GraphicsDevice = graphicsDevice;
            this.GraphicsDeviceInternal = graphicsDevice;
            this.CreationTrace = creationTrace;
        }

        ~GraphicsObject()
        {
            if (GraphicsDevice.IsDisposed || GraphicsDeviceInternal.IsCurrent)
                Dispose(false);
            else
                ((GraphicsFactory)GraphicsDevice.Factory).DeferredDispose.Add(this);
        }

        public void Dispose()
        {
            if (GraphicsDevice.IsDisposed || GraphicsDeviceInternal.IsCurrent)
                Dispose(true);
            else
                ((GraphicsFactory)GraphicsDevice.Factory).DeferredDispose.Add(this);

            GC.SuppressFinalize(this);
        }

        protected void AssertNotDisposed()
        {
            if (GraphicsDevice.IsDisposed)
                throw new ObjectDisposedException(GraphicsDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected void AssertCurrent()
        {
            AssertNotDisposed();
            
            // TODO (Joex3): Eigene Exception.
            if (!GraphicsDeviceInternal.IsCurrent)
                throw new Exception(string.Format("GraphicsDevice is not available on Thread {0}.", System.Threading.Thread.CurrentThread.Name));
        }

        internal virtual void Dispose(bool isDisposing)
        {
            if (Disposing != null)
                Disposing.Invoke(this, EventArgs.Empty);

            IsDisposed = true;
        }
    }
}
