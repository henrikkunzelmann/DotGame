using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.DXGI;

namespace DotGame.DirectX11
{
    public static class FormatConverter
    {
        private static readonly Dictionary<TextureFormat, Format> formats = new Dictionary<TextureFormat, Format>()
        {
            { TextureFormat.Unknown, Format.Unknown },
            { TextureFormat.RGB32_Float, Format.R32G32B32_Float },
            { TextureFormat.RGBA32_Float, Format.R32G32B32A32_Float },
            { TextureFormat.RGBA8_UIntNorm, Format.R8G8B8A8_UNorm },
            { TextureFormat.Depth16, Format.D16_UNorm },
            { TextureFormat.Depth24Stencil8, Format.D24_UNorm_S8_UInt },
            { TextureFormat.Depth32, Format.D32_Float }
            // TODO (henrik1235) Mehr TextureFormate hinzufügen
        };

        public static Format Convert(TextureFormat format)
        {
            if (!formats.ContainsKey(format))
                throw new NotSupportedException("format is not supported");
            return formats[format];
        }

        public static TextureFormat Convert(Format format)
        {
            if (!formats.ContainsValue(format))
                throw new NotImplementedException();
            return formats.First((f) => f.Value == format).Key;
        }
    }
}
