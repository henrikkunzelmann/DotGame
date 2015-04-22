using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    public abstract class RenderItem : EngineObject
    {
        public RenderItem(Engine engine)
            : base(engine)
        {
        }

        public abstract void Draw(GameTime gameTime, Pass pass, IRenderContext context);
    }
}
