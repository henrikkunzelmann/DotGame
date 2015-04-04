using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Gibt das Format (die Bit-Tiefe) von Sampledaten an.
    /// </summary>
    public enum AudioFormat : byte
    {
        /// <summary>
        /// Ein Sample entspricht 1 byte.
        /// </summary>
        Byte8,

        /// <summary>
        /// Ein Sample entspricht 2 byte.
        /// </summary>
        Short16,

        /// <summary>
        /// Ein Sample entspricht 4 byte.
        /// </summary>
        Float32,
    }
}
