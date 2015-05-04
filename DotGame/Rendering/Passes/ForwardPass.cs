using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering.Passes
{
    /// <summary>
    /// Stellt einen Pass dar, welcher ForwardShading als RenderTechnik nutzt.
    /// </summary>
    public class ForwardPass : Pass
    {
        public ForwardPass(Engine engine)
            : base(engine)
        {

        }

        public override void Apply(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }
    }
}
