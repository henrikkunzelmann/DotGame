using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public interface ITextureArray : IGraphicsObject
    {
        /// <summary>
        /// Die Anzahl der Texturen in diesem TextureArray.
        /// </summary>
        int ArraySize { get; }
    }
}
