using System.Runtime.InteropServices;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HeaderDXT10
    {
        /// <summary>
        /// The surface pixel format
        /// </summary>
        Format format;

        /// <summary>
        /// The surface pixel format
        /// </summary>
        public Format Format { get { return format; } }


        /// <summary>
        /// Identifies the type of resource.
        /// </summary>
        ResourceDimension resourceDimension;

        /// <summary>
        /// Identifies the type of resource.
        /// </summary>
        public ResourceDimension ResourceDimension { get { return resourceDimension; } }


        /// <summary>
        /// Identifies other, less common options for resources. 
        /// </summary>
        DXT10MiscFlag miscFlag;

        /// <summary>
        /// Identifies other, less common options for resources. 
        /// </summary>
        public DXT10MiscFlag MiscFlag { get { return miscFlag; } }


        /// <summary>
        /// The number of elements in the array.
        ///For a 2D texture that is also a cube-map texture, this number represents the number of cubes. This number is the same as the number in the NumCubes member of D3D10_TEXCUBE_ARRAY_SRV1 or D3D11_TEXCUBE_ARRAY_SRV). In this case, the DDS file contains arraySize*6 2D textures. For more information about this case, see the miscFlag description.
        ///For a 3D texture, you must set this number to 1.
        /// </summary>
        uint arraySize;

        /// <summary>
        /// The number of elements in the array.
        ///For a 2D texture that is also a cube-map texture, this number represents the number of cubes. This number is the same as the number in the NumCubes member of D3D10_TEXCUBE_ARRAY_SRV1 or D3D11_TEXCUBE_ARRAY_SRV). In this case, the DDS file contains arraySize*6 2D textures. For more information about this case, see the miscFlag description.
        ///For a 3D texture, you must set this number to 1.
        /// </summary>
        public uint ArraySize { get { return arraySize; } }


        /// <summary>
        /// Contains additional metadata (formerly was reserved). The lower 3 bits indicate the alpha mode of the associated resource. The upper 29 bits are reserved and are typically 0.
        /// </summary>
        DXT10MiscFlags2 miscFlags2;

        /// <summary>
        /// Contains additional metadata (formerly was reserved). The lower 3 bits indicate the alpha mode of the associated resource. The upper 29 bits are reserved and are typically 0.
        /// </summary>
        public DXT10MiscFlags2 MiscFlags2 { get { return miscFlags2; } }
    }
}
