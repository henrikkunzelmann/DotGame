using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using DotGame.Graphics;

namespace DotGame.DirectX11
{
    public class IndexBuffer : IIndexBuffer
    {
        private GraphicsDevice graphicsDevice;
        public IGraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        public int IndexCount { get; private set; }

        internal SharpDX.DXGI.Format Format { get; private set; }
        internal SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        public IndexBuffer(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            this.graphicsDevice = graphicsDevice;
        }

        public void SetData<T>(T[] data) where T : struct
        {
            this.IndexCount = data.Length;

            Type tType = typeof(T);
            if (tType == typeof(int))
                Format = SharpDX.DXGI.Format.R32_SInt;
            else if (tType == typeof(uint))
                Format = SharpDX.DXGI.Format.R32_UInt;
            else if (tType == typeof(short))
                Format = SharpDX.DXGI.Format.R16_SInt;
            else if (tType == typeof(ushort))
                Format = SharpDX.DXGI.Format.R16_UInt;
            else
                throw new NotSupportedException("Type for data not supported.");

            Buffer = SharpDX.Direct3D11.Buffer.Create(graphicsDevice.Context.Device, BindFlags.IndexBuffer, data);
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
