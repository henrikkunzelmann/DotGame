using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX.Direct3D11;
using DotGame.Graphics;
using SharpDX.DXGI;

namespace DotGame.DirectX11
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public IndexFormat Format { get; private set; }
        public int IndexCount { get; private set; }
        public int SizeBytes { get; private set; }

        internal SharpDX.DXGI.Format IndexFormat { get; private set; }
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

        internal void SetData<T>(T[] data, IndexFormat format) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");

            this.Format = format;
            this.IndexFormat = EnumConverter.Convert(format);

            int formatSize = FormatHelper.SizeOfInBytes(IndexFormat);
            this.SizeBytes = SharpDX.Utilities.SizeOf(data);
            if (this.SizeBytes % formatSize == 0)
                throw new ArgumentException("Data does not match index format.", "data");

            this.IndexCount = this.SizeBytes / formatSize;
            Buffer = SharpDX.Direct3D11.Buffer.Create(graphicsDevice.Device, BindFlags.IndexBuffer, data);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();
        }
    }
}
