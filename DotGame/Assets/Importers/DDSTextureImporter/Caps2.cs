using System;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    [Flags]
    internal enum Caps2 : uint
    {
        /// <summary>
        /// Required for a cube map.	
        /// </summary>
        CubeMap = 0x200,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapPositiveX = 0x400,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapNegativeX = 0x800,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapPositiveY = 0x1000,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapNegativeY = 0x2000,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapPositiveZ = 0x4000,
        /// <summary>
        /// Required when these surfaces are stored in a cube map.	
        /// </summary>
        CubeMapNegativeZ = 0x8000,

        /// <summary>
        /// All Surfaces are stored in a cubemap.
        /// Although Direct3D 9 supports partial cube-maps, Direct3D 10, 10.1, and 11 require that you define all six cube-map faces
        /// </summary>
        CubeMapAllFaces = CubeMapPositiveX | CubeMapNegativeX | CubeMapPositiveY | CubeMapNegativeY | CubeMapPositiveZ | CubeMapNegativeZ,

        /// <summary>
        /// Required for a volume texture.	
        /// </summary>
        Volume = 0x200000,
    }
}
