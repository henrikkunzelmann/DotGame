using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.OpenGL4
{
    public class GraphicsDevice : IGraphicsDevice
    {
        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {

        }
    }
}
