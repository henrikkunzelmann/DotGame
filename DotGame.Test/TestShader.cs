using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Assets;
using DotGame.Graphics;
using DotGame.Rendering;
using DotGame.Cameras;

namespace DotGame.Test
{
    public class TestShader : Shader
    {
        private IShader shader;
        private IConstantBuffer constantBuffer;
        private ISampler sampler;
        private IRasterizerState rasterizerState;

        public TestShader(Engine engine)
            : base(engine, "TestShader")
        {
            shader = engine.ShaderManager.CompileShader("shader");

            constantBuffer = shader.CreateConstantBuffer(BufferUsage.Dynamic);
            sampler = engine.GraphicsDevice.Factory.CreateSampler(new SamplerInfo(TextureFilter.Linear));
            rasterizerState = engine.GraphicsDevice.Factory.CreateRasterizerState(new RasterizerStateInfo()
                {
                    IsFrontCounterClockwise = true,
                    FillMode = FillMode.Solid
                });
        }

        public override void Apply(Pass pass, IRenderContext context, Material material, Matrix world)
        {
            context.SetState(Engine.RenderStatePool.GetRenderState(new RenderStateInfo()
                {
                    PrimitiveType = PrimitiveType.TriangleList,
                    Shader = shader,
                    Rasterizer = rasterizerState
                }));
            context.SetTexture(shader, "picture", material.Texture.Handle);
            if (Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                context.SetSampler(shader, "pictureSampler", sampler);
            else
                context.SetSampler(shader, "picture", sampler);
            context.Update(constantBuffer, Matrix.Transpose(world * pass.Scene.Camera.View * pass.Scene.Camera.Projection));
            context.SetConstantBuffer(shader, constantBuffer);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (shader != null)
                shader.Dispose();
            if (constantBuffer != null)
                constantBuffer.Dispose();
            if (sampler != null)
                sampler.Dispose();
            if (rasterizerState != null)
                rasterizerState.Dispose();
        }
    }
}
