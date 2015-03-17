using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.DirectX11
{
    public class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public void SwapBuffers()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public void Clear(Color color)
        {
            throw new NotImplementedException();
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }
    }
}
