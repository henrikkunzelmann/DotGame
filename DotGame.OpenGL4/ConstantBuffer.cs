using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DotGame.OpenGL4
{
    internal class ConstantBuffer : GraphicsObject, IConstantBuffer
    {
        internal int UboId { get; private set; }

        public int SizeBytes { get; internal set; }
        public ResourceUsage Usage { get; private set; }
        
        internal ConstantBuffer(GraphicsDevice graphicsDevice, ResourceUsage usage, DataArray data)
            : base(graphicsDevice, new System.Diagnostics.StackTrace())
        {
            if (data.IsNull)
                throw new ArgumentNullException("data.Pointer");
            if (data.Size <= 0)
                throw new ArgumentOutOfRangeException("data.Size", data.Size, "Size must be bigger than 0.");
            if (data.Size > graphicsDevice.OpenGLCapabilities.MaxUniformBlockSize)
                throw new PlatformNotSupportedException(string.Format("data.Size {0} is too big. Supported maximum is {1}", data.Size, graphicsDevice.OpenGLCapabilities.MaxUniformBlockSize));

            this.Usage = usage;

            UboId = GL.GenBuffer();
            graphicsDevice.CheckGLError();

            if (!data.IsNull)
            {
                this.SizeBytes = data.Size;

                if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
                {
                    graphicsDevice.BindManager.ConstantBuffer = this;
                    GL.BufferData(BufferTarget.UniformBuffer, new IntPtr(SizeBytes), data.Pointer, EnumConverter.Convert(Usage));
                }
                else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                {
                    OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(UboId, new IntPtr(SizeBytes), data.Pointer, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
                }
            }

            graphicsDevice.CheckGLError("Constant Buffer Constructor");
        }

        internal ConstantBuffer(GraphicsDevice graphicsDevice, int sizeBytes, ResourceUsage usage)
            : base(graphicsDevice, new System.Diagnostics.StackTrace())
        {
            if (usage == ResourceUsage.Immutable)
                throw new ArgumentException("data", "Immutable buffers must be initialized with data.");
            if (sizeBytes <= 0)
                throw new ArgumentOutOfRangeException("sizeBytes", sizeBytes, "Size must be bigger than 0.");

            this.SizeBytes = sizeBytes;
            this.Usage = usage;

            UboId = GL.GenBuffer();

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.ConstantBuffer = this;
                GL.BufferData(BufferTarget.UniformBuffer, new IntPtr(SizeBytes), IntPtr.Zero, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferData(UboId, new IntPtr(SizeBytes), IntPtr.Zero, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }

            graphicsDevice.CheckGLError("Constant Buffer Constructor");
        }

        internal void UpdateData(DataArray data)
        {
            if (data.IsNull)
                throw new ArgumentException("data.Pointer is null");
            if (data.Size < 0)
                throw new ArgumentException("Data must not be smaller than zero.", "data");
            if (data.Size != SizeBytes)
                throw new ArgumentException("Data does not match ConstantBuffer size.", "data");

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.ConstantBuffer = this;
                GL.BufferSubData(BufferTarget.UniformBuffer, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                OpenTK.Graphics.OpenGL.GL.Ext.NamedBufferSubData(UboId, new IntPtr(0), new IntPtr(SizeBytes), data.Pointer);
            }
            graphicsDevice.CheckGLError();
        }        

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteBuffer(UboId);
        }
    }
}
