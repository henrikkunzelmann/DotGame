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
    public class IndexBuffer : IIndexBuffer
    {
        private GraphicsDevice graphicsDevice;
        public IGraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        public int IndexCount { get; private set; }

        private int iboId;
 
        public IndexBuffer(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            this.graphicsDevice = graphicsDevice;

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

        public void Dispose()
        {
            if (IsDisposed)
                return;
            if (Disposing != null)
                Disposing(this, EventArgs.Empty);

            GL.DeleteTexture(iboId);

            IsDisposed = true;
        }
    }
}
