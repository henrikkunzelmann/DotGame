using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.ObjectModel;
using System.Diagnostics;
using DotGame.Utils;

namespace DotGame.OpenGL4
{
    public class GraphicsFactory : GraphicsObject, IGraphicsFactory
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

        internal readonly List<GraphicsObject> DeferredDispose; // Wird zum disposen benutzt, um MakeCurrent Aufrufe zu vermeiden. Siehe DisposeUnused und Referenzen dazu.
        internal ReadOnlyCollection<WeakReference<GraphicsObject>> Objects { get { return objects.AsReadOnly(); } }
        private readonly List<WeakReference<GraphicsObject>> objects;

        internal GraphicsFactory(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {
            DeferredDispose = new List<GraphicsObject>();
            objects = new List<WeakReference<GraphicsObject>>();
        }
        
        public ITexture2D LoadTexture2D(string file)
        {
            AssertCurrent();
            throw new NotImplementedException();
        }

        public ITexture2D CreateTexture2D(int width, int height, TextureFormat format)
        {
            AssertCurrent();
            return Register(new Texture2D(graphicsDevice, width, height, 0, format));
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

        public IVertexBuffer CreateVertexBuffer<T>(T[] data, VertexDescription description) where T : struct
        {
            AssertCurrent();
            VertexBuffer buffer = new VertexBuffer(graphicsDevice, description);
            buffer.SetData<T>(data);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer<T>(T[] data, IndexFormat format) where T : struct
        {
            AssertCurrent();
            IndexBuffer buffer = new IndexBuffer(graphicsDevice);
            buffer.SetData<T>(data, format);
            return buffer;
        }

        public IIndexBuffer CreateIndexBuffer(int[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.Int32);
        }

        public IIndexBuffer CreateIndexBuffer(uint[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.UInt32);
        }

        public IIndexBuffer CreateIndexBuffer(short[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.Short16);
        }

        public IIndexBuffer CreateIndexBuffer(ushort[] data)
        {
            return CreateIndexBuffer(data, IndexFormat.UShort16);
        }

        public IConstantBuffer CreateConstantBuffer(int size)
        {
            throw new NotImplementedException();
        }

        public IShader CompileShader(string name, ShaderCompileInfo vertex, ShaderCompileInfo pixel)
        {
            throw new NotImplementedException();
        }

        public IShader CreateShader(string name, byte[] vertexCode, byte[] pixelCode)
        {
            throw new NotSupportedException("Creating shader from byte code is not supported.");
        }


        internal void DisposeUnused()
        {
            // TODO (Joex3): Wieder eigene Exception? 
            if (!graphicsDevice.IsDisposed && !graphicsDevice.IsCurrent)
                throw new Exception("DisposeUnused must be called in the render thread, or after the GraphicsDevice has been disposed.");

            if (DeferredDispose.Count > 0)
            {
                foreach (var obj in DeferredDispose)     
                {
                    // TODO: (henrik1235) Klären ob die Liste nicht in IGraphicsDevice verschieben können
                    if (obj is GraphicsFactory)
                        return;

                    // Das sollte eigentlich nie passieren.
                    if (obj.IsDisposed)
                    {
                        Log.Debug("{0} is in DeferredDispose queue, but its already disposed.", obj.GetType().FullName);
                        continue;
                    }

                    DotGame.Utils.Log.Warning("{0} is not disposed! {1}", obj.GetType().FullName, obj);

                    obj.Dispose(); // TODO: (henrik1235) Überprüfen ob wir nicht obj.Dispose(bool isDisposing) aufrufen sollten, inbesondere weil dann die DeferredDispose Liste verändert werden kann
                }
                DeferredDispose.Clear();
            }
        }

        private T Register<T>(T obj) where T : GraphicsObject
        {
            objects.Add(new WeakReference<GraphicsObject>(obj));
            return obj;
        }

        protected override void Dispose(bool isDisposing)
        {
            DisposeUnused();
        }
    }
}