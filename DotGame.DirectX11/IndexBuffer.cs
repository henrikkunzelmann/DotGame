using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX.Direct3D11;
using DotGame.Graphics;

namespace DotGame.DirectX11
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public int IndexCount { get; private set; }

        internal SharpDX.DXGI.Format Format { get; private set; }
        internal SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        public IndexBuffer(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            this.graphicsDevice = graphicsDevice;
        }

        public void SetData<T>(T[] data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("data must not be empty.");

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

        protected override void Dispose(bool isDisposing)
        {
            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();
        }
    }
}
