using DotGame.EntitySystem.Rendering;
using DotGame.Rendering;
using DotGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem
{
    public class EntitySystemComponent : GameComponent
    {
        public readonly Scene Scene;
        public readonly PassPipeline PassPipeline;

        public EntitySystemComponent(Engine engine) : base(engine)
        {
            Scene = new Scene(engine);
            PassPipeline = new PassPipeline(Engine, new ForwardPass(engine, Scene));
        }

        public override void Init()
        {
            Scene.Invoke("Init", false);
        }

        public override void Update(GameTime gameTime)
        {
            Scene.Invoke("Update", false, gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Scene.Invoke("Draw", false, gameTime);

            foreach (var pass in PassPipeline.Passes)
                pass.Apply(gameTime);
        }

        public override void Unload()
        {
            // TODO (Joex3): Szene serialisieren.
            PassPipeline.Dispose();
        }
    }
}
