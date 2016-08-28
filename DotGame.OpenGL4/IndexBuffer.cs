using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DotGame.OpenGL4
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public IndexFormat Format { get; private set; }
        public int IndexCount { get; private set; }
        public ResourceUsage Usage { get; private set;}

        internal int IboID { get; private set; }

        private int sizeBytes;
        public int SizeBytes
        {
            get
            {
                return sizeBytes;
            }
            private set
            {
                int formatSize = graphicsDevice.GetSizeOf(Format);
                if (value % formatSize != 0)
                    throw new ArgumentException("Data does not match IndexFormat.", "data");
                this.IndexCount = value / formatSize;
                sizeBytes = value;
            }
        }
        
        public IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format, ResourceUsage usage, DataArray data) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if(data.IsNull)
                throw new ArgumentNullException("data.Pointer");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");
            EnumConverter.Convert(Format); //Check whether format is supported

            this.Usage = usage;
            this.Format = format;

            IboID = GL.GenBuffer();

            if (!data.IsNull)
            {
                SizeBytes = data.Size;

                if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
                {
                    graphicsDevice.BindManager.IndexBuffer = this;
                    GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(SizeBytes), data.Pointer, EnumConverter.Convert(Usage));
                }
                else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                {
                    OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(IboID, new IntPtr(SizeBytes), data.Pointer, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
                }
            }

            graphicsDevice.CheckGLError("IndexBuffer Constructor");
        }

        public IndexBuffer(GraphicsDevice graphicsDevice, IndexFormat format, ResourceUsage usage, int indexCount) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (indexCount <= 0)
                throw new ArgumentOutOfRangeException("indexCount", indexCount, "indexCount must be bigger than zero.");
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");
            EnumConverter.Convert(Format); //Check whether format is supported

            this.Usage = usage;
            this.Format = format;
            SizeBytes = indexCount * graphicsDevice.GetSizeOf(format);

            IboID = GL.GenBuffer();

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.IndexBuffer = this;
                GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(SizeBytes), IntPtr.Zero, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(IboID, new IntPtr(SizeBytes), IntPtr.Zero, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }

            graphicsDevice.CheckGLError("IndexBuffer Constructor");
        }

        internal void UpdateData(DataArray data)
        {
            if (data.IsNull)
                throw new ArgumentException("data.Pointer is null");
            if (data.Size < 0)
                throw new ArgumentException("Data must not be smaller than zero.", "data");
            if (data.Size != SizeBytes)
                throw new ArgumentException("Data does not match IndexBuffer size.", "data");

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.IndexBuffer = this;
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferSubData(IboID, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
            }
            graphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteBuffer(IboID);
        }
    }
}
