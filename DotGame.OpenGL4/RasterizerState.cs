using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;

namespace DotGame.OpenGL4
{
    public class RasterizerState : GraphicsObject, IRasterizerState
    {
        public RasterizerStateInfo Info { get; private set; }

        public RasterizerState(GraphicsDevice graphicsdevice, RasterizerStateInfo info)
            :base(graphicsdevice, new StackTrace(1))
        {
            this.Info = info;
        }

        protected override void Dispose(bool isDisposing)
        {
            //Nothing to dispose
        }
    }
}
