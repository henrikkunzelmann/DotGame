using DotGame.EntitySystem.Components;
using DotGame.Graphics;
using DotGame.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Rendering
{
    public class ForwardPass : Pass
    {
        public Scene Scene { get; private set; }

        private List<IRenderItem> items = new List<IRenderItem>();

        public ForwardPass(Engine engine, Scene scene) : base(engine, new ForwardShader(engine))
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            this.Scene = scene;
        }

        public override void Apply(GameTime gameTime)
        {
            var cameras = Scene.GetComponents<Camera>();
            foreach (var camera in Scene.GetComponents<Camera>())
            {
                if (!camera.IsEnabled)
                    continue;

                Scene.CurrentCamera = camera;
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

                items.Clear();
                Scene.Invoke("PrepareDraw", false, gameTime, items);

                for (int i = 0; i < items.Count; i++)
                    items[i].Draw(gameTime, this, Engine.GraphicsDevice.RenderContext);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Shader != null)
                Shader.Dispose();
        }
    }
}
