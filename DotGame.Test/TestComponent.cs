using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotGame.Graphics;
using DotGame.Utils;
using DotGame.Audio;
using System.Numerics;

namespace DotGame.Test
{
    public class TestComponent : GameComponent
    {
        public ISoundInstance Visualize;

        private IRenderTarget2D colorTarget;
        private IRenderTarget2D depthTarget;
        private IRasterizerState rasterizerState;
        private IShader shader;
        private IConstantBuffer constantBuffer;
        private IVertexBuffer vertexBuffer;
        private IVertexBuffer quad;
        private ITexture2D texture;
        private ISampler sampler;
        private IRenderState state;
        private IDepthStencilState depthStencil;

        public TestComponent(Engine engine)
            : base(engine)
        {

        }

        public override void Init()
        {
            Log.Debug("TestComponent.Init()");
            texture = GraphicsDevice.Factory.LoadTexture2D("GeneticaMortarlessBlocks.jpg", true);

            if (Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                shader = GraphicsDevice.Factory.CompileShader("testShader", new ShaderCompileInfo("shader.fx", "VS", "vs_4_0"), new ShaderCompileInfo("shader.fx", "PS", "ps_4_0"));
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                shader = GraphicsDevice.Factory.CompileShader("testShader", new ShaderCompileInfo("shader.vertex.glsl", "", "vs_4_0"), new ShaderCompileInfo("shader.fragment.glsl", "", "ps_4_0"));
            else
                throw new NotImplementedException();

            colorTarget = GraphicsDevice.Factory.CreateRenderTarget2D(GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height, TextureFormat.RGBA32_Float, false);
            depthTarget = GraphicsDevice.Factory.CreateRenderTarget2D(GraphicsDevice.DefaultWindow.Width, GraphicsDevice.DefaultWindow.Height, TextureFormat.Depth32, false);

            constantBuffer = shader.CreateConstantBuffer(BufferUsage.Dynamic);

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
            }, Geometry.VertexPositionTexture.Description, BufferUsage.Static);


            quad = GraphicsDevice.Factory.CreateVertexBuffer(new float[] {
                // 3D coordinates              UV Texture coordinates                 
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f, // Front
                -1.0f,  1.0f, -1.0f,    0.0f, 0.0f,
                 -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
                 
                 1.0f, -1.0f, -1.0f,    1.0f, 1.0f,
                 1.0f,  1.0f, -1.0f,    1.0f, 0.0f,
                -1.0f, -1.0f, -1.0f,    0.0f, 1.0f,
            }, Geometry.VertexPositionTexture.Description, BufferUsage.Static);


            sampler = GraphicsDevice.Factory.CreateSampler(new SamplerInfo(TextureFilter.Linear));

            rasterizerState = GraphicsDevice.Factory.CreateRasterizerState(new RasterizerStateInfo()
                {
                    //CullMode = CullMode.None,
                    FillMode = FillMode.Solid,
                    IsFrontCounterClockwise = false,
                });

            depthStencil = GraphicsDevice.Factory.CreateDepthStencilState(new DepthStencilStateInfo()
            {
                IsDepthEnabled = true,
                DepthComparsion = Comparison.LessEqual,
                DepthWriteMask = DepthWriteMask.All
            });


            state = GraphicsDevice.Factory.CreateRenderState(new RenderStateInfo()
            {
                PrimitiveType = PrimitiveType.TriangleList,
                Rasterizer = rasterizerState,
                Shader = shader,
                //DepthStencil = depthStencil
            });

            Log.FlushBuffer();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

            float time = (float)gameTime.TotalTime.TotalSeconds;
            var view = Matrix4x4.CreateLookAt(new Vector3(0, 0, 5f), new Vector3(0, 0, 0), Vector3.UnitY);
            var proj = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.PI / 4f, GraphicsDevice.DefaultWindow.Width / (float)GraphicsDevice.DefaultWindow.Height, 0.1f, 100.0f);
            var worldViewProj = Matrix4x4.Transpose(
                  Matrix4x4.CreateScale(Visualize != null ? Visualize.Peak : 1.0f)
                * Matrix4x4.CreateRotationX(time)
                * Matrix4x4.CreateRotationY(time * 2)
                * Matrix4x4.CreateRotationZ(time * .7f) * view * proj);
            

            //GraphicsDevice.RenderContext.SetRenderTargetsBackBuffer();
            GraphicsDevice.RenderContext.SetRenderTargetsColor(colorTarget);
            GraphicsDevice.RenderContext.SetRenderTargetsDepth(depthTarget); 
            GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Color.CornflowerBlue, 1f, 0);

            GraphicsDevice.RenderContext.SetState(state);
            GraphicsDevice.RenderContext.SetConstantBuffer(shader, constantBuffer);
            GraphicsDevice.RenderContext.SetTexture(shader, "picture", texture);

            if(Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                GraphicsDevice.RenderContext.SetSampler(shader, "pictureSampler", sampler);
            else
                GraphicsDevice.RenderContext.SetSampler(shader, "picture", sampler);

            GraphicsDevice.RenderContext.Update(constantBuffer, worldViewProj);
            GraphicsDevice.RenderContext.SetVertexBuffer(vertexBuffer);

            GraphicsDevice.RenderContext.Draw();
            
            GraphicsDevice.RenderContext.SetRenderTargetsBackBuffer();
            GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Color.Beige, 1f, 0);
            GraphicsDevice.RenderContext.SetState(state);
            GraphicsDevice.RenderContext.SetVertexBuffer(quad);
            GraphicsDevice.RenderContext.Update(constantBuffer, Matrix4x4.Identity);
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

            if (state != null)
                state.Dispose();

            if (colorTarget != null)
                colorTarget.Dispose();

            if (depthTarget != null)
                depthTarget.Dispose();

            if (depthStencil != null)
                depthStencil.Dispose();
        }
    }
}