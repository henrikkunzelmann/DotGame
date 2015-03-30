using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using System.Diagnostics;

namespace DotGame.DirectX11
{
    public abstract class GraphicsObject : IGraphicsObject
    {
        public IGraphicsDevice GraphicsDevice { get { return graphicsDevice; } }
        public bool IsDisposed { get; private set; }
        public object Tag { get; set; }
        public event EventHandler<EventArgs> OnDisposing;
        public event EventHandler<EventArgs> OnDisposed;

        protected GraphicsDevice graphicsDevice;
        private StackTrace creationStack;

        public GraphicsObject(GraphicsDevice graphicsDevice, StackTrace creationStack)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");
            if (creationStack == null)
                throw new ArgumentNullException("creationStack");

            this.graphicsDevice = graphicsDevice;
            this.creationStack = creationStack;
        }

        ~GraphicsObject()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            if (OnDisposing != null)
                OnDisposing(this, EventArgs.Empty);

            
            Dispose(true);
            IsDisposed = true;
            GC.SuppressFinalize(this);

            if (OnDisposed != null)
                OnDisposed(this, EventArgs.Empty);
        }

        protected abstract void Dispose(bool isDisposing);

        protected void AssertNotDisposed()
        {
            if (GraphicsDevice.IsDisposed)
                throw new ObjectDisposedException(GraphicsDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
