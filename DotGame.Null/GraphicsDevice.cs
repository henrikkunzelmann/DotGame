using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Null
{
    /// <summary>
    /// GraphicsDevice welches keinerlei realle Wirkung hat, nützlich um z.B. nur mit dem Asset System zu arbeiten.
    /// </summary>
    public class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }

        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public IGameWindow DefaultWindow
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear(Color color)
        {
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
        }

        public void SwapBuffers()
        {
        }

        public void Dispose()
        {
            // wir haben nichts wirklich zu disposen
            IsDisposed = true;
        }

        public bool VSync
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
