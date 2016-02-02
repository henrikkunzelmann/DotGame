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
    public class ForwardPass : ScenePass
    {
        public ForwardPass(Engine engine, Scene scene)
            : base(engine, scene)
        {
            AddShader(new ForwardShader(engine));
        }

        public override void Render(GameTime gameTime)
        {
            var models = Scene.Root.GetComponents<StaticModel>(true);
            var camera = Scene.Root.GetComponents<Camera>(true)?.First();

            if (camera != null && models != null)
            {
                if (camera == null || !camera.IsEnabled)
                    return;

                //Scene.CurrentCamera = camera;
                switch (camera.ClearMode)
                {
                    case CameraClearMode.Nothing:
                        break;
                    case CameraClearMode.Color:
                        Engine.GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, camera.ClearColor, camera.ClearDepth, 0);
                        break;
                    case CameraClearMode.Depth:
                        Engine.GraphicsDevice.RenderContext.Clear(ClearOptions.Depth, Color.Black, camera.ClearDepth, 0);
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
                        shader.Apply(GraphicsDevice.RenderContext, camera.ViewProjection, model.Material, model.Entity.Transform.Matrix);
                        model.Mesh.Draw(GraphicsDevice.RenderContext);
                    }
                }
            }
        }
    }
}
