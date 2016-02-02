using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using DotGame.Graphics;
using ResourceUsage = DotGame.Graphics.ResourceUsage;

namespace DotGame.DirectX11
{
    public class ConstantBuffer : GraphicsObject, IConstantBuffer
    {
        public int SizeBytes { get; private set; }
        public ResourceUsage Usage { get; private set; }

        internal Buffer Buffer { get; private set; }

        public ConstantBuffer(GraphicsDevice graphicsDevice, Graphics.ResourceUsage usage, int sizeBytes)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");
            if (sizeBytes <= 0)
                throw new ArgumentOutOfRangeException("sizeBytes", sizeBytes, "Size must be bigger than 0.");

            this.SizeBytes = sizeBytes;
            this.Usage = usage;

            BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(Usage),
                         BindFlags.ConstantBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

            this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, bufferDescription);
        }

        public ConstantBuffer(GraphicsDevice graphicsDevice, Graphics.ResourceUsage usage, DataArray data)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (data.IsNull)
                throw new ArgumentNullException("data.Pointer");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");

            this.Usage = usage;
            this.SizeBytes = data.Size;

            if (!data.IsNull)
            {
                BufferDescription bufferDescription = new BufferDescription(SizeBytes, (SharpDX.Direct3D11.ResourceUsage)EnumConverter.Convert(usage),
                 BindFlags.ConstantBuffer, EnumConverter.ConvertToAccessFlag(Usage), ResourceOptionFlags.None, 0);

                this.Buffer = new SharpDX.Direct3D11.Buffer(graphicsDevice.Device, data.Pointer, bufferDescription);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Buffer != null)
                Buffer.Dispose();
        }
    }
}
