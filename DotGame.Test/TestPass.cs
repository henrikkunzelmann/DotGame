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
        private List<RenderItem> renderItems = new List<RenderItem>();

        public TestPass(Engine engine, PassPipeline pipeline, Scene scene)
            : base(engine, pipeline, scene, new TestShader(engine))
        {
        }

        public override void Render(GameTime gameTime)
        {
            renderItems.Clear();
            Scene.PrepareForDraw(gameTime, renderItems);

            for (int i = 0; i < renderItems.Count; i++)
                renderItems[i].Draw(gameTime, this, GraphicsDevice.RenderContext);
        }

        protected override void Dispose(bool isDisposing)
        {
            DefaultShader.Dispose();
        }
    }
}
