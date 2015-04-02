using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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

        public ShaderBytecode VertexCode { get; private set; }
        public ShaderBytecode PixelCode { get; private set; }

        public VertexShader VertexShaderHandle { get; private set; }
        public PixelShader PixelShaderHandle { get; private set; }

        private Dictionary<string, int> resourcesVertex = new Dictionary<string, int>();
        private Dictionary<string, int> resourcesPixel = new Dictionary<string, int>();
        private Dictionary<string, int> constantBufferSizes = new Dictionary<string, int>();
        private Dictionary<string, IConstantBuffer> cachedConstantBuffers = new Dictionary<string, IConstantBuffer>();

        // TODO (henrik1235) Test, entfernen und mit ISampler ersetzen
        private SamplerState sampler;

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

            using (ShaderReflection reflection = new ShaderReflection(VertexCode.Data))
            {
                for (int i = 0; i < reflection.Description.BoundResources; i++)
                {
                    var res = reflection.GetResourceBindingDescription(i);
                    resourcesVertex[res.Name] = res.BindPoint;

                    if (res.Type == ShaderInputType.ConstantBuffer)
                        constantBufferSizes[res.Name] = reflection.GetConstantBuffer(res.Name).Description.Size;
                }
            }

            using (ShaderReflection reflection = new ShaderReflection(PixelCode.Data))
            {
                for (int i = 0; i < reflection.Description.BoundResources; i++)
                {
                    var res = reflection.GetResourceBindingDescription(i);
                    resourcesPixel[res.Name] = res.BindPoint;

                    if (res.Type == ShaderInputType.ConstantBuffer)
                        constantBufferSizes[res.Name] = reflection.GetConstantBuffer(res.Name).Description.Size;
                }
            }

            VertexShaderHandle = new VertexShader(graphicsDevice.Context.Device, VertexCode.Data);
            PixelShaderHandle = new PixelShader(graphicsDevice.Context.Device, PixelCode.Data);

            // TODO (henrik1235) Test, entfernen und mit ISampler ersetzen
            sampler = new SamplerState(graphicsDevice.Context.Device, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipPoint,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                BorderColor = SharpDX.Color.Black,
                ComparisonFunction = SharpDX.Direct3D11.Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16,
            });
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

            foreach (KeyValuePair<string, IConstantBuffer> buffer in cachedConstantBuffers)
                buffer.Value.Dispose();
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
            IConstantBuffer buffer;
            if (cachedConstantBuffers.TryGetValue(name, out buffer))
                return buffer;
            return graphicsDevice.Factory.CreateConstantBuffer(size);
        }

        public void SetConstantBuffer(IConstantBuffer buffer)
        {
            SetConstantBuffer("$Globals", buffer);
        }

        public void SetConstantBuffer(string variableName, IConstantBuffer buffer)
        {
            if (variableName == null)
                throw new ArgumentNullException("variableName");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            ConstantBuffer dxBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");

            int slot;
            bool vertexFound = false, pixelFound = false;
            if (resourcesVertex.TryGetValue(variableName, out slot))
            {
                graphicsDevice.Context.VertexShader.SetConstantBuffer(slot, dxBuffer.Handle);
                vertexFound = true;
            }

            if (resourcesPixel.TryGetValue(variableName, out slot))
            {
                graphicsDevice.Context.PixelShader.SetConstantBuffer(slot, dxBuffer.Handle);
                pixelFound = true;
            }

            if (!vertexFound && !pixelFound)
                throw new ArgumentException("ConstantBuffer not found in shader.", "name");
        }

        public void SetTexture(string name, ITexture2D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            SetTexture(name, graphicsDevice.Cast<Texture2D>(texture, "texture").ResourceView);
        }

        public void SetTexture(string name, ITexture2DArray texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            SetTexture(name, graphicsDevice.Cast<Texture2D>(texture, "texture").ResourceView);
        }

        public void SetTexture(string name, ITexture3D texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            throw new NotImplementedException();
            //SetTexture(name, graphicsDevice.Cast<Texture3D>(texture, "texture").ResourceView);
        }

        public void SetTexture(string name, ITexture3DArray texture)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (texture == null)
                throw new ArgumentNullException("texture");

            throw new NotImplementedException();
            //SetTexture(name, graphicsDevice.Cast<Texture3D>(texture, "texture").ResourceView);
        }

        private void SetTexture(string name, ShaderResourceView view)
        {
            if (view == null)
                throw new ArgumentException("Texture does not support being used in a shader.", "texture");

            int slot;
            bool vertexFound = false, pixelFound = false;
            if (resourcesVertex.TryGetValue(name, out slot))
            {
                graphicsDevice.Context.PixelShader.SetSampler(slot, sampler); // TODO (henrik1235) Test, entfernen und mit ISampler ersetzen
                graphicsDevice.Context.VertexShader.SetShaderResource(slot, view);
                vertexFound = true;
            }

            if (resourcesPixel.TryGetValue(name, out slot))
            {
                graphicsDevice.Context.PixelShader.SetSampler(slot, sampler); // TODO (henrik1235) Test, entfernen und mit ISampler ersetzen
                graphicsDevice.Context.PixelShader.SetShaderResource(slot, view);
                pixelFound = true;
            }

            if (!vertexFound && !pixelFound)
                Log.Warning("Shader.SetTexture({0}) did nothing because the variable name does not exist in the shader.", name);
        }
    }
}
