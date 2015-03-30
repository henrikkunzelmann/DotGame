using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class VertexBuffer : GraphicsObject, IVertexBuffer
    {
        internal int VertexBufferObjectID { get; private set; }
        internal int VertexArrayObjectID { get; private set; }

        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            VertexArrayObjectID = GL.GenVertexArray();
            VertexBufferObjectID = GL.GenBuffer();

            this.Description = description;
        }

        public void SetData<T>(T[] data) where T : struct, IVertexType
        {
            if(data == null) 
                throw new ArgumentNullException("data");
            if(data.Length == 0) 
                throw new ArgumentException("data must not be empty.");


            this.VertexCount = data.Length;

            int size = GraphicsDevice.GetSizeOf(data[0].Description);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObjectID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(size), data, BufferUsageHint.StaticDraw); 
        }

        internal override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            GL.DeleteVertexArray(VertexArrayObjectID);
            GL.DeleteBuffer(VertexBufferObjectID);

            base.Dispose(isDisposing);
        }
    }
}
