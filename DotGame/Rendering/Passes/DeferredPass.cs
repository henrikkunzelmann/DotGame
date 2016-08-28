using System;

namespace DotGame.Rendering.Passes
{
    /// <summary>
    /// Stellt einen Pass dar, welcher DeferredShading als Rendertechnik benutzt.
    /// </summary>
    public class DeferredPass : Pass
    {
        public DeferredPass(Engine engine)
            : base(engine)
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
