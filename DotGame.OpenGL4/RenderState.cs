using DotGame.Graphics;

namespace DotGame.OpenGL4
{
    public class RenderState : GraphicsObject, IRenderState
    {
        public RenderStateInfo Info { get; private set; }

        public RenderState(GraphicsDevice graphicsDevice, RenderStateInfo info)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            this.Info = info;
        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
