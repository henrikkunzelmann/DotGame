using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;

namespace DotGame.DirectX11
{
    public static class EnumConverter
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

        private static readonly Dictionary<PrimitiveType, PrimitiveTopology> primitiveTypes = new Dictionary<PrimitiveType, PrimitiveTopology>()
        {
            { PrimitiveType.PointList, PrimitiveTopology.PointList },
            { PrimitiveType.LineList, PrimitiveTopology.LineList },
            { PrimitiveType.LineStrip, PrimitiveTopology.LineStrip },
            { PrimitiveType.TriangleList, PrimitiveTopology.TriangleList },
            { PrimitiveType.TriangleStrip, PrimitiveTopology.TriangleStrip }
        };

        private static readonly Dictionary<VertexElementUsage, string> vertexElementUsages = new Dictionary<VertexElementUsage, string>()
        {
            { VertexElementUsage.Position, "POSITION" },
            { VertexElementUsage.Color, "COLOR" },
            { VertexElementUsage.TexCoord, "TEXCOORD" },
            { VertexElementUsage.Normal, "NORMAL" },
            { VertexElementUsage.Tangent, "TANGENT" },
            { VertexElementUsage.Binormal, "BINORMAL" },
        };

        private static readonly Dictionary<IndexFormat, Format> indexFormats = new Dictionary<IndexFormat, Format>()
        {
            { IndexFormat.Int32, Format.R32_SInt },
            { IndexFormat.UInt32, Format.R32_UInt },
            { IndexFormat.Short16, Format.R16_SInt },
            { IndexFormat.UShort16, Format.R16_UInt },
        };

        private static readonly Dictionary<Tuple<TextureFilter, TextureFilter, TextureFilter>, Filter> filters = new Dictionary<Tuple<TextureFilter, TextureFilter, TextureFilter>, Filter>()
        {
            { Tuple.Create(TextureFilter.Point, TextureFilter.Point, TextureFilter.Point), Filter.MinMagMipPoint },
            { Tuple.Create(TextureFilter.Point, TextureFilter.Point, TextureFilter.Linear), Filter.MinMagMipLinear },
            { Tuple.Create(TextureFilter.Point, TextureFilter.Linear, TextureFilter.Point), Filter.MinPointMagLinearMipPoint },
            { Tuple.Create(TextureFilter.Point, TextureFilter.Linear, TextureFilter.Linear), Filter.MinPointMagMipLinear },
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Point, TextureFilter.Point), Filter.MinLinearMagMipPoint },
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Point, TextureFilter.Linear), Filter.MinLinearMagPointMipLinear },
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Linear, TextureFilter.Point), Filter.MinMagLinearMipPoint },
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Linear, TextureFilter.Linear), Filter.MinMagMipLinear },
            { Tuple.Create(TextureFilter.Anisotropic, TextureFilter.Anisotropic, TextureFilter.Anisotropic), Filter.Anisotropic }
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

        public static PrimitiveTopology Convert(PrimitiveType type)
        {
            if (!primitiveTypes.ContainsKey(type))
                throw new NotSupportedException("Type is not supported.");
            return primitiveTypes[type];
        }

        public static string Convert(VertexElementUsage usage)
        {
            if (!vertexElementUsages.ContainsKey(usage))
                throw new NotSupportedException("Usage is not supported.");
            return vertexElementUsages[usage];
        }

        public static Format Convert(IndexFormat format)
        {
            if (!indexFormats.ContainsKey(format))
                throw new NotSupportedException("Format is not supported.");
            return indexFormats[format];
        }

        public static Filter Convert(SamplerType type, TextureFilter min, TextureFilter mag, TextureFilter mip)
        {
            int offset;
            switch(type)
            {
                case SamplerType.Normal:
                    offset = 0;
                    break;
                case SamplerType.Comparison:
                    offset = 0x80;
                    break;
                case SamplerType.Minimum:
                    offset = 0x100;
                    break;
                case SamplerType.Maximum:
                    offset = 0x180;
                    break;
                default:
                    throw new NotSupportedException("SamplerType is not supported.");
            }
            Filter f;
            if (!filters.TryGetValue(new Tuple<TextureFilter, TextureFilter, TextureFilter>(min, mag, mip), out f))
                throw new NotSupportedException("TextureFilter variant not supported.");
            return (Filter)(f + offset);
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

        public static PrimitiveType Convert(PrimitiveTopology type)
        {
            if (!primitiveTypes.ContainsValue(type))
                throw new NotImplementedException();
            return primitiveTypes.First((f) => f.Value == type).Key;
        }

        public static VertexElementUsage ConvertToUsage(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (!vertexElementUsages.ContainsValue(name))
                throw new NotImplementedException();
            return vertexElementUsages.First((f) => f.Value == name).Key;
        }

        public static IndexFormat Convert(Format format)
        {
            if (!indexFormats.ContainsValue(format))
                throw new NotImplementedException();
            return indexFormats.First((f) => f.Value == format).Key;
        }
    }
}
