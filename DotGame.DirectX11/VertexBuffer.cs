using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using SharpDX.Direct3D11;
using ResourceUsage = DotGame.Graphics.ResourceUsage;

namespace DotGame.DirectX11
{
    public class VertexBuffer : GraphicsObject, IVertexBuffer
    {
        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }
        public ResourceUsage Usage { get; private set; }

        internal SharpDX.Direct3D11.Buffer Buffer { get; private set; }
        internal VertexBufferBinding Binding { get; private set; }


        private int sizeBytes;
        public int SizeBytes
        {
            get
            {
                return sizeBytes;
            }
            private set
            {
                int descriptionSize = graphicsDevice.GetSizeOf(Description);
                if (value % descriptionSize != 0)
                    throw new ArgumentException("Data does not match vertex description.", "data");
                this.VertexCount = value / descriptionSize;
                sizeBytes = value;
            }
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, int vertexCount, VertexDescription description, Graphics.ResourceUsage usage)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (vertexCount <= 0)
                throw new ArgumentException("vertexCount must not be smaller than/ equal to zero.", "vertexCount");
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");

            this.Description = description;
            int descriptionSize = graphicsDevice.GetSizeOf(description);
            this.SizeBytes = descriptionSize * vertexCount;
            this.Usage = usage;

            BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(Usage),
                         BindFlags.VertexBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

            this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, bufferDescription);

            Binding = new VertexBufferBinding(Buffer, descriptionSize, 0);
        }

        public VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description, Graphics.ResourceUsage usage, DataArray data)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (data.IsNull)
                throw new ArgumentNullException("data.Pointer");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");

            this.Description = description;
            int descriptionSize = graphicsDevice.GetSizeOf(description);
            this.Usage = usage;

            if (!data.IsNull)
            {
                this.SizeBytes = data.Size;

                BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(usage),
                 BindFlags.VertexBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

                this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, data.Pointer, bufferDescription);
                Binding = new VertexBufferBinding(Buffer, descriptionSize, 0);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Buffer != null && !Buffer.IsDisposed)
                Buffer.Dispose();
        }
    }
}
