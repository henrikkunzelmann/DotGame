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
        public IndexFormat Format { get; private set; }
        public int IndexCount { get; private set; }
        public int SizeBytes { get; private set; }

        internal int IboID { get; private set; }
 
        public IndexBuffer(GraphicsDevice graphicsDevice) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            IboID = GL.GenBuffer();
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        internal void SetData<T>(T[] data, IndexFormat format) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");

            EnumConverter.Convert(format);

            // TODO (henrik1235) Format und SizeBytes supporten
            this.Format = format;
            this.IndexCount = data.Length;
            this.SizeBytes = Marshal.SizeOf(data[0]) * data.Length;

            graphicsDevice.StateManager.IndexBuffer = this;
            GL.BufferData<T>(BufferTarget.ElementArrayBuffer, new IntPtr(this.SizeBytes), data, BufferUsageHint.StaticDraw);
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteBuffer(IboID);
        }
    }
}
