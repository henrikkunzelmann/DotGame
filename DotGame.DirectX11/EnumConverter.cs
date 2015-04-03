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

        private static readonly Dictionary<AddressMode, TextureAddressMode> addressModes = new Dictionary<AddressMode, TextureAddressMode>()
        {
            { AddressMode.Border, TextureAddressMode.Border },
            { AddressMode.Clamp, TextureAddressMode.Clamp },
            { AddressMode.Mirror, TextureAddressMode.Mirror },
            { AddressMode.MirrorOnce, TextureAddressMode.MirrorOnce },
            { AddressMode.Wrap, TextureAddressMode.Wrap },
        };

        private static readonly Dictionary<DotGame.Graphics.Comparison, SharpDX.Direct3D11.Comparison> comparsionFunctions = new Dictionary<Graphics.Comparison, SharpDX.Direct3D11.Comparison>()
        {
            { DotGame.Graphics.Comparison.Always, SharpDX.Direct3D11.Comparison.Always },
            { DotGame.Graphics.Comparison.Equal, SharpDX.Direct3D11.Comparison.Equal },
            { DotGame.Graphics.Comparison.Greater, SharpDX.Direct3D11.Comparison.Greater },
            { DotGame.Graphics.Comparison.GreaterEqual, SharpDX.Direct3D11.Comparison.GreaterEqual },
            { DotGame.Graphics.Comparison.Less, SharpDX.Direct3D11.Comparison.Less },
            { DotGame.Graphics.Comparison.LessEqual, SharpDX.Direct3D11.Comparison.LessEqual },
            { DotGame.Graphics.Comparison.Never, SharpDX.Direct3D11.Comparison.Never },
            { DotGame.Graphics.Comparison.NotEqual, SharpDX.Direct3D11.Comparison.NotEqual },
        };

        public static Format Convert(TextureFormat format)
        {
            Format f;
            if (!textureFormats.TryGetValue(format, out f))
                throw new NotSupportedException("Format is not supported.");
            return f;
        }

        public static Format Convert(VertexElementType format)
        {
            Format f;
            if (!vertexFormats.TryGetValue(format, out f))
                throw new NotSupportedException("Format is not supported.");
            return f;
        }

        public static PrimitiveTopology Convert(PrimitiveType type)
        {
            PrimitiveTopology t;
            if (!primitiveTypes.TryGetValue(type, out t))
                throw new NotSupportedException("Type is not supported.");
            return t;
        }

        public static string Convert(VertexElementUsage usage)
        {
            string str;
            if (!vertexElementUsages.TryGetValue(usage, out str))
                throw new NotSupportedException("Usage is not supported.");
            return str;
        }

        public static Format Convert(IndexFormat format)
        {
            Format f;
            if (!indexFormats.TryGetValue(format, out f))
                throw new NotSupportedException("Format is not supported.");
            return f;
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

        public static TextureAddressMode Convert(AddressMode mode)
        {
            TextureAddressMode m;
            if (!addressModes.TryGetValue(mode, out m))
                throw new NotSupportedException("Address mode not supported.");
            return m;
        }

        public static SharpDX.Direct3D11.Comparison Convert(DotGame.Graphics.Comparison comparison)
        {
            SharpDX.Direct3D11.Comparison c;
            if (!comparsionFunctions.TryGetValue(comparison, out c))
                throw new NotSupportedException("Comparison function not supported.");
            return c;
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

        public static AddressMode Convert(TextureAddressMode mode)
        {
            if (!addressModes.ContainsValue(mode))
                throw new NotImplementedException();
            return addressModes.First((f) => f.Value == mode).Key;
        }

        public static DotGame.Graphics.Comparison Convert(SharpDX.Direct3D11.Comparison comparsionFunction)
        {
            if (!comparsionFunctions.ContainsValue(comparsionFunction))
                throw new NotImplementedException();
            return comparsionFunctions.First((f) => f.Value == comparsionFunction).Key;
        }
    }
}
