using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using DotGame.Graphics;
using DotGame.Utils;
using Texture2D = DotGame.DirectX11.Texture2D;

namespace DotGame.DirectX11
{
    public class Shader : GraphicsObject, IShader
    {
        public string Name { get; private set; }

        internal ShaderBytecode VertexCode { get; private set; }
        internal ShaderBytecode PixelCode { get; private set; }

        private byte[] binaryCode;
        public byte[] BinaryCode
        {
            get { return (byte[])binaryCode.Clone(); }
        }

        internal VertexShader VertexShaderHandle { get; private set; }
        internal PixelShader PixelShaderHandle { get; private set; }

        private Dictionary<string, int> resourcesVertex = new Dictionary<string, int>();
        private Dictionary<string, int> resourcesPixel = new Dictionary<string, int>();
        private Dictionary<string, int> constantBufferSizes = new Dictionary<string, int>();

        public Shader(GraphicsDevice graphicsDevice, string name, ShaderBytecode vertex, ShaderBytecode pixel)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is empty or whitespace.", "name");
            if (vertex == null)
                throw new ArgumentNullException("vertex");
            if (pixel == null)
                throw new ArgumentNullException("pixel");

            this.Name = name;
            this.VertexCode = vertex;
            this.PixelCode = pixel;

            using (ShaderReflection reflection = new ShaderReflection(VertexCode))
            {
                for (int i = 0; i < reflection.Description.BoundResources; i++)
                {
                    var res = reflection.GetResourceBindingDescription(i);
                    resourcesVertex[res.Name] = res.BindPoint;

                    if (res.Type == ShaderInputType.ConstantBuffer)
                        constantBufferSizes[res.Name] = reflection.GetConstantBuffer(res.Name).Description.Size;
                }
            }

            using (ShaderReflection reflection = new ShaderReflection(PixelCode))
            {
                for (int i = 0; i < reflection.Description.BoundResources; i++)
                {
                    var res = reflection.GetResourceBindingDescription(i);
                    resourcesPixel[res.Name] = res.BindPoint;

                    if (res.Type == ShaderInputType.ConstantBuffer)
                        constantBufferSizes[res.Name] = reflection.GetConstantBuffer(res.Name).Description.Size;
                }
            }

            VertexShaderHandle = new VertexShader(graphicsDevice.Device, VertexCode);
            PixelShaderHandle = new PixelShader(graphicsDevice.Device, PixelCode);

            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("DIRECTX11");
                writer.Write(VertexCode.Data.Length);
                writer.Write(VertexCode.Data);
                writer.Write(PixelCode.Data.Length);
                writer.Write(PixelCode.Data);
                binaryCode = stream.GetBuffer();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (VertexShaderHandle != null)
                VertexShaderHandle.Dispose();
            if (PixelShaderHandle != null)
                PixelShaderHandle.Dispose();
            if (VertexCode != null)
                VertexCode.Dispose();
            if (PixelCode != null)
                PixelCode.Dispose();
        }

        public IConstantBuffer CreateConstantBuffer()
        {
            return CreateConstantBuffer("$Globals");
        }
 
        public IConstantBuffer CreateConstantBuffer(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            int size;
            if (!constantBufferSizes.TryGetValue(name, out size))
                throw new ArgumentException("Constant buffer not bound to shader.", "name");
            return graphicsDevice.Factory.CreateConstantBuffer(size);
        }

        public bool TryGetSlotVertex(string name, out int slot)
        {
            return resourcesVertex.TryGetValue(name, out slot);
        }

        public bool TryGetSlotPixel(string name, out int slot)
        {
            return resourcesPixel.TryGetValue(name, out slot);
        }
    }
}
