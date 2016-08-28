using System.Runtime.InteropServices;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    [StructLayout(LayoutKind.Sequential, Size = 124)]
    internal struct Header
    {
        /// <summary>
        /// Size of the structure. This member must be set to 124.
        /// </summary>
        private uint headerSize;

        /// <summary>
        /// Size of the structure. This member must be set to 124.
        /// </summary>
        public uint HeaderSize
        {
            get { return headerSize; }
        }


        /// <summary>
        /// Flags to indicate which members contain valid data.
        /// </summary>
        private Flags flags;

        /// <summary>
        /// Flags to indicate which members contain valid data.
        /// </summary>
        public Flags Flags
        {
            get { return flags; }
        }


        /// <summary>
        /// Surface height (in pixels).
        /// </summary>
        private uint height;

        /// <summary>
        /// Surface height (in pixels).
        /// </summary>
        public uint Height
        {
            get { return height; }
        }


        /// <summary>
        /// Surface width (in pixels).
        /// </summary>
        private uint width;

        /// <summary>
        /// Surface width (in pixels).
        /// </summary>
        public uint Width
        {
            get { return width; }
        }


        /// <summary>
        /// The total number of bytes in the top level texture for a compressed texture or the number of bytes per scan line in an uncompressed texture.
        /// </summary>
        private uint pitchOrLinearSize;

        /// <summary>
        /// The total number of bytes in the top level texture for a compressed texture or the number of bytes per scan line in an uncompressed texture.
        /// </summary>
        public uint PitchOrLinearSize
        {
            get { return pitchOrLinearSize; }
        }


        /// <summary>
        /// The number of bytes per scan line in an uncompressed texture.
        /// </summary>
        public uint LinearSize
        {
            get
            {
                if ((Flags & Flags.LinearSize) == Flags.LinearSize)
                {
                    return pitchOrLinearSize;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Depth of a volume texture (in pixels), otherwise unused.
        /// </summary>
        private uint depth;

        /// <summary>
        /// Depth of a volume texture (in pixels), otherwise unused.
        /// </summary>
        public uint Depth
        {
            get { return depth; }
        }


        /// <summary>
        /// Number of mipmap levels, otherwise unused.
        /// </summary>
        private uint mipMapCount;

        /// <summary>
        /// Number of mipmap levels, otherwise unused.
        /// </summary>
        public uint MipMapCount
        {
            get { return mipMapCount; }
        }


        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        uint[] reserved;


        /// <summary>
        /// The pixel format
        /// </summary>
        private PixelFormat pixelFormat;

        /// <summary>
        /// The pixel format
        /// </summary>
        public PixelFormat PixelFormat
        {
            get { return pixelFormat; }
        }


        /// <summary>
        /// Specifies the complexity of the surfaces stored.
        /// </summary>
        private Caps caps;

        /// <summary>
        /// Specifies the complexity of the surfaces stored.
        /// </summary>
        public Caps Caps
        {
            get { return caps; }
        }


        /// <summary>
        /// Additional detail about the surfaces stored.
        /// </summary>
        private Caps2 caps2;

        /// <summary>
        /// Additional detail about the surfaces stored.
        /// </summary>
        public Caps2 Caps2
        {
            get { return caps2; }
        }


        /// <summary>
        /// Unused
        /// </summary>
        private uint caps3;
        /// <summary>
        /// Ununsed
        /// </summary>
        private uint caps4;
        /// <summary>
        /// Unused
        /// </summary>
        private uint reserved2;
    }
}
