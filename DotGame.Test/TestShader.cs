using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.Test
{
    public class TestShader : Shader
    {
        private ISampler sampler;
        private IConstantBuffer constantBuffer;
        private IRasterizerState rasterizerState;
        private IDepthStencilState depthStencil;

        public TestShader(Engine engine)
            : base(engine, "shader")
        {
            sampler = engine.GraphicsDevice.Factory.CreateSampler(new SamplerInfo(TextureFilter.Linear));
            constantBuffer = shader.CreateConstantBuffer(BufferUsage.Dynamic);

            rasterizerState = engine.GraphicsDevice.Factory.CreateRasterizerState(new RasterizerStateInfo()
            {
                CullMode = CullMode.Front,
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

        public override void Apply(IRenderContext context)
        {
            throw new NotImplementedException();
        }

        public override void Apply(IRenderContext context, Matrix4x4 viewProjection, Assets.Material material, Matrix4x4 world)
        {
            context.SetState(Engine.RenderStatePool.GetRenderState(new RenderStateInfo()
            {
                PrimitiveType = Graphics.PrimitiveType.TriangleList,
                Shader = shader,
                Rasterizer = rasterizerState,
                DepthStencil = depthStencil
            }));
            context.SetTexture(shader, "picture", material.Texture.Handle);
            if (Engine.Settings.GraphicsAPI == GraphicsAPI.Direct3D11)
                context.SetSampler(shader, "pictureSampler", sampler);
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                context.SetSampler(shader, "picture", sampler);

            context.Update(constantBuffer, Matrix4x4.Transpose(world * viewProjection));
            context.SetConstantBuffer(shader, constantBuffer);
        }
    }
}
