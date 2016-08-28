using DotGame.Graphics;
using DotGame.Rendering;
using System.Numerics;
using System.Runtime.InteropServices;

namespace DotGame.EntitySystem.Rendering
{
    public class GBufferShader : SceneShader
    {
        private ISampler sampler;
        private IConstantBuffer constantBuffer;
        private IRasterizerState rasterizerState;
        private IDepthStencilState depthStencil;

        private MaterialDescription materialDescription = new MaterialDescription() { HasDiffuseTexture = true };
        public override MaterialDescription MaterialDescription
        {
            get
            {
                return materialDescription;
            }
        }

        public GBufferShader(Engine engine) : base(engine, @"Rendering\Passes\Shaders\gbuffer")
        {
            sampler = engine.GraphicsDevice.Factory.CreateSampler(new SamplerInfo(TextureFilter.Linear));
            constantBuffer = shader.CreateConstantBuffer(ResourceUsage.Dynamic);

            rasterizerState = engine.GraphicsDevice.Factory.CreateRasterizerState(new RasterizerStateInfo()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = false,
            });

            depthStencil = engine.GraphicsDevice.Factory.CreateDepthStencilState(new DepthStencilStateInfo()
            {
                IsDepthEnabled = true,
                DepthComparsion = Comparison.LessEqual,
                DepthWriteMask = DepthWriteMask.All
            });
        }

        public override void Apply(Graphics.IRenderContext context, System.Numerics.Matrix4x4 viewProjection, Assets.Material material, System.Numerics.Matrix4x4 world)
        {
            context.SetState(Engine.RenderStatePool.GetRenderState(new RenderStateInfo()
            {
                PrimitiveType = Graphics.PrimitiveType.TriangleList,
                Shader = shader,
                Rasterizer = rasterizerState,
                DepthStencil = depthStencil
            }));
            context.SetTexture(shader, "picture", material.Texture.Handle);
            if(Engine.Settings.GraphicsAPI == GraphicsAPI.Direct3D11)
                context.SetSampler(shader, "pictureSampler", sampler);
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                context.SetSampler(shader, "picture", sampler);

            GCHandle handle;
            DataArray dataArray = DataArray.FromObject<Matrix4x4>(Matrix4x4.Transpose(world * viewProjection), out handle);
            try
            {
                context.UpdateContext.Update(constantBuffer, dataArray);
            }
            finally
            {
                handle.Free();
            }
            context.SetConstantBuffer(shader, constantBuffer);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
        }
    }
}
