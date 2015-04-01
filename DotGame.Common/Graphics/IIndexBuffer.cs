using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt einen IndexBuffer dar.
    /// </summary>
    public interface IIndexBuffer : IGraphicsObject
    {
        /// <summary>
        /// Gibt die Anzahl der Indices in diesem IndexBuffer an.
        /// </summary>
        int IndexCount { get; }

        /// <summary>
        /// Gibt die Größe des IndexBuffers in Bytes an.
        /// </summary>
        int SizeBytes { get; }
    }
}
