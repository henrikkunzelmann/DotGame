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

        private int currentVertexArray;
        private IVertexBuffer currentVertexBuffer;
        private IIndexBuffer currentIndexBuffer;
        private IConstantBuffer currentConstantBuffer;
        private IShader currentShader;
        private FrameBuffer currentFbo;
        
        //Texture & Sampler
        private IGraphicsObject[] currentTextures;
        private IGraphicsObject[] currentSamplers;

        //OGL 4.3
        private IVertexBuffer[] currentVertexBuffers;

        internal IVertexBuffer VertexBuffer 
        {
            get { return currentVertexBuffer; }
            set
            {
                if (value != currentVertexBuffer ) 
                {
                    if (value == null)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    }
                    else
                    {
                        VertexBuffer internalVertexBuffer = graphicsDevice.Cast<VertexBuffer>(value, "value");
                        GL.BindBuffer(BufferTarget.ArrayBuffer, internalVertexBuffer.VboID);
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

        internal int VertexArray
        {
            get { return currentVertexArray; }
            set
            {
                if (value != currentVertexArray)
                {
                    if (value <= 0)
                    {
                        GL.BindVertexArray(0);
                    }
                    else
                    {                        
                        GL.BindVertexArray(value);
                    }
                    currentVertexArray = value;
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

                        GL.BindBuffer(BufferTarget.UniformBuffer, internalConstantBuffer.UboId);
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

        internal FrameBuffer Fbo
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

            this.currentTextures = new IGraphicsObject[graphicsDevice.OpenGLCapabilities.TextureUnits];
            this.currentSamplers = new IGraphicsObject[graphicsDevice.OpenGLCapabilities.TextureUnits];
            
            if(graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
                this.currentVertexBuffers = new IVertexBuffer[graphicsDevice.OpenGLCapabilities.MaxVertexAttribBindings];
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
                    Texture3D internalTexture = graphicsDevice.Cast<Texture3D>(texture, "texture");
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
                    GL.BindTexture(TextureTarget.TextureCubeMap, 0);
                    GL.BindTexture(TextureTarget.Texture2DArray, 0);
                }
                else
                {
                    Texture2DArray internalTexture = graphicsDevice.Cast<Texture2DArray>(texture, "texture");
                    GL.ActiveTexture(TextureUnit.Texture0 + unit);

                    GL.BindTexture(internalTexture.TextureTarget, internalTexture.TextureID);
                }
                currentTextures[unit] = texture;
            }
        }

        internal void SetVertexBuffer(int bindingPoint, IVertexBuffer buffer)
        {
            if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
                throw new PlatformNotSupportedException("VertexAttribBinding is not supported");

            if (buffer != currentVertexBuffers[bindingPoint])
            {
                if (buffer == null)
                {
                    GL.BindVertexBuffer(bindingPoint, 0, (IntPtr)0, 0);
                }
                else
                {
                    if (bindingPoint >= graphicsDevice.OpenGLCapabilities.MaxVertexAttribBindings)
                        throw new PlatformNotSupportedException("bindingPoint exceeds maximum amount of VertexAttribBindings");

                    if (currentVertexBuffer == buffer)
                        currentVertexBuffer = null;

                    VertexBuffer internalVertexBuffer = graphicsDevice.Cast<VertexBuffer>(buffer, "buffer");
                    int stride = graphicsDevice.GetSizeOf(internalVertexBuffer.Description);
                    if(stride > graphicsDevice.OpenGLCapabilities.MaxVertexAttribStride)
                        throw new GraphicsException(string.Format("The vertex size exceeds the maximum of {0}.",graphicsDevice.OpenGLCapabilities.MaxVertexAttribStride));
                    GL.BindVertexBuffer(0, internalVertexBuffer.VboID, (IntPtr)0, stride);
                }
                currentVertexBuffers[bindingPoint] = buffer;
            }
        }
    }
}
