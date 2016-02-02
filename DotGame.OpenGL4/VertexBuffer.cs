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
        public ResourceUsage Usage { get; private set; }

        internal Shader Shader { get; set; }

        internal bool LayoutDirty { get; set; }

        private int sizeBytes;
        public int SizeBytes
        {
            get
            {
                return sizeBytes;
            }
            private set
            {
                int descriptionSize = graphicsDevice.GetSizeOf(Description);
                if (value % descriptionSize != 0)
                    throw new ArgumentException("Data does not match vertex description.", "data");
                this.VertexCount = value / descriptionSize;
                sizeBytes = value;
            }
        }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description, ResourceUsage usage, DataArray data)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (data.IsNull)
                throw new ArgumentNullException("data.Pointer");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");

            this.Description = description;
            this.Usage = usage;

            VboID = GL.GenBuffer();

            if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
            {
                VaoID = GL.GenVertexArray();
                LayoutDirty = true;
            }            
            
            if (!data.IsNull)
            {
                SizeBytes = data.Size;

                if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
                {
                    graphicsDevice.BindManager.VertexBuffer = this;
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(SizeBytes), data.Pointer, EnumConverter.Convert(Usage));
                }
                else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                {
                    OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(VboID, new IntPtr(SizeBytes), data.Pointer, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
                }
            }

            graphicsDevice.CheckGLError();
        }

        internal VertexBuffer(GraphicsDevice graphicsDevice, VertexDescription description, ResourceUsage usage, int vertexCount)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (vertexCount <= 0)
                throw new ArgumentException("vertexCount must not be smaller than/ equal to zero.", "vertexCount");
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");

            this.Description = description;
            this.Usage = usage;

            VboID = GL.GenBuffer();

            if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
            {
                VaoID = GL.GenVertexArray();
                LayoutDirty = true;
            }

            SizeBytes = vertexCount * graphicsDevice.GetSizeOf(description);

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.VertexBuffer = this;
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(SizeBytes), IntPtr.Zero, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(VboID, new IntPtr(SizeBytes), IntPtr.Zero, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }

            graphicsDevice.CheckGLError();
        }

        internal void UpdateData(DataArray data)
        {
            if (data.IsNull)
                throw new ArgumentException("data.Pointer is null");
            if (data.Size < 0)
                throw new ArgumentException("Data must not be smaller than zero.", "data");
            if (data.Size != SizeBytes)
                throw new ArgumentException("Data does not match VertexBuffer size.", "data");

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.VertexBuffer = this;
                GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferSubData(VboID, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
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
