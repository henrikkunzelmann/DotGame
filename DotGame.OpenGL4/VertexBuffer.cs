using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace DotGame.OpenGL4
{
    internal class VertexBuffer : GraphicsObject, IVertexBuffer
    {
        internal int VaoID { get; private set; }
        internal int VboID { get; private set; }

        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }
        public BufferUsage Usage { get; private set; }

        internal Shader Shader { get; set; }

        public int SizeBytes 
        { 
            get { throw new NotImplementedException(); } 
        }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description, BufferUsage usage)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            VboID = GL.GenVertexArray();
            VaoID = GL.GenBuffer();
            graphicsDevice.CheckGLError();

            this.Description = description;
            this.Usage = usage;
        }

        internal void SetData<T>(T[] data) where T : struct
        {
            if(data == null) 
                throw new ArgumentNullException("data");
            if(data.Length == 0) 
                throw new ArgumentException("Data must not be empty.", "data");

            int sizePerVertex = GraphicsDevice.GetSizeOf(Description);
            int size = 0;

            if (typeof(T) == typeof(IVertexType))
            {
                size = data.Length * sizePerVertex;
                VertexCount = data.Length;
            }
            else
            {
                size = data.Length * Marshal.SizeOf(typeof(T));
                VertexCount = size / sizePerVertex; // TODO (henrik1235) Überprüfen ob Date-Größe (in bytes) der VertexDescription entspricht (dataSizeBytes % size == 0)
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VaoID);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(size), data, BufferUsageHint.StaticDraw);
            graphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
            {
                GL.DeleteVertexArray(VboID);
                GL.DeleteBuffer(VaoID);
            }
        }
    }
}
