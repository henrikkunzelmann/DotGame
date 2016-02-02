using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class RenderContext : GraphicsObject, IRenderContext
    {
        private RenderStateInfo currentState = new RenderStateInfo();
        private VertexBuffer currentVertexBuffer;
        private IndexBuffer currentIndexBuffer;

        //Wird benutzt um ein FBO vom Pool abzurufen oder zu erstellen
        private int[] currentRenderTargets;
        private int currentDepthRenderTarget = -1;

        private TextureFormat currentDepthFormat;

        private ClearBufferMask currentClearBufferMask = ClearBufferMask.None;
        private Color clearColor;
        private float clearDepth;
        private int clearStencil;
        
        private Viewport currentViewPort;
        private Rectangle currentScissor;

        private RasterizerState currentRasterizer;
        private Color currentBlendFactor;
        private BlendState currentBlend;
        private int currentStencilReference = 0;
        private DepthStencilState currentDepthStencil;

        //BlendState
        private bool[] currentBlendingEnabled = new bool[8];

        public IRenderUpdateContext UpdateContext
        {
            get; private set;
        }

        public RenderContext(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            CreateDefaultState();
            UpdateContext = new RenderUpdateContext(graphicsDevice, this);
        }

        /// <summary>
        /// OpenGL States auf DirectX11 default Werte setzen
        /// </summary>
        private void CreateDefaultState()
        {
            IBlendState defaultBlend = graphicsDevice.Factory.CreateBlendState(new BlendStateInfo() 
            { 
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
                RenderTargets = new RTBlendInfo[]{
                    new RTBlendInfo() {
                         IsBlendEnabled = false,
                         SrcBlend = Blend.One,
                         DestBlend = Blend.Zero,
                         BlendOp = Graphics.BlendOp.Add,
                         SrcBlendAlpha = Blend.One,
                         DestBlendAlpha = Blend.Zero,
                         BlendOpAlpha = BlendOp.Add,
                         RenderTargetWriteMask = ColorWriteMaskFlags.All,
                    }
                },
            });

            IRasterizerState defaultRasterizer = graphicsDevice.Factory.CreateRasterizerState( new RasterizerStateInfo() {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                IsFrontCounterClockwise = false,
                DepthBias = 0,
                SlopeScaledDepthBias = 0.0f,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                IsScissorEnabled = true,
                IsMultisampleEnabled = false,
                IsAntialiasedLineEnabled = false,
            });

            IDepthStencilState defaultDepthStencil = graphicsDevice.Factory.CreateDepthStencilState(new DepthStencilStateInfo()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparsion = Comparison.Less,
                IsStencilEnabled = false,
                StencilReadMask = 0x0,
                StencilWriteMask = 0x0,
                FrontFace = new DepthStencilOperator() { 
                    Comparsion = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                },
                BackFace = new DepthStencilOperator()
                {
                    Comparsion = Comparison.Always,
                    DepthFailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                },
            });

            graphicsDevice.Cast<BlendState>(defaultBlend, "defaultBlend").Apply();
            graphicsDevice.Cast<RasterizerState>(defaultRasterizer, "defaultRasterizer").Apply();
            graphicsDevice.Cast<DepthStencilState>(defaultDepthStencil, "depthStencil").Apply(currentStencilReference);
        }

        public void SetRenderTargetsBackBuffer()
        {
            currentDepthRenderTarget = -1;
            currentRenderTargets = null;
        }

        public void SetRenderTargets(IRenderTarget2D depth, params IRenderTarget2D[] colorTargets)
        {
            SetRenderTargetsColor(colorTargets);
            SetRenderTargetsDepth(depth);
        }
        
        public void SetRenderTargetsColor(params IRenderTarget2D[] colorTargets)
        {
            if (colorTargets != null && colorTargets.Length > 0)
            {
                currentRenderTargets = new int[colorTargets.Length];
                for (int i = 0; i < colorTargets.Length; i++)
                {
                    if (colorTargets[i] == null)
                        continue;

                    Texture2D texture = graphicsDevice.Cast<Texture2D>(colorTargets[i], string.Format("color[{0}]", i));
                    currentRenderTargets[i] = texture.TextureID;
                }
            }
            else
                currentRenderTargets = null;
        }

        public void SetRenderTargetsDepth(IRenderTarget2D depth)
        {
            if (depth != null)
            {
                Texture2D texture = graphicsDevice.Cast<Texture2D>(depth, "depth");
                currentDepthRenderTarget = texture.TextureID;
                currentDepthFormat = texture.Format;
            }
            else
                currentDepthRenderTarget = -1;
        }

        public void Clear(Color color)
        {
            Clear(ClearOptions.ColorDepth, color, 1.0f, 0);
        }

        public void Clear(ClearOptions options, Color color, float depth, byte stencil)
        {
            if (depth < 0 || depth > 1)
                throw new ArgumentOutOfRangeException("Depth must be between 0 and 1", "depth");

            if (options.HasFlag(ClearOptions.Color))
            {
                SetClearColor(ref color);
                currentClearBufferMask |= ClearBufferMask.ColorBufferBit;
            }

            if (options.HasFlag(ClearOptions.Depth))
            {
                SetClearDepth(ref depth);
                currentClearBufferMask |= ClearBufferMask.DepthBufferBit;
            }

            if (options.HasFlag(ClearOptions.Stencil))
            {
                SetClearStencil(ref stencil);
                currentClearBufferMask |= ClearBufferMask.StencilBufferBit;
            }
        }

        private void SetClearColor(ref Color color)
        {
            if (color != clearColor)
            {
                clearColor = color;
                GL.ClearColor(color.R, color.G, color.B, color.A);
            }
        }

        private void SetClearDepth(ref float depth)
        {
            if (depth != clearDepth)
            {
                clearDepth = depth;
                GL.ClearDepth(depth);
            }
        }

        private void SetClearStencil(ref byte stencil)
        {
            if (stencil != clearStencil)
            {
                clearStencil = stencil;
                GL.ClearStencil(stencil);
            }
        }

        public void SetViewport(Viewport viewport)
        {
            if (currentViewPort != viewport)
            {
                if (viewport == null)
                    GL.Viewport(0, 0, 0, 0);
                else
                    GL.Viewport((int)viewport.X, (int)viewport.Y, (int)viewport.Width, (int)viewport.Height);

                currentViewPort = viewport;
            }
        }

        public void SetScissor(Rectangle rectangle)
        {
            if(currentScissor != rectangle)
                GL.Scissor((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);

            currentScissor = rectangle;
        }

        public void SetBlendFactor(Color blendFactor)
        {
            if(blendFactor != currentBlendFactor)
                GL.BlendColor(blendFactor.R, blendFactor.G, blendFactor.B, blendFactor.A);
            currentBlendFactor = blendFactor;
        }

        public void SetStencilReference(byte stencilReference)
        {
            currentStencilReference = stencilReference;
        }

        public void SetState(IRenderState state)
        {
            var internalState = graphicsDevice.Cast<RenderState>(state, "state");
            if (!internalState.Info.Equals(currentState))
                currentState = internalState.Info;
        }

        public void SetVertexBuffer(IVertexBuffer vertexBuffer)
        {
            var buffer = graphicsDevice.Cast<VertexBuffer>(vertexBuffer, "vertexBuffer");
            if (currentVertexBuffer != buffer)
            {
                currentVertexBuffer = buffer;
            }
        }

        public void SetIndexBuffer(IIndexBuffer indexBuffer)
        {
            var buffer = graphicsDevice.Cast<IndexBuffer>(indexBuffer, "indexBuffer");
            if (currentIndexBuffer != buffer)
            {
                currentIndexBuffer = buffer;
            }
        }

        public void SetConstantBuffer(IShader shader, string name, IConstantBuffer buffer)
        {
            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            var internalBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");
            
            graphicsDevice.BindManager.Shader = shader;

            graphicsDevice.BindManager.ConstantBuffer = buffer;
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, internalShader.GetUniformBlockBindingPoint(name), internalBuffer.UboId);
            graphicsDevice.CheckGLError();
        }

        public void SetConstantBuffer(IShader shader, IConstantBuffer buffer)
        {
            SetConstantBuffer(shader, "global", buffer);
        }

        public void SetTextureNull(IShader shader, string name)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");
            if (name == null)
                throw new ArgumentNullException("name");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture((Texture2D)null, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture2D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (shader == null)
                throw new ArgumentNullException("shader");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture2DArray texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (shader == null)
                throw new ArgumentNullException("shader");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture3D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (shader == null)
                throw new ArgumentNullException("shader");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture3DArray texture)
        {
            throw new NotSupportedException();
        }

        public void SetSampler(IShader shader, string name, ISampler sampler)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (sampler == null)
                throw new ArgumentNullException("sampler");
            if (shader == null)
                throw new ArgumentNullException("shader");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");
            
            graphicsDevice.BindManager.SetSampler(sampler, internalShader.GetTextureUnit(name));
        }

        private void ApplyRenderTarget()
        {
            graphicsDevice.BindManager.Fbo = graphicsDevice.GetFBO(currentDepthRenderTarget, currentRenderTargets);

            if (currentClearBufferMask != 0)
            {
                GL.Clear(currentClearBufferMask);
                currentClearBufferMask = 0;
            }
        }

        private void ApplyState()
        {
            if (currentState.Shader == null)
                throw new InvalidOperationException("A shader is not set to the render context.");

            var shader = graphicsDevice.Cast<Shader>(currentState.Shader, "currentState.Shader");

            graphicsDevice.BindManager.Shader = shader;

            if (currentState.Rasterizer != null && currentState.Rasterizer != currentRasterizer)
            {
                RasterizerState rasterizer = graphicsDevice.Cast<RasterizerState>(currentState.Rasterizer, "currentState.Rasterizer");

                rasterizer.Apply(currentRasterizer);
                currentRasterizer = rasterizer;
            }

            if (currentState.Blend != null && currentState.Blend != currentBlend)
            {
                BlendState blend = graphicsDevice.Cast<BlendState>(currentState.Blend, "currentState.Blend");

                blend.Apply(currentBlend);
                currentBlend = blend;
            }

            if (currentState.DepthStencil != null && currentState.DepthStencil != currentDepthStencil)
            {
                DepthStencilState depthStencil = graphicsDevice.Cast<DepthStencilState>(currentState.DepthStencil, "currentState.DepthStencil");

                depthStencil.Apply(currentDepthStencil, currentStencilReference);
                currentDepthStencil = depthStencil;
            }
            
            // Das VertexArrayObject speichert die Attribute calls eines bestimmten Shaders
            // Falls ein anderer Shader gesetzt ist oder diese Attribute gesetzt sind, müssen diese VertexAttributePointer gesetzt werden

            if (currentVertexBuffer.Shader != currentState.Shader)
            {
                if (!currentState.Shader.VertexDescription.EqualsIgnoreOrder(currentVertexBuffer.Description))
                    throw new GraphicsException("Current shader VertexDescription doesn't match the description of the current VertexBuffer");

                VertexElement[] elements = currentVertexBuffer.Description.GetElements();
                for (int i = 0; i < currentVertexBuffer.Description.ElementCount; i++)
                {
                    GL.BindAttribLocation(shader.ProgramID, i, EnumConverter.Convert(elements[i].Usage));
                }

                currentVertexBuffer.Shader = shader;
            }

            if (!graphicsDevice.OpenGLCapabilities.VertexAttribBinding)
            {
                graphicsDevice.BindManager.VertexArray = currentVertexBuffer.VaoID;

                if (currentVertexBuffer.LayoutDirty)
                {
                    if (!currentState.Shader.VertexDescription.EqualsIgnoreOrder(currentVertexBuffer.Description))
                        throw new GraphicsException("Current shader VertexDescription doesn't match the description of the current VertexBuffer");

                    graphicsDevice.BindManager.VertexBuffer = currentVertexBuffer;

                    int offset = 0;
                    VertexElement[] elements = currentVertexBuffer.Description.GetElements();
                    for (int i = 0; i < graphicsDevice.OpenGLCapabilities.MaxVertexAttribs; i++)
                    {
                        if (i < currentVertexBuffer.Description.ElementCount)
                        {
                            GL.EnableVertexAttribArray(i);

                            GL.VertexAttribPointer(i, graphicsDevice.GetComponentsOf(elements[i].Type), VertexAttribPointerType.Float, false, graphicsDevice.GetSizeOf(currentVertexBuffer.Description), offset);
                            offset += graphicsDevice.GetSizeOf(elements[i].Type);
                        }
                        else
                        {
                            GL.DisableVertexAttribArray(i);
                        }
                    }

                    currentVertexBuffer.LayoutDirty = false;
                }
            }
            else
            {
                if (!currentState.Shader.VertexDescription.EqualsIgnoreOrder(currentVertexBuffer.Description))
                    throw new GraphicsException("Current shader VertexDescription doesn't match the description of the current VertexBuffer");

                graphicsDevice.BindManager.VertexBuffer = null;
                int layout = graphicsDevice.GetLayout(currentVertexBuffer.Description, graphicsDevice.Cast<Shader>(currentState.Shader, "currentState.shader"));
                graphicsDevice.BindManager.VertexArray = layout;

                graphicsDevice.BindManager.SetVertexBuffer(0, currentVertexBuffer);
            }

            graphicsDevice.BindManager.IndexBuffer = currentIndexBuffer;

            graphicsDevice.CheckGLError("RenderContext.ApplyState");
        }

        public void Draw()
        {
            Draw(currentVertexBuffer.VertexCount, 0);
        }

        public void Draw(int vertexCount, int firstVertexLocation)
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");

            ApplyRenderTarget();
            ApplyState();
            GL.DrawArrays(EnumConverter.Convert(currentState.PrimitiveType), firstVertexLocation, vertexCount);
        }

        public void DrawIndexed()
        {
            DrawIndexed(currentIndexBuffer.IndexCount, 0, 0);
        }

        public void DrawIndexed(int indexCount, int firstIndexLocation, int firstVertexLocation)
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");
            if (currentIndexBuffer == null)
                throw new InvalidOperationException("Tried to draw without an indexbuffer set.");

            ApplyState();
            GL.DrawElements((BeginMode)EnumConverter.Convert(currentState.PrimitiveType), indexCount, EnumConverter.Convert(currentIndexBuffer.Format), firstVertexLocation * graphicsDevice.GetSizeOf(currentIndexBuffer.Format));
        }

        protected override void Dispose(bool isDisposing)
        {

        }
    }
}