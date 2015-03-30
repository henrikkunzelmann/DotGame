using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using DotGame.Graphics;
using System.Runtime.InteropServices;

namespace DotGame.OpenGL4
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public int IndexCount { get; private set; }

        private int iboId;
 
        public IndexBuffer(GraphicsDevice graphicsDevice) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");
            
            iboId = GL.GenBuffer();
        }

        public void SetData<T>(T[] data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("data must not be empty.");

            this.IndexCount = data.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboId);
            GL.BufferData<T>(BufferTarget.ElementArrayBuffer, new IntPtr(Marshal.SizeOf(data[0]) * data.Length), data, BufferUsageHint.StaticDraw);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteTexture(iboId);
        }
    }
}
