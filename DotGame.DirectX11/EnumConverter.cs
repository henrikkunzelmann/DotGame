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
    }
}
