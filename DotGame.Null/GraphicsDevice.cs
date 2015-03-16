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
        public IGraphicsFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {

        }
    }
}
