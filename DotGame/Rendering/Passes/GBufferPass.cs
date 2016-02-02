using DotGame.Assets;
using DotGame.EntitySystem.Components;
using DotGame.EntitySystem.Rendering;
using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering.Passes
{
    /// <summary>
    /// Stellt einen Pass dar, welcher ForwardShading als RenderTechnik nutzt.
    /// </summary>
    public class GBufferPass : ScenePass
    {
        public IRenderTarget2D[] RenderTargets
        {
            get;
            private set;
        } = new IRenderTarget2D[2];

        public IRenderTarget2D DepthRenderTarget
        {
            get; private set;
        }

        public GBufferPass(Engine engine, Scene scene, AssetManager manager) : base(engine, scene)
        {
            AddShader(new GBufferShader(engine));

            RenderTargets[0] = Engine.GraphicsDevice.Factory.CreateRenderTarget2D(1280, 720, TextureFormat.RGBA8_UIntNorm, false);
            RenderTargets[1] = Engine.GraphicsDevice.Factory.CreateRenderTarget2D(1280, 720, TextureFormat.RGBA8_UIntNorm, false);
            DepthRenderTarget = Engine.GraphicsDevice.Factory.CreateRenderTarget2D(1280, 720, TextureFormat.Depth16, false);
        }

        public override void Render(GameTime gameTime)
        {
            var models = Scene.Root.GetComponents<StaticModel>(true);

            if (Scene.Camera != null && models != null)
            {
                if (Scene.Camera == null || !Scene.Camera.IsEnabled)
                    return;

                //GraphicsDevice.RenderContext.SetRenderTargets(DepthRenderTarget, RenderTargets);

                //Scene.CurrentCamera = camera;
                switch (Scene.Camera.ClearMode)
                {
                    case CameraClearMode.Nothing:
                        break;
                    case CameraClearMode.Color:
                        Engine.GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Scene.Camera.ClearColor, Scene.Camera.ClearDepth, 0);
                        break;
                    case CameraClearMode.Depth:
                        Engine.GraphicsDevice.RenderContext.Clear(ClearOptions.Depth, Color.Black, Scene.Camera.ClearDepth, 0);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
                foreach (var model in models)
                {
                    if (model == null || model.Material == null || model.Mesh == null)
                        continue;
                    
                    var materialDescription = model.Material.CreateDescription();
                    SceneShader shader;
                    if ((shader = GetShader(model.Mesh.VertexBufferHandle?.Description, materialDescription)) != null)
                    {
                        shader.Apply(GraphicsDevice.RenderContext, Scene.Camera.ViewProjection, model.Material, model.Entity.Transform.Matrix);
                        model.Mesh.Draw(GraphicsDevice.RenderContext);
                    }
                }
            }
        }
    }
}
