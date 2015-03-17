using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame
{
    /// <summary>
    /// Ein bestimmer Bereich in dem die Engine zeichnen kann, z.B. ein normales Fenster oder ein Teil davon.
    /// </summary>
    public interface IGameWindow
    {
        IGraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Stellt die Breite des Engine-Fensters dar.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Stellt die Höhe des Engine-Fensters dar.
        /// </summary>
        int Height { get; set; }
    }
}
