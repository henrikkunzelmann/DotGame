using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;

namespace DotGame.OpenGL4
{
    internal class ConstantBuffer : GraphicsObject, IConstantBuffer
    {
        internal int UboId { get; private set; }

        public int Size { get; internal set; }
        public BufferUsage Usage { get; private set; }

        internal ConstantBuffer(GraphicsDevice graphicsDevice, int sizeBytes, BufferUsage usage)
            : base(graphicsDevice, new System.Diagnostics.StackTrace())
        {
            if (sizeBytes < 0)
                throw new ArgumentException("Size must be bigger or equal to 0.", "sizeBytes");
            this.Size = sizeBytes;
            this.Usage = usage;

            UboId = GL.GenBuffer();
            graphicsDevice.CheckGLError();
        }

        internal void SetData<T>(T data) where T : struct
        {
            // TODO (henrik1235) Format und SizeBytes supporten
            if (this.Size == Marshal.SizeOf(data))
                throw new ArgumentException("data size exceeds constant buffer size");

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.ConstantBuffer = this;
                GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(this.Size), ref data, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.NamedBufferData<T>(UboId, new IntPtr(this.Size), ref data, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
            }
            graphicsDevice.CheckGLError();
        }

        internal void SetData<T>(T[] data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");
            
            // TODO (henrik1235) Format und SizeBytes supporten
            this.Size = Marshal.SizeOf(data[0]) * data.Length;

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.ConstantBuffer = this;
                GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(this.Size), data, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.NamedBufferData<T>(UboId, new IntPtr(this.Size), data, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess)EnumConverter.Convert(Usage));
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
