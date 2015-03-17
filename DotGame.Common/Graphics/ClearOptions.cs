using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Definiert die Channel, die bei Clear* Operationen angesprochen werden.
    /// </summary>
    /// <seealso cref="IGraphicsContext.Clear(ClearOptions, Color, float, int)"/>
    [Flags]
    public enum ClearOptions : byte
    {
        /// <summary>
        /// Der Farbchannel.
        /// </summary>
        Color = 1,
        /// <summary>
        /// Der Tiefenchannel.
        /// </summary>
        Depth = 2,
        /// <summary>
        /// = Color | Depth
        /// </summary>
        ColorDepth = Color | Depth,
        /// <summary>
        /// Der Stencilchannel.
        /// </summary>
        Stencil = 4,
        /// <summary>
        /// = Color | Depth | Stencil
        /// </summary>
        ColorDepthStencil = Color | Depth | Stencil
    }
}
