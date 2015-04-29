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
        /// Die Größe des ConstantBuffers in Bytes.
        /// </summary>
        int SizeBytes { get; }

        /// <summary>
        /// Gibt an wie der ConstantBuffer benutzt wird.
        /// </summary>
        BufferUsage Usage { get; }
    }
}
