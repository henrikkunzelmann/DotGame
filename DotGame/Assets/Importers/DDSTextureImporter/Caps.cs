using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    [Flags]
    internal enum Caps : uint
    {
        /// <summary>
        /// Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
        /// </summary>
        Complex = 0x8,
        /// <summary>
        /// Optional; should be used for a mipmap.	
        /// </summary>
        MipMap = 0x400000,
        /// <summary>
        /// Required
        /// </summary>
        Texture = 0x1000,
    }
}
