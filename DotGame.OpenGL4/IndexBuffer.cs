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
        public IndexFormat Format
        {
            get { throw new NotImplementedException(); }
        }
        public int IndexCount { get; private set; }
        public int SizeBytes
        {
            get { throw new NotImplementedException(); }
        }

        internal int IndexBufferID { get; private set; }
 
        public IndexBuffer(GraphicsDevice graphicsDevice) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            IndexBufferID = GL.GenBuffer();
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public void SetData<T>(T[] data, IndexFormat format) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("data must not be empty.");

            // TODO (henrik1235) Format und SizeBytes supporten
            this.IndexCount = data.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferID);
            GL.BufferData<T>(BufferTarget.ElementArrayBuffer, new IntPtr(Marshal.SizeOf(data[0]) * data.Length), data, BufferUsageHint.StaticDraw);
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteBuffer(IndexBufferID);
        }
    }
}
