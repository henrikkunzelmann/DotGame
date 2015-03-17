using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt einen VertexBuffer dar.
    /// </summary>
    public interface IVertexBuffer : IGraphicsObject
    {
        /// <summary>
        /// Die Anzahl aller Vertices in diesem VertexBuffer.
        /// </summary>
        int VertexCount { get; }
    }
}
