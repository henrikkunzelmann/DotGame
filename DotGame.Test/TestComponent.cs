using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using DotGame.Utils;
using DotGame.Audio;

namespace DotGame.Test
{
    public class TestComponent : GameComponent
    {
        public ISoundInstance Visualize;

        private IRenderTarget2D depthBuffer;
        private IRenderTarget2D colorTarget;
        private IRasterizerState rasterizerState;
        private IShader shader;
        private IConstantBuffer constantBuffer;
        private IVertexBuffer vertexBuffer;
        private IVertexBuffer quad;
        private ITexture2D texture;
        private ISampler sampler;

        public TestComponent(Engine engine)
            : base(engine)
        {

        }

        public override void Init()
        {
            texture = GraphicsDevice.Factory.LoadTexture2D("GeneticaMortarlessBlocks.jpg", true);

            if (Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                shader = GraphicsDevice.Factory.CompileShader("testShader", new ShaderCompileInfo("shader.fx", "VS", "vs_4_0"), new ShaderCompileInfo("shader.fx", "PS", "ps_4_0"));
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                shader = GraphicsDevice.Factory.CompileShader("testShader", new ShaderCompileInfo("shader.vertex.glsl", "", "vs_4_0"), new ShaderCompileInfo("shader.fragment.glsl", "", "ps_4_0"));
            else
                throw new NotImplementedException();

            depthBuffer = GraphicsDevice.Factory.CreateRenderTarget2D(GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height, TextureFormat.Depth24Stencil8, false);
            colorTarget = GraphicsDevice.Factory.CreateRenderTarget2D(GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height, TextureFormat.RGBA32_Float, false);

            constantBuffer = shader.CreateConstantBuffer();

            vertexBuffer = GraphicsDevice.Factory.CreateVertexBuffer(new float[] {
                // 3D coordinates              UV Texture coordinates
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f, // Front
                -1.0f,  1.0f, -1.0f,    0.0f, 0.0f,
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f,
                 1.0f, -1.0f, -1.0f,    1.0f, 1.0f,
                
                -1.0f, -1.0f,  1.0f,    1.0f, 0.0f, // BACK
                 1.0f,  1.0f,  1.0f,    0.0f, 1.0f,
                -1.0f,  1.0f,  1.0f,    1.0f, 1.0f,
                -1.0f, -1.0f,  1.0f,    1.0f, 0.0f,
                 1.0f, -1.0f,  1.0f,    0.0f, 0.0f,
                 1.0f,  1.0f,  1.0f,    0.0f, 1.0f,
                
                -1.0f, 1.0f, -1.0f,     0.0f, 1.0f, // Top
                -1.0f, 1.0f,  1.0f,     0.0f, 0.0f,
                 1.0f, 1.0f,  1.0f,     1.0f, 0.0f,
                -1.0f, 1.0f, -1.0f,     0.0f, 1.0f,
                 1.0f, 1.0f,  1.0f,     1.0f, 0.0f,
                 1.0f, 1.0f, -1.0f,     1.0f, 1.0f,
                
                -1.0f,-1.0f, -1.0f,     1.0f, 0.0f, // Bottom
                 1.0f,-1.0f,  1.0f,     0.0f, 1.0f,
                -1.0f,-1.0f,  1.0f,     1.0f, 1.0f,
                -1.0f,-1.0f, -1.0f,     1.0f, 0.0f,
                 1.0f,-1.0f, -1.0f,     0.0f, 0.0f,
                 1.0f,-1.0f,  1.0f,     0.0f, 1.0f,
                
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f, // Left
                -1.0f, -1.0f,  1.0f,    0.0f, 0.0f,
                -1.0f,  1.0f,  1.0f,    1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
                -1.0f,  1.0f,  1.0f,    1.0f, 0.0f,
                -1.0f,  1.0f, -1.0f,    1.0f, 1.0f,
                
                 1.0f, -1.0f, -1.0f,    1.0f, 0.0f, // Right
                 1.0f,  1.0f,  1.0f,    0.0f, 1.0f,
                 1.0f, -1.0f,  1.0f,    1.0f, 1.0f,
                 1.0f, -1.0f, -1.0f,    1.0f, 0.0f,
                 1.0f,  1.0f, -1.0f,    0.0f, 0.0f,
                 1.0f,  1.0f,  1.0f,    0.0f, 1.0f,
            }, Geometry.VertexPositionTexture.Description);


            quad = GraphicsDevice.Factory.CreateVertexBuffer(new float[] {
                // 3D coordinates              UV Texture coordinates                 
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f, // Front
                -1.0f,  1.0f, -1.0f,    0.0f, 0.0f,
                 -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
                 
                 1.0f, -1.0f, -1.0f,    1.0f, 1.0f,
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
            }, Geometry.VertexPositionTexture.Description);


            sampler = GraphicsDevice.Factory.CreateSampler(new SamplerInfo(TextureFilter.Linear));
            rasterizerState = GraphicsDevice.Factory.CreateRasterizerState(new RasterizerStateInfo()
                {
                    CullMode = CullMode.Back,
                    FillMode = FillMode.Solid,
                    IsFrontCounterClockwise = true
                });
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

            float time = (float)gameTime.TotalTime.TotalSeconds;
            var view = Matrix.CreateLookAt(new Vector3(0, 0, 5f), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PI / 4f, GraphicsDevice.DefaultWindow.Width / (float)GraphicsDevice.DefaultWindow.Height, 0.1f, 100.0f);
            var worldViewProj =
                  Matrix.CreateScale(Visualize != null ? Visualize.Peak : 1.0f)
                * Matrix.CreateRotationX(time)
                * Matrix.CreateRotationY(time * 2)
                * Matrix.CreateRotationZ(time * .7f) * view * proj;
            worldViewProj.Transpose();

            GraphicsDevice.RenderContext.SetRenderTarget(depthBuffer, colorTarget);
            GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Color.CornflowerBlue, 1f, 0);

            GraphicsDevice.RenderContext.SetRasterizer(rasterizerState);

            GraphicsDevice.RenderContext.SetShader(shader);
            GraphicsDevice.RenderContext.SetConstantBuffer(shader, constantBuffer);
            GraphicsDevice.RenderContext.SetTexture(shader, "picture", texture);

            if(Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                GraphicsDevice.RenderContext.SetSampler(shader, "pictureSampler", sampler);
            else
                GraphicsDevice.RenderContext.SetSampler(shader, "picture", sampler);

            GraphicsDevice.RenderContext.Update(constantBuffer, worldViewProj);
            GraphicsDevice.RenderContext.SetPrimitiveType(PrimitiveType.TriangleList);
            GraphicsDevice.RenderContext.SetVertexBuffer(vertexBuffer);

            GraphicsDevice.RenderContext.Draw();
            
            GraphicsDevice.RenderContext.SetRenderTargetBackBuffer();
            GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Color.Beige, 1f, 0);
            GraphicsDevice.RenderContext.SetVertexBuffer(quad);
            GraphicsDevice.RenderContext.Update(constantBuffer, Matrix.Identity);
            GraphicsDevice.RenderContext.SetTexture(shader, "picture", colorTarget);
            GraphicsDevice.RenderContext.Draw();

            GraphicsDevice.RenderContext.SetTextureNull(shader, "picture");
        }

        public override void Unload()
        {
            if (rasterizerState != null)
                rasterizerState.Dispose();

            if (shader != null)
                shader.Dispose();

            if (constantBuffer != null)
                constantBuffer.Dispose();

            if (vertexBuffer != null)
                vertexBuffer.Dispose();

            if (texture != null)
                texture.Dispose();

            if (sampler != null)
                sampler.Dispose();
        }
    }
}