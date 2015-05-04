using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering.Passes
{
    /// <summary>
    /// Stellt einen Pass dar, welcher Partikel zeichnet.
    /// </summary>
    public class ParticlePass : Pass
    {
        public ParticlePass(Engine engine)
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
