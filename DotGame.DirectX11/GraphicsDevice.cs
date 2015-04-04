﻿using System;
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
        internal Device Device { get; private set; }
        private SwapChain swapChain;

        private Texture2D backBuffer;
        private Texture2D depthBuffer;

        public bool IsDisposed { get; private set; }
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
            this.Device = device;
            this.swapChain = swapChain;

            this.CreatedObjects = new List<GraphicsObject>();

            this.Factory = new GraphicsFactory(this);
            this.RenderContext = new RenderContext(this, device.ImmediateContext);

            InitBackbuffer();
        }

        private void InitBackbuffer()
        {
            backBuffer = new Texture2D(this, swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0));
            depthBuffer = (Texture2D)Factory.CreateRenderTarget2D(DefaultWindow.Width, DefaultWindow.Height, TextureFormat.Depth16);
            RenderContext.SetRenderTarget(backBuffer, depthBuffer);
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

            backBuffer.Dispose();
            depthBuffer.Dispose();
            Factory.Dispose();

            GraphicsObject[] created = CreatedObjects.ToArray();
            for (int i = 0; i < created.Length; i++)
                if (!created[i].IsDisposed)
                {
                    Log.Warning("{0} was not disposed! Created at:\r\n{1}", created[i].GetType().Name, created[i].CreationStack);
                    created[i].Dispose();
                }
            CreatedObjects.Clear();

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
