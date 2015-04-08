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

        private RasterizerState currentRasterizer;
        private BlendState currentBlend;
        private IDepthStencilState currentDepthStencil;

        //BlendState
        private bool[] currentBlendingEnabled = new bool[8];

        public RenderContext(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {

        }

        public void Update<T>(IConstantBuffer buffer, T data) where T : struct
        {
            var internalBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");
            if (internalBuffer.Size < 0)
                internalBuffer.Size = Marshal.SizeOf(data);
            else if (Marshal.SizeOf(data) != internalBuffer.Size)
                throw new ArgumentException("Data does not match ConstantBuffer size.", "data");

            graphicsDevice.BindManager.ConstantBuffer = buffer;
            // TODO (Robin) BufferUsageHint
            GL.BufferData<T>(BufferTarget.UniformBuffer, new IntPtr(buffer.Size), ref data, BufferUsageHint.DynamicDraw);
            graphicsDevice.CheckGLError();
        }

        public void Update<T>(ITexture2D texture, T[] data) where T : struct
        {
            Update(texture, 0, data);
        }

        public void Update<T>(ITexture2D texture, int mipLevel, T[] data) where T : struct
        {
            graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (mipLevel < 0 || mipLevel >= texture.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            graphicsDevice.BindManager.SetTexture(texture, 0);

            GCHandle arrayHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr ptr = arrayHandle.AddrOfPinnedObject();

            if (!TextureFormatHelper.IsCompressed(texture.Format))
            {
                Tuple<OpenTK.Graphics.OpenGL4.PixelFormat, PixelType> tuple = EnumConverter.ConvertPixelDataFormat(texture.Format);
                GL.TexImage2D(TextureTarget.Texture2D, mipLevel, EnumConverter.Convert(texture.Format), texture.Width, texture.Height, 0, tuple.Item1, tuple.Item2, ptr);
            }
            else
                GL.CompressedTexImage2D(TextureTarget.Texture2D, mipLevel, EnumConverter.Convert(texture.Format), texture.Width, texture.Height, 0, Marshal.SizeOf(typeof(T)) * data.Length, ptr);

            arrayHandle.Free();
        }

        public void Update<T>(ITexture2DArray textureArray, int arrayIndex, T[] data) where T : struct
        {
            Update(textureArray, arrayIndex, 0, data);
        }

        public void Update<T>(ITexture2DArray textureArray, int arrayIndex, int mipLevel, T[] data) where T : struct
        {
            if (textureArray == null)
                throw new ArgumentNullException("texture");
            if (arrayIndex < 0 || arrayIndex >= textureArray.ArraySize)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (mipLevel < 0 || mipLevel >= textureArray.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            throw new NotImplementedException();
        }

        public void GenerateMips(ITexture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (texture.IsDisposed)
                throw new ObjectDisposedException("texture");

            graphicsDevice.BindManager.SetTexture(texture, 0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void GenerateMips(ITexture2DArray textureArray)
        {
            if (textureArray == null)
                throw new ArgumentNullException("textureArray");

            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void SetBlendFactor(Color blendFactor)
        {
            throw new NotImplementedException();
        }

        public void SetStencilReference(byte stencilReference)
        {
            throw new NotImplementedException();
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

            // TODO (Robin) Durch GraphicsDevice Methode ersetzen
            graphicsDevice.BindManager.Shader = shader;

            graphicsDevice.BindManager.ConstantBuffer = buffer;
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, internalShader.GetUniformBlockBindingPoint(name), internalBuffer.UniformBufferObjectID);
            graphicsDevice.CheckGLError();
        }

        public void SetConstantBuffer(IShader shader, IConstantBuffer buffer)
        {
            SetConstantBuffer(shader, "global", buffer);
        }

        private void SetTexture(IShader shader, string name, IGraphicsObject texture, TextureTarget target)
        {
            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name), target);
        }

        public void SetTextureNull(IShader shader, string name)
        {
            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");

            graphicsDevice.BindManager.SetTexture((Texture2D)null, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture2D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture2DArray texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture3D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");

            graphicsDevice.BindManager.SetTexture(texture, internalShader.GetTextureUnit(name));
        }

        public void SetTexture(IShader shader, string name, ITexture3DArray texture)
        {
            throw new NotSupportedException();
        }

        public void SetSampler(IShader shader, string name, ISampler sampler)
        {
            var internalShader = graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");

            var internalSampler = graphicsDevice.Cast<Sampler>(sampler, "sampler");
            GL.BindSampler(internalShader.GetTextureUnit(name), internalSampler.SamplerID);
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

                depthStencil.Apply(currentDepthStencil, 0);
                currentDepthStencil = depthStencil;
            }

            graphicsDevice.BindManager.VertexBuffer = currentVertexBuffer;

            // Das VertexArrayObject speichert die Attribute calls eines bestimmten Shaders
            // Falls ein anderer Shader gesetzt ist oder diese Attribute gesetzt sind, müssen diese VertexAttributePointer gesetzt werden
            Shader internalShader = graphicsDevice.Cast<Shader>(currentState.Shader, "currentState.Shader");
            if (currentVertexBuffer.Shader != currentState.Shader)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, currentVertexBuffer.VaoID);

                int offset = 0;
                VertexElement[] elements = currentVertexBuffer.Description.GetElements();
                for (int i = 0; i < currentVertexBuffer.Description.ElementCount; i++)
                {
                    GL.EnableVertexAttribArray(i);
                    GL.BindAttribLocation(internalShader.ProgramID, i, EnumConverter.Convert(elements[i].Usage));

                    GL.VertexAttribPointer(i, graphicsDevice.GetComponentsOf(elements[i].Type), VertexAttribPointerType.Float, false, graphicsDevice.GetSizeOf(currentVertexBuffer.Description), offset);
                    offset += graphicsDevice.GetSizeOf(elements[i].Type);
                }
                currentVertexBuffer.Shader = internalShader;
            }

            graphicsDevice.BindManager.IndexBuffer = currentIndexBuffer;

            graphicsDevice.CheckGLError();
        }

        public void Draw()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");

            ApplyRenderTarget();
            ApplyState();
            GL.DrawArrays(EnumConverter.Convert(currentState.PrimitiveType), 0, currentVertexBuffer.VertexCount);
        }

        public void DrawIndexed()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");
            if (currentIndexBuffer == null)
                throw new InvalidOperationException("Tried to draw without an indexbuffer set.");

            ApplyState();
            GL.DrawElements((BeginMode)EnumConverter.Convert(currentState.PrimitiveType), currentIndexBuffer.IndexCount, EnumConverter.Convert(currentIndexBuffer.Format), 0);
        }

        protected override void Dispose(bool isDisposing)
        {

        }
    }
}