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
        public BufferUsage Usage { get; private set; }

        internal SharpDX.DXGI.Format IndexFormat { get; private set; }
        internal SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        public IndexBuffer(GraphicsDevice graphicsDevice, int indexCount, IndexFormat format, BufferUsage usage)
            : this(graphicsDevice, format, usage)
        {
            this.IndexCount = indexCount;
            this.SizeBytes = graphicsDevice.GetSizeOf(format) * indexCount;

            this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, SizeBytes, 
                EnumConverter.Convert(usage), 
                BindFlags.IndexBuffer, Usage == BufferUsage.Static ? CpuAccessFlags.None : CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format, BufferUsage usage)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.graphicsDevice = graphicsDevice;
            this.Format = format;
            this.IndexFormat = EnumConverter.Convert(format);
            this.Usage = usage;
        }

        internal void SetData<T>(T[] data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");

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
