using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size=32)]
    internal struct PixelFormat
    {
        /// <summary>
        /// Structure size; set to 32 (bytes).
        /// </summary>
        private uint size;

        /// <summary>
        /// Structure size; set to 32 (bytes).
        /// </summary>
        public uint Size
        {
            get { return size; }
        }


        /// <summary>
        /// Values which indicate what type of data is in the surface.
        /// </summary>
        private PixelFormatFlags flags;

        /// <summary>
        /// Values which indicate what type of data is in the surface.
        /// </summary>
        public PixelFormatFlags Flags
        {
            get { return flags; }
        }


        /// <summary>
        /// Four-character codes for specifying compressed or custom formats. Possible values include: DXT1, DXT2, DXT3, DXT4, or DXT5. A FourCC of DX10 indicates the prescense of the DDS_HEADER_DXT10 extended header, and the dxgiFormat member of that structure indicates the true format. When using a four-character code, dwFlags must include DDPF_FOURCC.
        /// </summary>
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst=4)]
        //private string fourCC;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] fourCCBytes;

        /// <summary>
        /// Four-character codes for specifying compressed or custom formats. Possible values include: DXT1, DXT2, DXT3, DXT4, or DXT5. A FourCC of DX10 indicates the prescense of the DDS_HEADER_DXT10 extended header, and the dxgiFormat member of that structure indicates the true format. When using a four-character code, dwFlags must include DDPF_FOURCC.
        /// </summary>
        public string FourCC
        {
            get { //return fourCC; 
                return Encoding.ASCII.GetString(fourCCBytes).Trim();
            }
        }


        /// <summary>
        /// Number of bits in an RGB (possibly including alpha) format. Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.
        /// </summary>
        private uint rGBBitCount;

        /// <summary>
        /// Number of bits in an RGB (possibly including alpha) format. Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.
        /// </summary>
        public uint RGBBitCount
        {
            get { return rGBBitCount; }
        }


        /// <summary>
        /// Red (or lumiannce or Y) mask for reading color data. For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.
        /// </summary>
        private uint rBitMask;

        /// <summary>
        /// Red (or lumiannce or Y) mask for reading color data. For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.
        /// </summary>
        public uint RBitMask { get { return rBitMask; } }


        /// <summary>
        /// Green (or U) mask for reading color data. For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.
        /// </summary>
        private uint gBitMask;

        /// <summary>
        /// Green (or U) mask for reading color data. For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.
        /// </summary>
        public uint GBitMask { get { return gBitMask; } }


        /// <summary>
        /// Blue (or V) mask for reading color data. For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.
        /// </summary>
        private uint bBitMask;

        /// <summary>
        /// Blue (or V) mask for reading color data. For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.
        /// </summary>
        public uint BBitMask { get { return bBitMask; } }


        /// <summary>
        /// Alpha mask for reading alpha data. dwFlags must include DDPF_ALPHAPIXELS or DDPF_ALPHA. For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.
        /// </summary>
        private uint aBitMask;

        /// <summary>
        /// Alpha mask for reading alpha data. dwFlags must include DDPF_ALPHAPIXELS or DDPF_ALPHA. For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.
        /// </summary>
        public uint ABitMask { get { return aBitMask; } }
        
        internal Color[] ToRGBA(byte[] data, int width, int height)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int rOffset = (int)Math.Log(rBitMask / 256.0, 255.0);
            int gOffset = (int)Math.Log(gBitMask / 256.0, 255.0);
            int bOffset = (int)Math.Log(bBitMask / 256.0, 255.0);
            int aOffset = (int)Math.Log(aBitMask / 256.0, 255.0);

            Color[] colors = new Color[width * height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = (int)(x * height + y) * (int)rGBBitCount / (int)8;

                    byte r = 0;
                    byte g = 0;
                    byte b = 0;
                    byte a = 255;

                    if (Flags.HasFlag(PixelFormatFlags.RGB))
                    {
                        r = data[rOffset + index];
                        g = data[gOffset + index];
                        b = data[bOffset + index];
                    }
                    if (flags.HasFlag(PixelFormatFlags.AlphaPixels))
                        a = data[aOffset + index];

                    Color color = Color.FromArgb(a, r, g, b);

                    colors[x * height + y] = color;
                }
            }
            return colors;
        }
    }
}
