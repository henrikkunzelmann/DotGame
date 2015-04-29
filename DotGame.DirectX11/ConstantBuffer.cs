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
        public int SizeBytes { get; private set; }
        public BufferUsage Usage { get; private set; }

        internal Buffer Handle { get; private set; }

        public ConstantBuffer(GraphicsDevice graphicsDevice, int sizeBytes, BufferUsage usage)
            : this(graphicsDevice, usage)
        {
            if (sizeBytes < 0)
                throw new ArgumentException("Size must be bigger or equal to 0.", "sizeBytes");
            this.SizeBytes = sizeBytes;

            Handle = new Buffer(graphicsDevice.Device, sizeBytes, EnumConverter.Convert(usage), BindFlags.ConstantBuffer, Usage == BufferUsage.Static ? CpuAccessFlags.None : CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
        }
        
        public ConstantBuffer(GraphicsDevice graphicsDevice, BufferUsage usage)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Usage = usage;
        }

        internal void SetData<T>(T data) where T : struct
        {
            Handle = Buffer.Create(graphicsDevice.Device, BindFlags.ConstantBuffer, ref data, 0, EnumConverter.Convert(Usage), Usage == BufferUsage.Static ? CpuAccessFlags.None : CpuAccessFlags.Write);
        }

        protected override void Dispose(bool disposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
