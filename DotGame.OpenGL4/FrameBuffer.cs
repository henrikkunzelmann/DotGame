using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using Ext = OpenTK.Graphics.OpenGL.GL.Ext;

namespace DotGame.OpenGL4
{
    internal class FrameBuffer : GraphicsObject
    {
        internal int FboID { get; private set; }
        internal FrameBufferDescription Description { get; private set; }

        internal FrameBuffer(GraphicsDevice graphicsDevice, FrameBufferDescription description)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            FboID = GL.GenFramebuffer();
            Attach(description);
            CheckStatus();
        }

        private void Attach(FrameBufferDescription description)
        {
            if (!description.HasAttachments)
                throw new ArgumentException("Can't create a framebuffer object without attachments.", "description");

            Description = description;

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.Fbo = this;

                if (description.ColorAttachmentIDs != null)
                {
                    DrawBuffersEnum[] buffers = new DrawBuffersEnum[description.ColorAttachmentIDs.Length];

                    for (int i = 0; i < description.ColorAttachmentIDs.Length; i++)
                    {
                        GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, description.ColorAttachmentIDs[i], 0);
                        buffers[i] = DrawBuffersEnum.ColorAttachment0 + i;
                    }

                    if (description.ColorAttachmentIDs.Length == 0)
                        GL.DrawBuffer(DrawBufferMode.None);
                    else
                        GL.DrawBuffers(buffers.Length, buffers);
                }
                else
                    GL.DrawBuffer(DrawBufferMode.None);

                if (description.DepthAttachmentID != -1)
                {
                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, description.DepthAttachmentID, 0);
                }
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
            {
                if (description.ColorAttachmentIDs != null)
                {
                    OpenTK.Graphics.OpenGL.DrawBufferMode[] buffers = new OpenTK.Graphics.OpenGL.DrawBufferMode[description.ColorAttachmentIDs.Length];

                    for (int i = 0; i < description.ColorAttachmentIDs.Length; i++)
                    {
                        Ext.NamedFramebufferTexture(FboID, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment0 + i, description.ColorAttachmentIDs[i], 0);
                        buffers[i] = OpenTK.Graphics.OpenGL.DrawBufferMode.ColorAttachment0 + i;
                    }

                    if (description.ColorAttachmentIDs.Length == 0)
                        Ext.FramebufferDrawBuffer(FboID, OpenTK.Graphics.OpenGL.DrawBufferMode.None);
                    else
                        Ext.FramebufferDrawBuffers(FboID, buffers.Length, buffers);
                }
                else
                    GL.DrawBuffer(DrawBufferMode.None);

                if (description.DepthAttachmentID != -1)
                {
                    Ext.NamedFramebufferTexture(FboID, OpenTK.Graphics.OpenGL.FramebufferAttachment.DepthAttachment, description.DepthAttachmentID, 0);
                }
            }
        }

        internal void CheckStatus()
        {
            FramebufferErrorCode error = 0;
            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.Fbo = this;
                error = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                error = (FramebufferErrorCode)Ext.CheckNamedFramebufferStatus(FboID, OpenTK.Graphics.OpenGL.FramebufferTarget.Framebuffer);

            if (error != FramebufferErrorCode.FramebufferComplete)
                throw new GraphicsException(error.ToString());
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
                GL.DeleteFramebuffer(FboID);
        }
    }
}
