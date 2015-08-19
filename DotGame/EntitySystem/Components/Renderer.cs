using DotGame.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.EntitySystem.Components
{
    public abstract class Renderer : Component
    {
        /// <summary>
        /// Wird beim Zeichnen aufegrufen.
        /// </summary>
        public abstract void Draw(GameTime gameTime, Pass pass, IRenderContext context);
    }
}
