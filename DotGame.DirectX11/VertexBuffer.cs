using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class VertexBuffer : IVertexBuffer
    {
        private GraphicsDevice graphicsDevice;
        public IGraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }

        public SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            this.graphicsDevice = graphicsDevice;
            this.Description = description;
        }

        public void SetData<T>(T[] data) where T : struct, IVertexType
        {
            this.VertexCount = data.Length;

            Buffer = SharpDX.Direct3D11.Buffer.Create(graphicsDevice.Context.Device, BindFlags.VertexBuffer, data);
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            if (Disposing != null)
                Disposing(this, EventArgs.Empty);

            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();


            IsDisposed = true;
        }
    }
}
