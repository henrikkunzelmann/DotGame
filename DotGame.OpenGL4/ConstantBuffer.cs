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
    internal class ConstantBuffer : GraphicsObject, IConstantBuffer
    {
        internal int UniformBufferObjectID { get; private set; }

        public int Size { get; private set; }

        internal ConstantBuffer(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new System.Diagnostics.StackTrace())
        {
            UniformBufferObjectID = GL.GenBuffer();
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public void UpdateData<T>(T data) where T : struct
        {
            if (Size == 0)
            {
                Size = Marshal.SizeOf(data);
            }

            graphicsDevice.StateMachine.ConstantBuffer = this;
            // TODO (Robin) BufferUsageHint
            GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(Size), ref data, BufferUsageHint.DynamicDraw);
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteBuffer(UniformBufferObjectID);
        }
    }
}
