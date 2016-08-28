using DotGame.Graphics;
using DotGame.Utils;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using Device = SharpDX.Direct3D11.Device;
using DeviceCreationFlags = DotGame.Graphics.DeviceCreationFlags;

namespace DotGame.DirectX11
{
    public sealed class GraphicsDevice : IGraphicsDevice
    {
        internal Device Device { get; private set; }
        private SwapChain swapChain;

        internal Texture2D BackBuffer { get; private set; }
        internal IRenderTarget2D DepthStencilBuffer { get; private set; }

        public bool IsDisposed { get; private set; }

        public DeviceCreationFlags CreationFlags { get; private set; }
        public GraphicsCapabilities Capabilities { get; private set; }
        public IGraphicsFactory Factory { get; private set; }
        public IRenderContext RenderContext { get; private set; }

        public IGameWindow DefaultWindow { get; private set; }

        internal List<GraphicsObject> CreatedObjects { get; private set; }

        private int syncInterval = 0;
        public bool VSync
        {
            get { return syncInterval != 0; }
            set { syncInterval = value ? 1 : 0; }
        }

        private Dictionary<VertexDescription, InputLayout> inputLayoutPool = new Dictionary<VertexDescription, InputLayout>();


        internal GraphicsDevice(IGameWindow window, Device device, SwapChain swapChain, DeviceCreationFlags creationFlags)
        {
            if (window == null)
                throw new ArgumentNullException("window");
            if (device == null)
                throw new ArgumentNullException("device");
            if (device.IsDisposed)
                throw new ObjectDisposedException("device");
            if (swapChain == null)
                throw new ArgumentNullException("swapChain");
            if (swapChain.IsDisposed)
                throw new ObjectDisposedException("swapChain");


            this.DefaultWindow = window;
            this.Device = device;
            this.swapChain = swapChain;
            this.CreationFlags = CreationFlags;

            this.CreatedObjects = new List<GraphicsObject>();

            this.Device.DebugName = string.Format("{0}@{1:X}", GetType().FullName, ((object)this).GetHashCode());
            this.Factory = new GraphicsFactory(this);
            this.RenderContext = new RenderContext(this, device.ImmediateContext);

            this.Capabilities = new GraphicsCapabilities()
            {
                SupportsBinaryShaders = true
            };

            InitBackbuffer();
        }

        private void InitBackbuffer()
        {
            BackBuffer = new Texture2D(this, swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));
            DepthStencilBuffer = Factory.CreateRenderTarget2D(DefaultWindow.Width, DefaultWindow.Height, TextureFormat.Depth24Stencil8, false);
            RenderContext.SetRenderTargetsBackBuffer();
            RenderContext.SetViewport(new Viewport(0, 0, DefaultWindow.Width, DefaultWindow.Height, 0, 1));
        }

        public T Cast<T>(IGraphicsObject obj, string parameterName) where T : class, IGraphicsObject
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
            if (obj.IsDisposed)
                throw new ObjectDisposedException(parameterName);

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

            if (RenderContext != null)
                RenderContext.Dispose();
            if (BackBuffer != null)
                BackBuffer.Dispose();
            if (DepthStencilBuffer != null)
                DepthStencilBuffer.Dispose();
            //if (Factory != null)
                //Factory.Dispose();
            if (swapChain != null)
                swapChain.Dispose();

            GraphicsObject[] created = CreatedObjects.ToArray();
            for (int i = 0; i < created.Length; i++)
                if (!created[i].IsDisposed)
                {
                    Log.Warning("{0} was not disposed! Created at:\r\n{1}", created[i].GetType().Name, created[i].CreationStack);
                    created[i].Dispose();
                }
            CreatedObjects.Clear();

            if (Device != null)
                Device.Dispose();

            IsDisposed = true;
        }

        public void MakeCurrent()
        {
        }
        public void DetachCurrent()
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
            layout = inputLayoutPool[description] = new InputLayout(Device, shader.VertexCode, dxElements);
            return layout;
        }

        public void SwapBuffers()
        {
            swapChain.Present(syncInterval, PresentFlags.None);
        }
    }
}
