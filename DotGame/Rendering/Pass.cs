using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    public abstract class Pass : EngineObject, IDisposable
    {
        public PassPipeline Pipeline { get; private set; }
        public IGraphicsDevice GraphicsDevice { get { return Engine.GraphicsDevice; } }

        public Scene Scene { get; private set; }
        public Shader DefaultShader { get; private set; }

        public Pass(Engine engine, PassPipeline pipeline, Scene scene, Shader defaultShader)
            : base(engine)
        {
            if (pipeline == null)
                throw new ArgumentNullException("pipeline");
            if (scene == null)
                throw new ArgumentNullException("scene");
            if (defaultShader == null)
                throw new ArgumentNullException("defaultShader");

            this.Pipeline = pipeline;
            this.Scene = scene;
            this.DefaultShader = defaultShader;
        }

        public abstract void Render(GameTime gameTime);
    }
}
