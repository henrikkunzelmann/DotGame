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
        public int Size { get; private set; }

        internal Buffer Handle { get; private set; }

        public ConstantBuffer(GraphicsDevice graphicsDevice, int size)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (size < 0)
                throw new ArgumentException("Size is smaller then 0.", "size");
            this.Size = size;

            Handle = new Buffer(graphicsDevice.Device, size, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
