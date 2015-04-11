using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.Test
{
    public class TestPass : Pass
    {
        public TestPass(Engine engine, PassPipeline pipeline, Scene scene)
            : base(engine, pipeline, scene, new TestShader(engine))
        {
        }

        public override void Render(GameTime gameTime)
        {
            RenderItemCollection itemCollection = new RenderItemCollection();
            Scene.Draw(gameTime, itemCollection);

            RenderItem[] items = itemCollection.GetItems();
            for (int i = 0; i < items.Length; i++)
                items[i].Draw(gameTime, this, GraphicsDevice.RenderContext);
        }

        protected override void Dispose(bool isDisposing)
        {
            DefaultShader.Dispose();
        }
    }
}
