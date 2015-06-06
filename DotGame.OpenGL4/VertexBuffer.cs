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

        internal bool LayoutDirty { get; set; }

        public int SizeBytes { get; private set; }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description, BufferUsage usage)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            VboID = GL.GenBuffer();

            if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
            {
                VaoID = GL.GenVertexArray();
                LayoutDirty = true;
            }
            
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

            if (typeof(T) == typeof(IVertexType))
            {
                SizeBytes = data.Length * sizePerVertex;
                VertexCount = data.Length;
            }
            else
            {
                SizeBytes = data.Length * Marshal.SizeOf(typeof(T));
                VertexCount = SizeBytes / sizePerVertex; // TODO (henrik1235) Überprüfen ob Date-Größe (in bytes) der VertexDescription entspricht (dataSizeBytes % size == 0)
            }

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.VertexBuffer = this;
                GL.BufferData<T>(BufferTarget.ArrayBuffer, new IntPtr(SizeBytes), data, EnumConverter.Convert(Usage));
            } 
            else if(graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData<T>(VboID, new IntPtr(SizeBytes), data, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }

            graphicsDevice.CheckGLError();
        }
        internal void SetData(IntPtr data, int size)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (size <= 0)
                throw new ArgumentException("Data must not be empty.", "data");

            SizeBytes=size;

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.VertexBuffer = this;
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(SizeBytes), data, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(VboID, new IntPtr(SizeBytes), data, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }
            graphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
            {
                if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
                    GL.DeleteVertexArray(VboID);

                GL.DeleteBuffer(VaoID);
            }
        }
    }
}
