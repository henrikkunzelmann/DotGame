using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class RenderState : GraphicsObject, IRenderState
    {
        public RenderStateInfo Info { get; private set; }

        public RenderState(GraphicsDevice graphicsDevice, RenderStateInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            if (info.Shader == null)
                throw new ArgumentException("Shader of Info is null.", "info");

            this.Info = info;
        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
