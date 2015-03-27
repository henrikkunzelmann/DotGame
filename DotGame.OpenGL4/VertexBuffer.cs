using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class VertexBuffer : IVertexBuffer
    {
        private GraphicsDevice graphicsDevice;
        public IGraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        private int vboId;
        private int vaoId;

        public VertexDescription Description { get; private set; }
        public int VertexCount { get; private set; }

        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description)
        {
            vaoId = GL.GenVertexArray();
            vboId = GL.GenBuffer();

            this.graphicsDevice = graphicsDevice;
            this.Description = description;
        }

        public void SetData<T>(T[] data) where T : struct, IVertexType
        {
            if(data == null) 
                throw new ArgumentNullException("data");
            if(data.Length == 0) 
                throw new ArgumentException("data must not be empty.");


            this.VertexCount = data.Length;

            int size = ((IVertexType)data[0]).Description.Size;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(size), data, BufferUsageHint.StaticDraw); 
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;
            if (Disposing != null)
                Disposing(this, EventArgs.Empty);
                        
            GL.DeleteVertexArray(vaoId);
            GL.DeleteBuffer(vboId);

            IsDisposed = true;
        }
    }
}
