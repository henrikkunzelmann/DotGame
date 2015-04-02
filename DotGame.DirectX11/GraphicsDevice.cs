using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using System.Windows.Forms;
using SharpDX.Windows;
using SharpDX.DXGI;
using DotGame.Utils;

namespace DotGame.DirectX11
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        private Device device;
        private SwapChain swapChain;

        private Texture2D backBuffer;
        private Texture2D depthBuffer;

        public bool IsDisposed { get; private set; }
        public IGraphicsFactory Factory { get; private set; }
        public IRenderContext RenderContext { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }
        
        internal DeviceContext Context { get; private set; }

        private int syncInterval = 0;
        public bool VSync
        {
            get { return syncInterval != 0; }
            set { syncInterval = value ? 1 : 0; }
        }

        private RenderTargetView currentRenderTarget;
        private DepthStencilView currentDepthTarget;

        private Dictionary<VertexDescription, InputLayout> inputLayoutPool = new Dictionary<VertexDescription, InputLayout>();

        internal GraphicsDevice(IGameWindow window, Device device, SwapChain swapChain)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (device == null)
                throw new ArgumentNullException("device");
            if (device.IsDisposed)
                throw new ArgumentException("Device is disposed.", "device");
            if (swapChain == null)
                throw new ArgumentNullException("swapChain");
            if (swapChain.IsDisposed)
                throw new ArgumentException("SwapChain is disposed.", "swapChain");

            this.DefaultWindow = window;
            this.device = device;
            this.swapChain = swapChain;
            this.Context = device.ImmediateContext;

            this.Factory = new GraphicsFactory(this);
            this.RenderContext = new RenderContext(this);

            InitBackbuffer();
        }

        private void InitBackbuffer()
        {
            backBuffer = new Texture2D(this, swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));
            depthBuffer = (Texture2D)Factory.CreateRenderTarget2D(DefaultWindow.Width, DefaultWindow.Height, TextureFormat.Depth16);
            SetRenderTarget(backBuffer, depthBuffer);

            Context.Rasterizer.State = new RasterizerState(Context.Device, new RasterizerStateDescription()
            {
                CullMode = CullMode.Back,
                FillMode = FillMode.Solid,
            });
        }

        public T Cast<T>(IGraphicsObject obj, string parameterName) where T : class, IGraphicsObject
        {
            T ret = obj as T;
            if (ret == null)
                throw new ArgumentException("GraphicsObject is not part of this api.", parameterName);
            if (obj.GraphicsDevice != this)
                throw new ArgumentException("GraphicsObject is not part of this graphics device.", parameterName);
            return ret;
        }

        public int GetSizeOf(TextureFormat format)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(EnumConverter.Convert(format));
        }

        public int GetSizeOf(VertexElementType format)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(EnumConverter.Convert(format));
        }

        public int GetSizeOf(IndexFormat format)
        {
            return SharpDX.DXGI.FormatHelper.SizeOfInBytes(EnumConverter.Convert(format));
        }

        public int GetSizeOf(VertexDescription description)
        {
            int size = 0;
            VertexElement[] elements = description.GetElements();
            for (int i = 0; i < elements.Length; i++)
                size += GetSizeOf(elements[i].Type);

            return size;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;


            if (Factory != null && !Factory.IsDisposed)
                Factory.Dispose();

            if (backBuffer != null && !backBuffer.IsDisposed)
                backBuffer.Dispose();

            IsDisposed = true;
        }

        public void MakeCurrent()
        {

        }

        public InputLayout GetLayout(VertexDescription description, Shader shader)
        {
            InputLayout layout;
            if (inputLayoutPool.TryGetValue(description, out layout))
                return layout;

            InputElement[] dxElements = new InputElement[description.ElementCount];
            VertexElement[] elements = description.GetElements();

            for (int i = 0; i < description.ElementCount; i++)
                dxElements[i] = new InputElement(EnumConverter.Convert(elements[i].Usage), 0, EnumConverter.Convert(elements[i].Type), elements[i].UsageIndex);
            layout = inputLayoutPool[description] = new InputLayout(Context.Device, shader.VertexCode, dxElements);
            return layout;
        }

        public void SwapBuffers()
        {
            swapChain.Present(syncInterval, PresentFlags.None);
        }


        public void Clear(Color color)
        {
            if (currentRenderTarget == null)
                throw new InvalidOperationException("No render target set for color.");
            Context.ClearRenderTargetView(currentRenderTarget, new SharpDX.Color4(color.R, color.G, color.B, color.A));
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            if (clearOptions.HasFlag(ClearOptions.Color))
                Clear(color);

            DepthStencilClearFlags clearFlags = 0;
            if (clearOptions.HasFlag(ClearOptions.Depth))
                clearFlags|= DepthStencilClearFlags.Depth;
            if (clearOptions.HasFlag(ClearOptions.Stencil))
                clearFlags |= DepthStencilClearFlags.Stencil;

            if (clearFlags != 0)
            {
                if (currentDepthTarget == null)
                    throw new InvalidOperationException("No render target set for depth.");
                Context.ClearDepthStencilView(currentDepthTarget, clearFlags, depth, (byte)stencil);
            }
        }

        public void SetRenderTarget(IRenderTarget2D color, IRenderTarget2D depth)
        {
            if (color == null)
                throw new ArgumentNullException("color");
            var dxColor = Cast<Texture2D>(color, "color");
            if (depth == null)
                throw new ArgumentNullException("depth");
            var dxDepth = Cast<Texture2D>(depth, "depth");

            if (dxColor.RenderView == null)
                throw new ArgumentException("Texture is not a color render target.", "color");
            if (dxDepth.DepthView == null)
                throw new ArgumentException("Texture is not a depth render target.", "depth");

            if (currentRenderTarget == dxColor.RenderView && currentDepthTarget == dxDepth.DepthView)
                return;

            currentRenderTarget = dxColor.RenderView;
            currentDepthTarget = dxDepth.DepthView;

            Context.OutputMerger.SetTargets(currentDepthTarget, currentRenderTarget);
            Context.Rasterizer.SetViewport(new SharpDX.ViewportF(0, 0, dxColor.Width, dxColor.Height, 0.0f, 1.0f));
        }

        public void SetRenderTargetColor(IRenderTarget2D color)
        {
            if (color == null)
                throw new ArgumentNullException("color");
            var dxColor = Cast<Texture2D>(color, "color");

            if (dxColor.RenderView == null)
                throw new ArgumentException("Texture is not a color render target.", "color");

            if (dxColor.RenderView == currentRenderTarget)
                return;

            currentRenderTarget = dxColor.RenderView;

            Context.OutputMerger.SetTargets(currentDepthTarget, currentRenderTarget);
            Context.Rasterizer.SetViewport(new SharpDX.ViewportF(0, 0, dxColor.Width, dxColor.Height, 0.0f, 1.0f));
        }

        public void SetRenderTargetDepth(IRenderTarget2D depth)
        {
            if (depth == null)
                throw new ArgumentNullException("depth");
            var dxDepth = Cast<Texture2D>(depth, "depth");

            if (dxDepth.DepthView == null)
                throw new ArgumentException("Texture is not a depth render target.", "depth");

            if (dxDepth.DepthView == currentDepthTarget)
                return;

            currentDepthTarget = dxDepth.DepthView;

            Context.OutputMerger.SetTargets(currentDepthTarget, currentRenderTarget);
        }
    }
}
