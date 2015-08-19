using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering.Passes
{
    /// <summary>
    /// Stellt einen Pass dar, welcher für das Zeichnen von UI Elementen zuständig ist.
    /// </summary>
    public class UIPass : Pass
    {
        public UIPass(Engine engine)
            : base(engine, null)
        {

        }

        public override void Render(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }
    }
}
