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

        private List<Renderer> items = new List<Renderer>();

        public ForwardPass(Engine engine, Scene scene) : base(engine, new ForwardShader(engine))
        {
            if (scene == null)
                throw new ArgumentNullException("scene");

            this.Scene = scene;
        }

        public override void Render(GameTime gameTime)
        {
            var cameras = Scene.Root.GetComponents(true, typeof(Camera)).Select(c => (Camera)c);
            foreach (var camera in cameras)
            {
                if (camera == null || !camera.IsEnabled)
                    continue;


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

                items.Clear();
                foreach (Renderer item in Scene.Root.GetComponents(true, typeof(Renderer)))
                    items.Add(item);

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
