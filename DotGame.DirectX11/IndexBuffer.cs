using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX.Direct3D11;
using DotGame.Graphics;
using SharpDX.DXGI;
using ResourceUsage = DotGame.Graphics.ResourceUsage;

namespace DotGame.DirectX11
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public IndexFormat Format { get; private set; }
        public int IndexCount { get; private set; }
        public ResourceUsage Usage { get; private set; }

        internal SharpDX.DXGI.Format IndexFormat { get { return EnumConverter.Convert(Format); } }
        internal SharpDX.Direct3D11.Buffer Buffer { get; private set; }

        private int sizeBytes;
        public int SizeBytes
        {
            get
            {
                return sizeBytes;
            }
            private set
            {
                int formatSize = graphicsDevice.GetSizeOf(Format);
                if (value % formatSize != 0)
                    throw new ArgumentException("Data does not match IndexFormat.", "data");
                this.IndexCount = value / formatSize;
                sizeBytes = value;
            }
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format, Graphics.ResourceUsage usage, DataArray data)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (data.IsNull)
                throw new ArgumentException("data.Pointer is null");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");

            this.Format = format;
            this.SizeBytes = data.Size;
            this.Usage = usage;

            if (!data.IsNull)
            {
                this.SizeBytes = data.Size;

                BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(usage),
                 BindFlags.IndexBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

                this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, data.Pointer, bufferDescription);
            }
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format, Graphics.ResourceUsage usage, int indexCount)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (indexCount <= 0)
                throw new ArgumentOutOfRangeException("indexCount", indexCount, "indexCount must be bigger than zero.");
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");

            this.Format = format;
            this.SizeBytes = indexCount * graphicsDevice.GetSizeOf(format);
            this.Usage = usage;
            
            BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(Usage),
             BindFlags.IndexBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

            this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, bufferDescription);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();
        }
    }
}
