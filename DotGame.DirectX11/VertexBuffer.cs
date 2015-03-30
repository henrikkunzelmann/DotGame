using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class VertexBuffer : GraphicsObject, IVertexBuffer
    {
        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }

        public SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description)
            : base(graphicsDevice, new StackTrace(1))
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
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("data must not be empty.");

            this.VertexCount = data.Length;

            Buffer = SharpDX.Direct3D11.Buffer.Create(graphicsDevice.Context.Device, BindFlags.VertexBuffer, data);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();
        }
    }
}
