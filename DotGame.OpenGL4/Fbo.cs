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
        internal IGraphicsObject[] ColorAttachments { get; private set; }
        internal IGraphicsObject DepthAttachment { get; private set; }

        private Fbo(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            FboID = GL.GenFramebuffer();
        }

        /// <summary>
        /// Creates a FrameBufferObject with ITexture2D attachments. Should only be called by IGraphicsFactory
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice</param>
        /// <param name="color">Textures to attach to color attachment slots</param>
        /// <returns></returns>
        internal static Fbo CreateWithITexture2D(GraphicsDevice graphicsDevice, params ITexture2D[] color)
        {
            Fbo fbo = new Fbo(graphicsDevice);
            fbo.Attach(null, color);
            fbo.CheckStatus();
            return fbo;
        }

        /// <summary>
        /// Creates a FrameBufferObject with ITexture2D attachments. Should only be called by IGraphicsFactory
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice</param>
        /// <param name="depth">Texture to attach to the depth attachment slots</param>
        /// <param name="color">Textures to attach to color attachment slots</param>
        /// <returns></returns>
        internal static Fbo CreateWithITexture2D(GraphicsDevice graphicsDevice, ITexture2D depth, params ITexture2D[] color)
        {
            Fbo fbo = new Fbo(graphicsDevice);
            fbo.Attach(depth, color);
            fbo.CheckStatus();
            return fbo;
        }

        private void Attach(ITexture2D depthAttachment, params ITexture2D[] colorAttachments)
        {
            if (depthAttachment == null && (colorAttachments == null || colorAttachments.Length == 0))
                throw new Exception("Can't create a framebuffer object without attachments.");
            
            graphicsDevice.StateManager.Fbo = this;

            if (colorAttachments != null)
            {
                for (int i = 0; i < colorAttachments.Length; i++)
                {
                    Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(colorAttachments[i], string.Format("attachments[{0}]", i));

                    GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, internalTexture.TextureID, 0);
                }
            }

            if (depthAttachment != null)
            {
                Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(depthAttachment, "attachment");
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, internalTexture.TextureID, 0);
            }
        }

        internal void CheckStatus()
        {
            graphicsDevice.StateManager.Fbo = this;
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
