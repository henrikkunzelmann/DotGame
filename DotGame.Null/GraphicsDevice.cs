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
        public bool IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear(Color color)
        {
            throw new NotImplementedException();
        }

        public void Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }

        public void SwapBuffers()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        bool IGraphicsDevice.IsDisposed
        {
            get { throw new NotImplementedException(); }
        }

        IGraphicsFactory IGraphicsDevice.Factory
        {
            get { throw new NotImplementedException(); }
        }

        void IGraphicsDevice.Clear(Color color)
        {
            throw new NotImplementedException();
        }

        void IGraphicsDevice.Clear(ClearOptions clearOptions, Color color, float depth, int stencil)
        {
            throw new NotImplementedException();
        }

        void IGraphicsDevice.SwapBuffers()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
