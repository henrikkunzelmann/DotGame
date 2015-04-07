using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace DotGame.DirectX11
{
    public class RenderContext : GraphicsObject, IRenderContext
    {
        private DeviceContext context;

        private RenderStateInfo currentState = new RenderStateInfo();
        private VertexBuffer currentVertexBuffer;
        private IndexBuffer currentIndexBuffer;

        private bool stateDirty;
        private bool vertexBufferDirty;
        private bool indexBufferDirty;

        private RenderTargetView[] currentColorTargets;
        private DepthStencilView currentDepthTarget;

        private IRasterizerState currentRasterizer;
        private PrimitiveType primitiveType;
        private ShaderStageCache vertexCache;
        private ShaderStageCache pixelCache;


        public RenderContext(GraphicsDevice graphicsDevice, DeviceContext context)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (context == null)
                throw new ArgumentNullException("context");
            this.context = context;

            vertexCache = new ShaderStageCache(null);
            pixelCache = new ShaderStageCache(null);
        }

        protected override void Dispose(bool isDisposing)
        {
        }

        public void Update<T>(IConstantBuffer buffer, T data) where T : struct
        {
            var dxBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");

            if (SharpDX.Utilities.SizeOf<T>() != buffer.Size)
                throw new ArgumentException("Data does not match ConstantBuffer size.", "data");

            //context.UpdateSubresource(ref data, dxBuffer.Handle); TODO (henrik1235) Rausfinden ob es einen Performance-Unterschied zwischen UpdateSubresource und Map/Unmap gibt
            SharpDX.DataBox box = context.MapSubresource(dxBuffer.Handle, 0, MapMode.WriteDiscard, MapFlags.None);
            SharpDX.Utilities.Write(box.DataPointer, ref data);
            context.UnmapSubresource(dxBuffer.Handle, 0);
        }

        public void Update<T>(ITexture2D texture, T[] data) where T : struct
        {
            Update(texture, 0, data);
        }

        public void Update<T>(ITexture2D texture, int mipLevel, T[] data) where T : struct
        {
            if (mipLevel < 0 || mipLevel >= texture.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            var dxTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            context.UpdateSubresource<T>(data, dxTexture.Handle, Resource.CalculateSubResourceIndex(mipLevel, 0, texture.MipLevels), texture.Width * graphicsDevice.GetSizeOf(texture.Format), texture.Width * texture.Height * graphicsDevice.GetSizeOf(texture.Format));
        }

        public void Update<T>(ITexture2DArray textureArray, int arrayIndex, T[] data) where T : struct
        {
            Update(textureArray, arrayIndex, 0, data);
        }

        public void Update<T>(ITexture2DArray textureArray, int arrayIndex, int mipLevel, T[] data) where T : struct
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(textureArray, "texture");
            if (arrayIndex < 0 || arrayIndex >= textureArray.ArraySize)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (mipLevel < 0 || mipLevel >= textureArray.MipLevels)
                throw new ArgumentOutOfRangeException("mipLevel");

            context.UpdateSubresource<T>(data, dxTexture.Handle, Resource.CalculateSubResourceIndex(mipLevel, arrayIndex, textureArray.MipLevels), textureArray.Width * graphicsDevice.GetSizeOf(textureArray.Format), textureArray.Width * textureArray.Height * graphicsDevice.GetSizeOf(textureArray.Format));
        }

        public void GenerateMips(ITexture2D texture)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            if (!dxTexture.Handle.Description.OptionFlags.HasFlag(SharpDX.Direct3D11.ResourceOptionFlags.GenerateMipMaps))
                throw new ArgumentException("Texture does not have the GenerateMipMaps flag.", "texture");
            context.GenerateMips(dxTexture.ResourceView);
        }

        public void GenerateMips(ITexture2DArray textureArray)
        {
            var dxTexture = graphicsDevice.Cast<Texture2D>(textureArray, "textureArray");
            if (!dxTexture.Handle.Description.OptionFlags.HasFlag(SharpDX.Direct3D11.ResourceOptionFlags.GenerateMipMaps))
                throw new ArgumentException("TextureArray does not have the GenerateMipMaps flag.", "textureArray");

            context.GenerateMips(dxTexture.ResourceView);
        }

        public void Clear(Color color)
        {
            if (currentColorTargets == null)
                throw new InvalidOperationException("No render target set for color.");

            var clearColor = new SharpDX.Color4(color.R, color.G, color.B, color.A);
            for (int i = 0; i < currentColorTargets.Length; i++)
                context.ClearRenderTargetView(currentColorTargets[i], clearColor);
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, byte stencil)
        {
            if (clearOptions.HasFlag(ClearOptions.Color))
                Clear(color);

            if (depth < 0 || depth > 1)
                throw new ArgumentOutOfRangeException("Depth must be between 0 and 1", "depth");

            DepthStencilClearFlags clearFlags = 0;
            if (clearOptions.HasFlag(ClearOptions.Depth))
                clearFlags |= DepthStencilClearFlags.Depth;
            if (clearOptions.HasFlag(ClearOptions.Stencil))
                clearFlags |= DepthStencilClearFlags.Stencil;

            if (clearFlags != 0)
            {
                if (currentDepthTarget == null)
                    throw new InvalidOperationException("No render target set for depth.");
                context.ClearDepthStencilView(currentDepthTarget, clearFlags, depth, stencil);
            }
        }

        public void SetRenderTargetBackBuffer()
        {
            SetRenderTarget(graphicsDevice.DepthStencilBuffer, graphicsDevice.BackBuffer);
        }

        public void SetRenderTarget(IRenderTarget2D depth, IRenderTarget2D color)
        {
            var dxDepth = graphicsDevice.Cast<Texture2D>(depth, "depth");
            var dxColor = graphicsDevice.Cast<Texture2D>(color, "color");

            if (dxDepth.DepthView == null)
                throw new ArgumentException("Texture is not a depth render target.", "depth");
            if (dxColor.RenderView == null)
                throw new ArgumentException("Texture is not a color render target.", "color");

            if (currentColorTargets != null && currentColorTargets.Length == 1 && dxColor.RenderView == currentColorTargets[0] && currentDepthTarget == dxDepth.DepthView)
                return;

            currentColorTargets = new RenderTargetView[] { dxColor.RenderView };
            currentDepthTarget = dxDepth.DepthView;

            context.OutputMerger.SetTargets(currentDepthTarget, currentColorTargets);
            context.Rasterizer.SetViewport(new SharpDX.ViewportF(0, 0, dxColor.Width, dxColor.Height, 0.0f, 1.0f));
        }

        public void SetRenderTargetColor(IRenderTarget2D color)
        {
            // TODO (henrik1235) color = null Rendering erlauben
            var dxColor = graphicsDevice.Cast<Texture2D>(color, "color");

            if (dxColor.RenderView == null)
                throw new ArgumentException("Texture is not a color render target.", "color");

            if (currentColorTargets != null && currentColorTargets.Length == 1 && dxColor.RenderView == currentColorTargets[0])
                return;

            currentColorTargets = new RenderTargetView[] { dxColor.RenderView };

            context.OutputMerger.SetTargets(currentDepthTarget, currentColorTargets);
            context.Rasterizer.SetViewport(new SharpDX.ViewportF(0, 0, dxColor.Width, dxColor.Height, 0.0f, 1.0f));
        }

        public void SetRenderTargetDepth(IRenderTarget2D depth)
        {
            // TODO (henrik1235) depth = null Rendering erlauben
            var dxDepth = graphicsDevice.Cast<Texture2D>(depth, "depth");

            if (dxDepth.DepthView == null)
                throw new ArgumentException("Texture is not a depth render target.", "depth");

            if (dxDepth.DepthView == currentDepthTarget)
                return;

            currentDepthTarget = dxDepth.DepthView;

            if (currentColorTargets != null)
                context.OutputMerger.SetTargets(currentDepthTarget, currentColorTargets);
            else
                context.OutputMerger.SetTargets(currentDepthTarget);
        }

        public void SetRenderTargets(IRenderTarget2D depth, params IRenderTarget2D[] colorTargets)
        {
            SetRenderTargetDepth(depth);
            SetRenderTargetsColor(colorTargets);
        }

        public void SetRenderTargetsColor(params IRenderTarget2D[] colorTargets)
        {
            if (colorTargets == null)
                throw new ArgumentNullException("colorTargets");
            if (colorTargets.Length == 0)
                throw new ArgumentException("ColorTargets is empty.", "colorTargets");

            RenderTargetView[] targets = new RenderTargetView[colorTargets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                Texture2D target = graphicsDevice.Cast<Texture2D>(colorTargets[i], "colorTargets[" + i + "]");
                if (target.RenderView == null)
                    throw new ArgumentException("Texture at index " + i + " is not a color target", "colorTargets");
                targets[i] = target.RenderView;
            }

            currentColorTargets = targets;
            context.OutputMerger.SetTargets(currentDepthTarget, currentColorTargets);
        }

        public void SetShader(IShader shader)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");
            graphicsDevice.Cast<IShader>(shader, "shader"); // Shader überprüfen

            currentState.Shader = shader;
            stateDirty = true;
        }

        public void SetPrimitiveType(PrimitiveType type)
        {
            EnumConverter.Convert(type); // Type überprüfen (ob supported ist)

            currentState.PrimitiveType = type;
            stateDirty = true;
        }

        public void SetRasterizer(IRasterizerState rasterizerState)
        {
            graphicsDevice.Cast<RasterizerState>(rasterizerState, "rasterizerState"); // State überprüfen

            currentState.Rasterizer = rasterizerState;
            stateDirty = true;
        }

        public void SetState(IRenderState state)
        {
            graphicsDevice.Cast<RenderState>(state, "state"); // State überprüfen

            if (!state.Info.Equals(currentState))
            {
                stateDirty = true;
                currentState = state.Info;
            }
        }

        public void SetVertexBuffer(IVertexBuffer vertexBuffer)
        {
            var dxVertexBuffer = graphicsDevice.Cast<VertexBuffer>(vertexBuffer, "vertexBuffer"); ;
            if (currentVertexBuffer != dxVertexBuffer)
            {
                currentVertexBuffer = dxVertexBuffer;
                vertexBufferDirty = true;
            }
        }

        public void SetIndexBuffer(IIndexBuffer indexBuffer)
        {
            var dxIndexBuffer = graphicsDevice.Cast<IndexBuffer>(indexBuffer, "indexBuffer"); 
            if (currentIndexBuffer != dxIndexBuffer)
            {
                currentIndexBuffer = dxIndexBuffer;
                indexBufferDirty = true;
            }
        }

        public void SetConstantBuffer(IShader shader, IConstantBuffer buffer)
        {
            SetConstantBuffer(shader, "$Globals", buffer);
        }

        public void SetConstantBuffer(IShader shader, string name, IConstantBuffer buffer)
        {
            var dxShader = graphicsDevice.Cast<Shader>(shader, "shader");
            if (shader != currentState.Shader)
                throw new ArgumentException("Shader does not match current set shader.", "shader");
            var dxBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");

            int slot;
            if (dxShader.TryGetSlotVertex(name, out slot))
                SetConstantBuffer(context.VertexShader, vertexCache, slot, dxBuffer.Handle);
            if (dxShader.TryGetSlotPixel(name, out slot))
                SetConstantBuffer(context.PixelShader, pixelCache, slot,  dxBuffer.Handle);
        }

        private void SetConstantBuffer(CommonShaderStage stage, ShaderStageCache cache, int slot, Buffer buffer)
        {
            if (cache.Buffers[slot] != buffer)
            {
                stage.SetConstantBuffer(slot, buffer);
                cache.Buffers[slot] = buffer;
            }
        }

        public void SetTextureNull(IShader shader, string name)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            SetTexture(shader, name, (ShaderResourceView)null);
        }

        public void SetTexture(IShader shader, string name, ITexture2D texture)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (shader == null)
                throw new ArgumentNullException("shader");
            if (name == null)
                throw new ArgumentNullException("name");
            SetTexture(shader, name, graphicsDevice.Cast<Texture2D>(texture, "texture").ResourceView);
        }
        public void SetTexture(IShader shader, string name, ITexture2DArray texture)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            SetTexture(shader, name, graphicsDevice.Cast<Texture2D>(texture, "texture").ResourceView);
        }
        public void SetTexture(IShader shader, string name, ITexture3D texture)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            throw new NotImplementedException();
            //SetTexture(shader, name, graphicsDevice.Cast<Texture3D>(texture, "texture").ResourceView);
        }
        public void SetTexture(IShader shader, string name, ITexture3DArray texture)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            throw new NotImplementedException();
            //SetTexture(shader, name, graphicsDevice.Cast<Texture3D>(texture, "texture").ResourceView);
        }

        private void SetTexture(IShader shader, string name, ShaderResourceView view)
        {
            if (shader != currentState.Shader)
                throw new ArgumentException("Shader does not match current set shader.", "shader");

            var dxShader = graphicsDevice.Cast<Shader>(shader, "shader");
            int slot;
            if (dxShader.TryGetSlotVertex(name, out slot))
                SetTexture(context.VertexShader, vertexCache, slot, view);
            if (dxShader.TryGetSlotPixel(name, out slot))
                SetTexture(context.PixelShader, pixelCache, slot, view);
        }

        private void SetTexture(CommonShaderStage stage, ShaderStageCache cache, int slot, ShaderResourceView view)
        {
            if (cache.Views[slot] != view)
            {
                stage.SetShaderResource(slot, view);
                cache.Views[slot] = view;
            }
        }

        public void SetSampler(IShader shader, string name, ISampler sampler)
        {
            graphicsDevice.Cast<Shader>(shader, "shader");
            if (name == null)
                throw new ArgumentNullException("name");
            if (shader != currentState.Shader)
                throw new ArgumentException("Shader does not match current set shader.", "shader");

            var dxShader = graphicsDevice.Cast<Shader>(shader, "shader");
            var dxSampler = graphicsDevice.Cast<Sampler>(sampler, "sampler");

            int slot;
            if (dxShader.TryGetSlotVertex(name, out slot))
                SetSampler(context.VertexShader, vertexCache, slot, dxSampler.Handle);
            if (dxShader.TryGetSlotPixel(name, out slot))
                SetSampler(context.PixelShader, pixelCache, slot, dxSampler.Handle);
        }

        private void SetSampler(CommonShaderStage stage, ShaderStageCache cache, int slot, SamplerState sampler)
        {
            if (cache.Samplers[slot] != sampler)
            {
                stage.SetSampler(slot, sampler);
                cache.Samplers[slot] = sampler;
            }
        }

        private void ApplyState()
        {
            var shader = graphicsDevice.Cast<Shader>(currentState.Shader, "currentState.Shader");
            if (stateDirty)
            {
                if (vertexCache.Shader != shader)
                {
                    context.VertexShader.Set(shader.VertexShaderHandle);
                    vertexCache.Shader = shader;
                }
                if (pixelCache.Shader != shader)
                {
                    context.PixelShader.Set(shader.PixelShaderHandle);
                    pixelCache.Shader = shader;
                }
                if (primitiveType != currentState.PrimitiveType)
                {
                    context.InputAssembler.PrimitiveTopology = EnumConverter.Convert(currentState.PrimitiveType);
                    primitiveType = currentState.PrimitiveType;
                }
                if (currentRasterizer != currentState.Rasterizer)
                {
                    var rasterizer = graphicsDevice.Cast<RasterizerState>(currentState.Rasterizer, "currentState.Rasterizer");
                    context.Rasterizer.State = rasterizer.Handle;

                    currentRasterizer = rasterizer;
                }
            }
            if (vertexBufferDirty)
            {
                context.InputAssembler.InputLayout = graphicsDevice.GetLayout(currentVertexBuffer.Description, shader);
                context.InputAssembler.SetVertexBuffers(0, currentVertexBuffer.Binding);
            }
            if (indexBufferDirty)
                context.InputAssembler.SetIndexBuffer(currentIndexBuffer.Buffer, currentIndexBuffer.IndexFormat, 0);

            stateDirty = false;
            vertexBufferDirty = false;
            indexBufferDirty = false;
        }

        public void Draw()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");

            ApplyState();
            context.Draw(currentVertexBuffer.VertexCount, 0);
        }

        public void DrawIndexed()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");
            if (currentIndexBuffer == null)
                throw new InvalidOperationException("Tried to draw without an indexbuffer set.");

            ApplyState();
            context.DrawIndexed(currentIndexBuffer.IndexCount, 0, 0);
        }

        private struct ShaderStageCache
        {
            public Shader Shader;
            public Buffer[] Buffers;
            public ShaderResourceView[] Views;
            public SamplerState[] Samplers;

            public ShaderStageCache(Shader Shader)
                : this()
            {
                this.Shader = Shader;

                Buffers = new Buffer[CommonShaderStage.ConstantBufferHwSlotCount];
                Views = new ShaderResourceView[CommonShaderStage.InputResourceSlotCount];
                Samplers = new SamplerState[CommonShaderStage.SamplerSlotCount];
            }
        }
    }
}