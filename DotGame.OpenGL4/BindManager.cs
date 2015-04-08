using OpenTK.Graphics.OpenGL4;
using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenGL4
{
    /// <summary>
    /// Changes OpenGL states and prevents redundant changes.
    /// Required because in order to change states of OpenGL resources they first need to be bound.
    /// </summary>
    public class BindManager
    {
        private GraphicsDevice graphicsDevice;

        private IVertexBuffer currentVertexBuffer;
        private IIndexBuffer currentIndexBuffer;
        private IConstantBuffer currentConstantBuffer;
        private IShader currentShader;
        private Fbo currentFbo;
        
        //Texture & Sampler
        private IGraphicsObject[] currentTextures;
        private IGraphicsObject[] currentSamplers;

        internal IVertexBuffer VertexBuffer 
        {
            get { return currentVertexBuffer; }
            set
            {
                if (value != currentVertexBuffer ) 
                {
                    if (value == null)
                    {
                        GL.BindVertexArray(0);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    }
                    else
                    {
                        VertexBuffer internalVertexBuffer = graphicsDevice.Cast<VertexBuffer>(value, "value");
                        GL.BindVertexArray(internalVertexBuffer.VboID);
                    }
                    currentVertexBuffer = value;
                }
            } 
        }

        internal IIndexBuffer IndexBuffer 
        { 
            get { return currentIndexBuffer; } 
            set
            {
                if (value != currentIndexBuffer)
                {
                    if (value == null)
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    else
                    {

                        IndexBuffer internalVertexBuffer = graphicsDevice.Cast<IndexBuffer>(value, "value");

                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, internalVertexBuffer.IboID);
                    }
                    currentIndexBuffer = value;
                }
                    
            } 
        }

        internal IConstantBuffer ConstantBuffer
        {
            get { return currentConstantBuffer; }
            set
            {
                if (value != currentConstantBuffer)
                {
                    if (value == null)
                        GL.BindBuffer(BufferTarget.UniformBuffer, 0);
                    else
                    {
                        ConstantBuffer internalConstantBuffer = graphicsDevice.Cast<ConstantBuffer>(value, "value");

                        GL.BindBuffer(BufferTarget.UniformBuffer, internalConstantBuffer.UniformBufferObjectID);
                    }
                    currentConstantBuffer = value;
                }

            }
        }

        internal IShader Shader 
        { 
            get { return currentShader; }
            set 
            {
                if (value != currentShader)
                {

                    if (value == null)
                        GL.UseProgram(0);
                    else
                    {
                        Shader internalShader = graphicsDevice.Cast<Shader>(value, "value");
                        GL.UseProgram(internalShader.ProgramID);
                    }

                    currentShader = value;
                }
            }
        }

        internal Fbo Fbo
        {
            get { return currentFbo; }
            set
            {
                if (currentFbo != value)
                {
                    if (value == null)
                        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                    else
                        GL.BindFramebuffer(FramebufferTarget.Framebuffer, value.FboID);

                    currentFbo = value;
                }
            }
        }

        public BindManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.currentTextures = new IGraphicsObject[graphicsDevice.TextureUnits];
            this.currentSamplers = new IGraphicsObject[graphicsDevice.TextureUnits];
        }

        internal void SetSampler(ISampler sampler, int unit)
        {
            if (sampler == null)
            {
                GL.BindSampler(unit, 0);
                currentSamplers[unit] = sampler;
            }
            else
            {
                if (currentSamplers[unit] != sampler)
                {
                    if (sampler == null)
                    {
                        GL.BindSampler(unit, 0);
                    }
                    else
                    {
                        Sampler internalSampler = graphicsDevice.Cast<Sampler>(sampler, "sampler");
                        GL.BindSampler(unit, internalSampler.SamplerID);
                    }
                    currentSamplers[unit] = sampler;
                }
            }
        }

        internal void SetTexture(ITexture2D texture, int unit)
        {
            if (currentTextures[unit] != texture)
            {
                if (texture == null)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                }
                else
                {
                    Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture2D, internalTexture.TextureID);
                }
                currentTextures[unit] = texture;
            }
        }

        internal void SetTexture(ITexture3D texture, int unit)
        {
            if (currentTextures[unit] != texture)
            {
                if (texture == null)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture3D, 0);
                }
                else
                {
                    Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture3D, internalTexture.TextureID);
                }
                currentTextures[unit] = texture;
            }
        }

        internal void SetTexture(ITexture2DArray texture, int unit)
        {
            if (currentTextures[unit] != texture)
            {
                if (texture == null)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture2DArray, 0);
                }
                else
                {
                    Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture2DArray, internalTexture.TextureID);
                }
                currentTextures[unit] = texture;
            }
        }

        internal void SetTexture(IGraphicsObject texture, int unit, TextureTarget target)
        {
            if (currentTextures[unit] != texture)
            {
                if (texture == null)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(target, 0);
                }
                else
                {
                    Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);
                    GL.BindTexture(TextureTarget.Texture2DArray, internalTexture.TextureID);
                }
                currentTextures[unit] = texture;
            }
        }
    }
}
