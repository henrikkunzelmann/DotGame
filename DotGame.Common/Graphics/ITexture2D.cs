using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt eine zweidimensionale Texture dar.
    /// </summary>
    public interface ITexture2D : IDisposable
    {
        /// <summary>
        /// Die Breite der Texture.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Die Höhe der Texture.
        /// </summary>
        int Height { get; }
    }
}
