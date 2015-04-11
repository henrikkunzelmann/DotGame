using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using DotGame.Graphics;
using System.Runtime.InteropServices;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;

namespace DotGame.OpenGL4
{
    public class IndexBuffer : GraphicsObject, IIndexBuffer
    {
        public IndexFormat Format { get; private set; }
        public int IndexCount { get; private set; }
        public int SizeBytes { get; private set; }
        public BufferUsage Usage { get; private set;}

        internal int IboID { get; private set; }
 
        public IndexBuffer(GraphicsDevice graphicsDevice, BufferUsage usage, IndexFormat format) : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");
            if (graphicsDevice.IsDisposed)
                throw new ArgumentException("GraphicsDevice is disposed.", "graphicsDevice");

            this.Usage = usage;
            this.Format = format;
            EnumConverter.Convert(Format);

            IboID = GL.GenBuffer();
            graphicsDevice.CheckGLError();
        }

        internal void SetData<T>(T[] data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.Length == 0)
                throw new ArgumentException("Data must not be empty.", "data");
            
            // TODO (henrik1235) Format und SizeBytes supporten
            this.IndexCount = data.Length;
            this.SizeBytes = Marshal.SizeOf(data[0]) * data.Length;

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.IndexBuffer = this;
                GL.BufferData<T>(BufferTarget.ElementArrayBuffer, new IntPtr(this.SizeBytes), data, EnumConverter.Convert(Usage));
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                Ext.NamedBufferData<T>(IboID, new IntPtr(this.SizeBytes), data, (OpenTK.Graphics.OpenGL.ExtDirectStateAccess) EnumConverter.Convert(Usage));
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
