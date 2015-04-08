using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class Fbo : GraphicsObject
    {
        internal int FboID { get; private set; }
        internal int[] ColorAttachmentIDs { get; private set; }
        internal int DepthAttachmentID { get; private set; }

        internal Fbo(GraphicsDevice graphicsDevice, int depth, params int[] color)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            FboID = GL.GenFramebuffer();
            Attach(depth, color);
            CheckStatus();
        }

        internal Fbo(GraphicsDevice graphicsDevice, params int[] color)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            FboID = GL.GenFramebuffer();
            Attach(-1, color);
            CheckStatus();
            graphicsDevice.CheckGLError();
        }

        private void Attach(int depthAttachment, params int[] colorAttachments)
        {
            if (depthAttachment == -1 && (colorAttachments == null || colorAttachments.Length == 0))
                throw new Exception("Can't create a framebuffer object without attachments.");
            
            graphicsDevice.BindManager.Fbo = this;
            ColorAttachmentIDs = colorAttachments;
            DepthAttachmentID = depthAttachment;

            if (colorAttachments != null)
            {
                DrawBuffersEnum[] buffers = new DrawBuffersEnum[colorAttachments.Length];                

                for (int i = 0; i < colorAttachments.Length; i++)
                {
                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, colorAttachments[i], 0);
                    buffers[i] = DrawBuffersEnum.ColorAttachment0 + i;
                }

                if (colorAttachments.Length == 0)
                    GL.DrawBuffer(DrawBufferMode.None);
                else
                    GL.DrawBuffers(buffers.Length, buffers);
            }
            else            
                GL.DrawBuffer(DrawBufferMode.None);

            if (depthAttachment != -1)
            {
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthAttachment, 0);
            }
        }

        internal void CheckStatus()
        {
            graphicsDevice.BindManager.Fbo = this;
            FramebufferErrorCode error = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (error != FramebufferErrorCode.FramebufferComplete)
                throw new Exception(error.ToString());
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteFramebuffer(FboID);
        }
    }
}
