using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Assets;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.Test
{
    public class Test2Component : GameComponent
    {

        private Scene scene;
        private PassPipeline pipeline;
        private TestCube cube;

        public Test2Component(Engine engine)
            : base(engine)
        {

        }

        public override void Init()
        {
            scene = new Scene(Engine);

            pipeline = new PassPipeline(Engine);
            pipeline.AddPass(new TestPass(Engine, pipeline, scene));

            cube = new TestCube(Engine);
            scene.Nodes.Add(cube);

            scene.Camera = new TestCamera(Engine);
        }

        public override void Update(GameTime gameTime)
        {
            scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.RenderContext.Clear(ClearOptions.ColorDepth, Color.LightSkyBlue, 1f, 0);

            pipeline.Draw(gameTime);
        }

        public override void Unload()
        {
            scene.Dispose();
            pipeline.Dispose();
        }
    }
}
