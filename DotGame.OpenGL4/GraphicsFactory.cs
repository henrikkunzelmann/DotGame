using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.ObjectModel;
using DotGame.Utils;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : IGraphicsFactory
    {
        public static Dictionary<TextureFormat, PixelInternalFormat> TextureFormats = new Dictionary<TextureFormat, PixelInternalFormat>() 
        {
            {TextureFormat.RGB32_Float, PixelInternalFormat.Rgb32f},
            {TextureFormat.DXT1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext},
            {TextureFormat.DXT3, PixelInternalFormat.CompressedRgbaS3tcDxt3Ext},
            {TextureFormat.DXT5, PixelInternalFormat.CompressedRgbaS3tcDxt5Ext},
            {TextureFormat.RGBA16_UIntNorm, PixelInternalFormat.Rgba16},
            {TextureFormat.RGBA8_UIntNorm, PixelInternalFormat.Rgba8},
            {TextureFormat.Depth16, PixelInternalFormat.DepthComponent16},
            {TextureFormat.Depth32, PixelInternalFormat.DepthComponent32},
            {TextureFormat.Depth24Stencil8, PixelInternalFormat.Depth24Stencil8},
        };

        public bool IsDisposed { get; private set; }
        public IGraphicsDevice GraphicsDevice { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal readonly GraphicsDevice GraphicsDeviceInternal;
        internal readonly List<GraphicsObject> DeferredDispose; // Wird zum disposen benutzt, um MakeCurrent Aufrufe zu vermeiden. Siehe DisposeUnused und Referenzen dazu.
        internal ReadOnlyCollection<WeakReference<GraphicsObject>> Objects { get { return objects.AsReadOnly(); } }
        private readonly List<WeakReference<GraphicsObject>> objects;

        internal GraphicsFactory(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            GraphicsDeviceInternal = graphicsDevice;

            DeferredDispose = new List<GraphicsObject>();
            objects = new List<WeakReference<GraphicsObject>>();
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format)
        {
            AssertCurrent();
            return Register(new Texture2D(GraphicsDeviceInternal, width, height, 0, format));
        }

        public ITexture3D CreateTexture3D(int width, int height, int length, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture2DArray CreateTexture2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture3DArray CreateTexture3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3D CreateRenderTarget3D(int width, int height, int length, TextureFormat format)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget2DArray CreateRenderTarget2DArray(int width, int height, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IRenderTarget3DArray CreateRenderTarget3DArray(int width, int height, int length, TextureFormat format, int arraySize)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description) where T : struct, IVertexType
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data) where T : struct
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        internal void DisposeUnused()
        {
            // TODO (Joex3): Wieder eigene Exception? :s
            if (!GraphicsDevice.IsDisposed && !GraphicsDeviceInternal.IsCurrent)
                throw new Exception("DisposeUnused must be called in the render thread, or after the GraphicsDevice has been disposed.");

            if (DeferredDispose.Count > 0)
            {
                foreach (var obj in DeferredDispose)
                {
                    // Das sollte eigentlich nie passieren. :s
                    if (obj.IsDisposed)
                        continue;

                    DotGame.Utils.Log.Warning("{0} not disposed! {1}", obj.GetType().FullName, obj.CreationTrace);

                    obj.Dispose(false);
                }
                DeferredDispose.Clear();
            }
        }

        private T Register<T>(T obj) where T : GraphicsObject
        {
            objects.Add(new WeakReference<GraphicsObject>(obj));
            return obj;
        }

        private void AssertNotDisposed()
        {
            if (GraphicsDevice.IsDisposed)
                throw new ObjectDisposedException(GraphicsDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private void AssertCurrent()
        {
            AssertNotDisposed();

            // TODO (Joex3): Eigene Exception.
            if (!GraphicsDeviceInternal.IsCurrent)
                throw new Exception(string.Format("GraphicsDevice is not available on Thread {0}.", System.Threading.Thread.CurrentThread.Name));
        }

        public void Dispose()
        {
            Log.Info("GraphicsFactory.Dispose() called!");
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            DisposeUnused();

            IsDisposed = true;
        }
    }
}