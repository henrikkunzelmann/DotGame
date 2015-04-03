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

        public int Size { get; internal set; }

        internal ConstantBuffer(GraphicsDevice graphicsDevice, int size)
            : base(graphicsDevice, new System.Diagnostics.StackTrace())
        {
            this.Size = size;

            UniformBufferObjectID = GL.GenBuffer();
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
