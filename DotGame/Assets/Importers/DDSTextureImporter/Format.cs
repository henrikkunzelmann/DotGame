using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    internal enum Format : uint
    {
        // Zusammenfassung:
        //      The format is not known.
        Unknown = 0,
        //
        // Zusammenfassung:
        //      A four-component, 128-bit typeless format that supports 32 bits per channel
        //     including alpha. 1
        R32G32B32A32_Typeless = 1,
        //
        // Zusammenfassung:
        //      A four-component, 128-bit floating-point format that supports 32 bits per
        //     channel including alpha. 1
        R32G32B32A32_Float = 2,
        //
        // Zusammenfassung:
        //      A four-component, 128-bit unsigned-integer format that supports 32 bits
        //     per channel including alpha. 1
        R32G32B32A32_UInt = 3,
        //
        // Zusammenfassung:
        //      A four-component, 128-bit signed-integer format that supports 32 bits per
        //     channel including alpha. 1
        R32G32B32A32_SInt = 4,
        //
        // Zusammenfassung:
        //      A three-component, 96-bit typeless format that supports 32 bits per color
        //     channel.
        R32G32B32_Typeless = 5,
        //
        // Zusammenfassung:
        //      A three-component, 96-bit floating-point format that supports 32 bits per
        //     color channel.
        R32G32B32_Float = 6,
        //
        // Zusammenfassung:
        //      A three-component, 96-bit unsigned-integer format that supports 32 bits
        //     per color channel.
        R32G32B32_UInt = 7,
        //
        // Zusammenfassung:
        //      A three-component, 96-bit signed-integer format that supports 32 bits per
        //     color channel.
        R32G32B32_SInt = 8,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit typeless format that supports 16 bits per channel
        //     including alpha.
        R16G16B16A16_Typeless = 9,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit floating-point format that supports 16 bits per
        //     channel including alpha.
        R16G16B16A16_Float = 10,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit unsigned-normalized-integer format that supports
        //     16 bits per channel including alpha.
        R16G16B16A16_UNorm = 11,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit unsigned-integer format that supports 16 bits per
        //     channel including alpha.
        R16G16B16A16_UInt = 12,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit signed-normalized-integer format that supports
        //     16 bits per channel including alpha.
        R16G16B16A16_SNorm = 13,
        //
        // Zusammenfassung:
        //      A four-component, 64-bit signed-integer format that supports 16 bits per
        //     channel including alpha.
        R16G16B16A16_SInt = 14,
        //
        // Zusammenfassung:
        //      A two-component, 64-bit typeless format that supports 32 bits for the red
        //     channel and 32 bits for the green channel.
        R32G32_Typeless = 15,
        //
        // Zusammenfassung:
        //      A two-component, 64-bit floating-point format that supports 32 bits for
        //     the red channel and 32 bits for the green channel.
        R32G32_Float = 16,
        //
        // Zusammenfassung:
        //      A two-component, 64-bit unsigned-integer format that supports 32 bits for
        //     the red channel and 32 bits for the green channel.
        R32G32_UInt = 17,
        //
        // Zusammenfassung:
        //      A two-component, 64-bit signed-integer format that supports 32 bits for
        //     the red channel and 32 bits for the green channel.
        R32G32_SInt = 18,
        //
        // Zusammenfassung:
        //      A two-component, 64-bit typeless format that supports 32 bits for the red
        //     channel, 8 bits for the green channel, and 24 bits are unused.
        R32G8X24_Typeless = 19,
        //
        // Zusammenfassung:
        //      A 32-bit floating-point component, and two unsigned-integer components (with
        //     an additional 32 bits). This format supports 32-bit depth, 8-bit stencil,
        //     and 24 bits are unused.
        D32_Float_S8X24_UInt = 20,
        //
        // Zusammenfassung:
        //      A 32-bit floating-point component, and two typeless components (with an
        //     additional 32 bits). This format supports 32-bit red channel, 8 bits are
        //     unused, and 24 bits are unused.
        R32_Float_X8X24_Typeless = 21,
        //
        // Zusammenfassung:
        //      A 32-bit typeless component, and two unsigned-integer components (with an
        //     additional 32 bits). This format has 32 bits unused, 8 bits for green channel,
        //     and 24 bits are unused.
        X32_Typeless_G8X24_UInt = 22,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit typeless format that supports 10 bits for each
        //     color and 2 bits for alpha.
        R10G10B10A2_Typeless = 23,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format that supports
        //     10 bits for each color and 2 bits for alpha.
        R10G10B10A2_UNorm = 24,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-integer format that supports 10 bits for
        //     each color and 2 bits for alpha.
        R10G10B10A2_UInt = 25,
        //
        // Zusammenfassung:
        //      Three partial-precision floating-point numbers encoded into a single 32-bit
        //     value (a variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit
        //     biased (15) exponent). There are no sign bits, and there is a 5-bit biased
        //     (15) exponent for each channel, 6-bit mantissa for R and G, and a 5-bit mantissa
        //     for B, as shown in the following illustration.
        R11G11B10_Float = 26,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit typeless format that supports 8 bits per channel
        //     including alpha.
        R8G8B8A8_Typeless = 27,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format that supports
        //     8 bits per channel including alpha.
        R8G8B8A8_UNorm = 28,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized integer sRGB format that supports
        //     8 bits per channel including alpha.
        R8G8B8A8_UNorm_SRgb = 29,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-integer format that supports 8 bits per
        //     channel including alpha.
        R8G8B8A8_UInt = 30,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit signed-normalized-integer format that supports
        //     8 bits per channel including alpha.
        R8G8B8A8_SNorm = 31,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit signed-integer format that supports 8 bits per
        //     channel including alpha.
        R8G8B8A8_SInt = 32,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit typeless format that supports 16 bits for the red
        //     channel and 16 bits for the green channel.
        R16G16_Typeless = 33,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit floating-point format that supports 16 bits for
        //     the red channel and 16 bits for the green channel.
        R16G16_Float = 34,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit unsigned-normalized-integer format that supports
        //     16 bits each for the green and red channels.
        R16G16_UNorm = 35,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit unsigned-integer format that supports 16 bits for
        //     the red channel and 16 bits for the green channel.
        R16G16_UInt = 36,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit signed-normalized-integer format that supports 16
        //     bits for the red channel and 16 bits for the green channel.
        R16G16_SNorm = 37,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit signed-integer format that supports 16 bits for
        //     the red channel and 16 bits for the green channel.
        R16G16_SInt = 38,
        //
        // Zusammenfassung:
        //      A single-component, 32-bit typeless format that supports 32 bits for the
        //     red channel.
        R32_Typeless = 39,
        //
        // Zusammenfassung:
        //      A single-component, 32-bit floating-point format that supports 32 bits for
        //     depth.
        D32_Float = 40,
        //
        // Zusammenfassung:
        //      A single-component, 32-bit floating-point format that supports 32 bits for
        //     the red channel.
        R32_Float = 41,
        //
        // Zusammenfassung:
        //      A single-component, 32-bit unsigned-integer format that supports 32 bits
        //     for the red channel.
        R32_UInt = 42,
        //
        // Zusammenfassung:
        //      A single-component, 32-bit signed-integer format that supports 32 bits for
        //     the red channel.
        R32_SInt = 43,
        //
        // Zusammenfassung:
        //      A two-component, 32-bit typeless format that supports 24 bits for the red
        //     channel and 8 bits for the green channel.
        R24G8_Typeless = 44,
        //
        // Zusammenfassung:
        //      A 32-bit z-buffer format that supports 24 bits for depth and 8 bits for
        //     stencil.
        D24_UNorm_S8_UInt = 45,
        //
        // Zusammenfassung:
        //      A 32-bit format, that contains a 24 bit, single-component, unsigned-normalized
        //     integer, with an additional typeless 8 bits. This format has 24 bits red
        //     channel and 8 bits unused.
        R24_UNorm_X8_Typeless = 46,
        //
        // Zusammenfassung:
        //      A 32-bit format, that contains a 24 bit, single-component, typeless format,
        //     with an additional 8 bit unsigned integer component. This format has 24 bits
        //     unused and 8 bits green channel.
        X24_Typeless_G8_UInt = 47,
        //
        // Zusammenfassung:
        //      A two-component, 16-bit typeless format that supports 8 bits for the red
        //     channel and 8 bits for the green channel.
        R8G8_Typeless = 48,
        //
        // Zusammenfassung:
        //      A two-component, 16-bit unsigned-normalized-integer format that supports
        //     8 bits for the red channel and 8 bits for the green channel.
        R8G8_UNorm = 49,
        //
        // Zusammenfassung:
        //      A two-component, 16-bit unsigned-integer format that supports 8 bits for
        //     the red channel and 8 bits for the green channel.
        R8G8_UInt = 50,
        //
        // Zusammenfassung:
        //      A two-component, 16-bit signed-normalized-integer format that supports 8
        //     bits for the red channel and 8 bits for the green channel.
        R8G8_SNorm = 51,
        //
        // Zusammenfassung:
        //      A two-component, 16-bit signed-integer format that supports 8 bits for the
        //     red channel and 8 bits for the green channel.
        R8G8_SInt = 52,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit typeless format that supports 16 bits for the
        //     red channel.
        R16_Typeless = 53,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit floating-point format that supports 16 bits for
        //     the red channel.
        R16_Float = 54,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit unsigned-normalized-integer format that supports
        //     16 bits for depth.
        D16_UNorm = 55,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit unsigned-normalized-integer format that supports
        //     16 bits for the red channel.
        R16_UNorm = 56,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit unsigned-integer format that supports 16 bits
        //     for the red channel.
        R16_UInt = 57,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit signed-normalized-integer format that supports
        //     16 bits for the red channel.
        R16_SNorm = 58,
        //
        // Zusammenfassung:
        //      A single-component, 16-bit signed-integer format that supports 16 bits for
        //     the red channel.
        R16_SInt = 59,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit typeless format that supports 8 bits for the red
        //     channel.
        R8_Typeless = 60,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit unsigned-normalized-integer format that supports
        //     8 bits for the red channel.
        R8_UNorm = 61,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit unsigned-integer format that supports 8 bits for
        //     the red channel.
        R8_UInt = 62,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit signed-normalized-integer format that supports
        //     8 bits for the red channel.
        R8_SNorm = 63,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit signed-integer format that supports 8 bits for
        //     the red channel.
        R8_SInt = 64,
        //
        // Zusammenfassung:
        //      A single-component, 8-bit unsigned-normalized-integer format for alpha only.
        A8_UNorm = 65,
        //
        // Zusammenfassung:
        //      A single-component, 1-bit unsigned-normalized integer format that supports
        //     1 bit for the red channel. 2.
        R1_UNorm = 66,
        //
        // Zusammenfassung:
        //      Three partial-precision floating-point numbers encoded into a single 32-bit
        //     value all sharing the same 5-bit exponent (variant of s10e5, which is sign
        //     bit, 10-bit mantissa, and 5-bit biased (15) exponent). There is no sign bit,
        //     and there is a shared 5-bit biased (15) exponent and a 9-bit mantissa for
        //     each channel, as shown in the following illustration. 2.
        R9G9B9E5_Sharedexp = 67,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format. This packed
        //     RGB format is analogous to the UYVY format. Each 32-bit block describes a
        //     pair of pixels: (R8, G8, B8) and (R8, G8, B8) where the R8/B8 values are
        //     repeated, and the G8 values are unique to each pixel. 3 Width must be even.
        R8G8_B8G8_UNorm = 68,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format. This packed
        //     RGB format is analogous to the YUY2 format. Each 32-bit block describes a
        //     pair of pixels: (R8, G8, B8) and (R8, G8, B8) where the R8/B8 values are
        //     repeated, and the G8 values are unique to each pixel. 3 Width must be even.
        G8R8_G8B8_UNorm = 69,
        //
        // Zusammenfassung:
        //      Four-component typeless block-compression format. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC1_Typeless = 70,
        //
        // Zusammenfassung:
        //      Four-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC1_UNorm = 71,
        //
        // Zusammenfassung:
        //      Four-component block-compression format for sRGB data. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC1_UNorm_SRgb = 72,
        //
        // Zusammenfassung:
        //      Four-component typeless block-compression format. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC2_Typeless = 73,
        //
        // Zusammenfassung:
        //      Four-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC2_UNorm = 74,
        //
        // Zusammenfassung:
        //      Four-component block-compression format for sRGB data. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC2_UNorm_SRgb = 75,
        //
        // Zusammenfassung:
        //      Four-component typeless block-compression format. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC3_Typeless = 76,
        //
        // Zusammenfassung:
        //      Four-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC3_UNorm = 77,
        //
        // Zusammenfassung:
        //      Four-component block-compression format for sRGB data. For information about
        //     block-compression formats, see Texture Block Compression in Direct3D 11.
        BC3_UNorm_SRgb = 78,
        //
        // Zusammenfassung:
        //      One-component typeless block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC4_Typeless = 79,
        //
        // Zusammenfassung:
        //      One-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC4_UNorm = 80,
        //
        // Zusammenfassung:
        //      One-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC4_SNorm = 81,
        //
        // Zusammenfassung:
        //      Two-component typeless block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC5_Typeless = 82,
        //
        // Zusammenfassung:
        //      Two-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC5_UNorm = 83,
        //
        // Zusammenfassung:
        //      Two-component block-compression format. For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC5_SNorm = 84,
        //
        // Zusammenfassung:
        //      A three-component, 16-bit unsigned-normalized-integer format that supports
        //     5 bits for blue, 6 bits for green, and 5 bits for red. Direct3D 10 through
        //     Direct3D 11:??This value is defined for DXGI. However, Direct3D 10, 10.1,
        //     or 11 devices do not support this format. Direct3D 11.1:??This value is not
        //     supported until Windows?8.
        B5G6R5_UNorm = 85,
        //
        // Zusammenfassung:
        //      A four-component, 16-bit unsigned-normalized-integer format that supports
        //     5 bits for each color channel and 1-bit alpha. Direct3D 10 through Direct3D
        //     11:??This value is defined for DXGI. However, Direct3D 10, 10.1, or 11 devices
        //     do not support this format. Direct3D 11.1:??This value is not supported until
        //     Windows?8.
        B5G5R5A1_UNorm = 86,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format that supports
        //     8 bits for each color channel and 8-bit alpha.
        B8G8R8A8_UNorm = 87,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized-integer format that supports
        //     8 bits for each color channel and 8 bits unused.
        B8G8R8X8_UNorm = 88,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit 2.8-biased fixed-point format that supports 10
        //     bits for each color channel and 2-bit alpha.
        R10G10B10_Xr_Bias_A2_UNorm = 89,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit typeless format that supports 8 bits for each channel
        //     including alpha. 4
        B8G8R8A8_Typeless = 90,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized standard RGB format that supports
        //     8 bits for each channel including alpha. 4
        B8G8R8A8_UNorm_SRgb = 91,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit typeless format that supports 8 bits for each color
        //     channel, and 8 bits are unused. 4
        B8G8R8X8_Typeless = 92,
        //
        // Zusammenfassung:
        //      A four-component, 32-bit unsigned-normalized standard RGB format that supports
        //     8 bits for each color channel, and 8 bits are unused. 4
        B8G8R8X8_UNorm_SRgb = 93,
        //
        // Zusammenfassung:
        //      A typeless block-compression format. 4 For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC6H_Typeless = 94,
        //
        // Zusammenfassung:
        //      A block-compression format. 4 For information about block-compression formats,
        //     see Texture Block Compression in Direct3D 11.
        BC6H_Uf16 = 95,
        //
        // Zusammenfassung:
        //      A block-compression format. 4 For information about block-compression formats,
        //     see Texture Block Compression in Direct3D 11.
        BC6H_Sf16 = 96,
        //
        // Zusammenfassung:
        //      A typeless block-compression format. 4 For information about block-compression
        //     formats, see Texture Block Compression in Direct3D 11.
        BC7_Typeless = 97,
        //
        // Zusammenfassung:
        //      A block-compression format. 4 For information about block-compression formats,
        //     see Texture Block Compression in Direct3D 11.
        BC7_UNorm = 98,
        //
        // Zusammenfassung:
        //      A block-compression format. 4 For information about block-compression formats,
        //     see Texture Block Compression in Direct3D 11.
        BC7_UNorm_SRgb = 99,
    }
}
