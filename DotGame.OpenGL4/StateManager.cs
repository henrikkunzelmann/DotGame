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
    /// </summary>
    public class StateManager
    {
        private GraphicsDevice graphicsDevice;

        private IVertexBuffer currentVertexBuffer;
        private IIndexBuffer currentIndexBuffer;
        private IConstantBuffer currentConstantBuffer;
        private IShader currentShader;
        private Fbo currentFbo;

        //Rasterizer
        private bool currentIsFrontCounterClockwise = false;
        private CullMode currentCullMode;
        private FillMode currentFillMode;
        private bool currentIsDepthClipEnabled = false;
        private bool currentMultisampleEnabled = false;
        private bool currentScissorEnabled = false;
        private bool currentIsAntialiasedLineEnable = false;
        private int currentDepthBias;
        private float currentDepthBiasClamp;
        private float currentSlopeScaledDepthBias;

        private IGraphicsObject[] currentTextures;
        private IGraphicsObject[] currentSamplers;

        private List<Fbo> fbos = new List<Fbo>();

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
                if (value == null)
                {
                    GL.BindVertexArray(0);
                }
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

        internal bool IsFrontCounterClockwise
        {
            get { return currentIsFrontCounterClockwise; }
            set
            {
                if (value != currentIsFrontCounterClockwise)
                {
                    currentIsFrontCounterClockwise = value;

                    GL.FrontFace(value ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw);
                }
            }
        }

        internal FillMode FillMode
        {
            get { return currentFillMode; }
            set
            {
                if (value != currentFillMode)
                {
                    currentFillMode = value;

                    GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(value));
                }
            }
        }

        internal CullMode CullMode
        {
            get { return currentCullMode; }
            set
            {
                if (value != currentCullMode)
                {
                    currentCullMode = value;

                    if (value != CullMode.None)
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(EnumConverter.Convert(value));
                    }
                    else
                        GL.Disable(EnableCap.CullFace);
                }
            }
        }

        internal bool IsDepthClipEnabled
        {
            get { return currentIsDepthClipEnabled; }
            set
            {
                if (value != currentIsDepthClipEnabled)
                {
                    currentIsDepthClipEnabled = value;

                    if (value)
                        GL.Enable(EnableCap.DepthTest);
                    else
                        GL.Disable(EnableCap.DepthTest);
                }
            }
        }

        internal bool IsMultisampleEnabled
        {
            get { return currentMultisampleEnabled; }
            set
            {
                if (value != currentMultisampleEnabled)
                {
                    currentMultisampleEnabled = value;

                    if (value)
                    {
                        GL.Enable(EnableCap.Multisample);
                        GL.Enable(EnableCap.PolygonSmooth);
                    }
                    else
                    {
                        GL.Disable(EnableCap.Multisample);
                        GL.Disable(EnableCap.PolygonSmooth);
                    }
                }
            }
        }

        internal bool IsScissorEnabled
        {
            get { return currentScissorEnabled; }
            set
            {
                if (value != currentScissorEnabled)
                {
                    currentScissorEnabled = value;

                    if (value)
                        GL.Enable(EnableCap.ScissorTest);
                    else
                        GL.Disable(EnableCap.ScissorTest);
                }
            }
        }

        internal bool IsAntialiasedLineEnable
        {
            get { return currentIsAntialiasedLineEnable; }
            set
            {
                if (value != currentIsAntialiasedLineEnable)
                {
                    currentIsAntialiasedLineEnable = value;

                    if (value)
                        GL.Enable(EnableCap.LineSmooth);
                    else
                        GL.Disable(EnableCap.LineSmooth);
                }
            }
        }

        internal int DepthBias 
        {
            get { return currentDepthBias; }
        }

        internal float DepthBiasClamp
        {
            get { return currentDepthBiasClamp; }
            set 
            {
                if (currentDepthBiasClamp != value)
                { 
                    currentDepthBiasClamp = value;
                }
            }
        }
        internal float SlopeScaledDepthBias
        {
            get { return currentSlopeScaledDepthBias; }
        }
        internal void SetPolygonOffset(float depthBias, float slopeScaledDepthBias)
        {
            if (depthBias != currentDepthBias || slopeScaledDepthBias != currentSlopeScaledDepthBias)
            {
                currentDepthBiasClamp = depthBias;
                currentSlopeScaledDepthBias = slopeScaledDepthBias;
                GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(FillMode));
                GL.PolygonOffset(currentDepthBias, currentSlopeScaledDepthBias);
            }
        }

        public StateManager(GraphicsDevice graphicsDevice)
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
            if (texture == null)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + unit);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                currentTextures[unit] = texture;
            }
            else
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
    }
}
