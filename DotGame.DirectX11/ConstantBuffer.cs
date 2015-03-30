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

namespace DotGame.DirectX11
{
    public class ConstantBuffer : GraphicsObject, IConstantBuffer
    {
        public int Size
        {
            get { throw new NotImplementedException(); }
        }

        private Buffer handle;

        public ConstantBuffer(GraphicsDevice graphicsDevice, int size)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (size < 0)
                throw new ArgumentException("Size is smaller then 0.", "size");

            handle = new Buffer(graphicsDevice.Context.Device, new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = Size,
                StructureByteStride = 0
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (handle != null)
                handle.Dispose();
        }

        public void UpdateData<T>(T Data) where T : struct
        {
            DataBox box = graphicsDevice.Context.MapSubresource(handle, 0, MapMode.WriteDiscard, MapFlags.None);
            Utilities.Write(box.DataPointer, ref Data);
            graphicsDevice.Context.UnmapSubresource(handle, 0);
        }
    }
}
