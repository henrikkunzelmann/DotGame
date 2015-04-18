using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    public class RenderStatePool : EngineObject
    {
        private Dictionary<RenderStateInfo, IRenderState> renderStates = new Dictionary<RenderStateInfo, IRenderState>();

        public RenderStatePool(Engine engine)
            : base(engine)
        {

        }

        public IRenderState GetRenderState(RenderStateInfo info)
        {
            lock (renderStates)
            {
                IRenderState rs;
                if (renderStates.TryGetValue(info, out rs))
                    return rs;
                rs = Engine.GraphicsDevice.Factory.CreateRenderState(info);
                renderStates[info] = rs;
                return rs;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            lock (renderStates)
            {
                foreach (KeyValuePair<RenderStateInfo, IRenderState> state in renderStates)
                    state.Value.Dispose();
            }
        }
    }
}
