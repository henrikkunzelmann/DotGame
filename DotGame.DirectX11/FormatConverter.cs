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
        private static readonly Dictionary<TextureFormat, Format> textureFormats = new Dictionary<TextureFormat, Format>()
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

        private static readonly Dictionary<VertexElementType, Format> vertexFormats = new Dictionary<VertexElementType, Format>()
        {
            { VertexElementType.Single, Format.R32_Float },
            { VertexElementType.Vector2, Format.R32G32_Float },
            { VertexElementType.Vector3, Format.R32G32B32_Float },
            { VertexElementType.Vector4, Format.R32G32B32A32_Float }
        };

        public static Format Convert(TextureFormat format)
        {
            if (!textureFormats.ContainsKey(format))
                throw new NotSupportedException("Format is not supported.");
            return textureFormats[format];
        }

        public static Format Convert(VertexElementType format)
        {
            if (!vertexFormats.ContainsKey(format))
                throw new NotSupportedException("Format is not supported.");
            return vertexFormats[format];
        }

        public static TextureFormat ConvertToTexture(Format format)
        {
            if (!textureFormats.ContainsValue(format))
                throw new NotImplementedException();
            return textureFormats.First((f) => f.Value == format).Key;
        }

        public static VertexElementType ConvertToVertex(Format format)
        {
            if (!vertexFormats.ContainsValue(format))
                throw new NotImplementedException();
            return vertexFormats.First((f) => f.Value == format).Key;
        }
    }
}
