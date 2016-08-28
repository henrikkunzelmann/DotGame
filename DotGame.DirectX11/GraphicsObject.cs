using DotGame.Graphics;
using System;
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

        public StackTrace CreationStack { get; private set; }

        public GraphicsObject(GraphicsDevice graphicsDevice, StackTrace creationStack)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ObjectDisposedException("graphicsDevice");
            if (creationStack == null)
                throw new ArgumentNullException("creationStack");

            this.graphicsDevice = graphicsDevice;
            this.CreationStack = creationStack;

            graphicsDevice.CreatedObjects.Add(this);
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

            graphicsDevice.CreatedObjects.Remove(this);

            if (OnDisposed != null)
                OnDisposed(this, EventArgs.Empty);

            DotGame.Utils.Log.Debug("(Dispose) {0} disposed", GetType().FullName);
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
