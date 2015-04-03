using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public interface IConstantBuffer : IGraphicsObject
    {
        /// <summary>
        /// Die Größe in Bytes.
        /// </summary>
        int Size { get; }
    }
}
