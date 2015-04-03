using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal static class EnumConverter
    {
        private static readonly Dictionary<TextureFormat, PixelInternalFormat> formats = new Dictionary<TextureFormat, PixelInternalFormat>() 
        {
            {TextureFormat.RGB32_Float, PixelInternalFormat.Rgb32f},
            {TextureFormat.DXT1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext},
            {TextureFormat.DXT3, PixelInternalFormat.CompressedRgbaS3tcDxt3Ext},
            {TextureFormat.DXT5, PixelInternalFormat.CompressedRgbaS3tcDxt5Ext},
            {TextureFormat.RGBA16_UIntNorm, PixelInternalFormat.Rgba16},
            {TextureFormat.RGBA8_UIntNorm, PixelInternalFormat.Rgba8},
            {TextureFormat.Depth16, PixelInternalFormat.DepthComponent16},
            {TextureFormat.Depth32, PixelInternalFormat.DepthComponent32},
            {TextureFormat.Depth24Stencil8, PixelInternalFormat.Depth24Stencil8},
        };

        private static readonly Dictionary<string, string> shaderModels = new Dictionary<string, string>() 
        {
            {"vs_4_0","330"},
            {"vs_5_0","430"},
            {"ps_4_0","330"},
            {"ps_5_0","430"},
        };

        private static readonly Dictionary<PrimitiveType, OpenTK.Graphics.OpenGL4.PrimitiveType> primitiveTypes = new Dictionary<PrimitiveType, OpenTK.Graphics.OpenGL4.PrimitiveType>()
        {
            {PrimitiveType.TriangleList, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles},
            {PrimitiveType.TriangleStrip, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip},
            {PrimitiveType.PointList, OpenTK.Graphics.OpenGL4.PrimitiveType.Points},
            {PrimitiveType.LineList, OpenTK.Graphics.OpenGL4.PrimitiveType.Lines},
            {PrimitiveType.LineStrip, OpenTK.Graphics.OpenGL4.PrimitiveType.LineStrip},
        };

        private static readonly Dictionary<VertexElementUsage, string> vertexElementUsages = new Dictionary<VertexElementUsage, string>()
        {
            { VertexElementUsage.Position, "in_position" },
            { VertexElementUsage.Color, "in_COLOR" },
            { VertexElementUsage.TexCoord, "in_texCoord" },
            { VertexElementUsage.Normal, "in_normal" },
            { VertexElementUsage.Tangent, "in_tangent" },
            { VertexElementUsage.Binormal, "in_binormal" },
        };

        private static readonly Dictionary<IndexFormat, DrawElementsType> indexFormats = new Dictionary<IndexFormat, DrawElementsType>()
        {
            {IndexFormat.UInt32, DrawElementsType.UnsignedInt},
            {IndexFormat.UShort16, DrawElementsType.UnsignedShort},
        };

        private static readonly Dictionary<CullMode, CullFaceMode> cullModes = new Dictionary<CullMode, CullFaceMode>()
        {
            {CullMode.Front, CullFaceMode.Front},
            {CullMode.Back, CullFaceMode.Back},
        };

        internal static PixelInternalFormat Convert(TextureFormat format)
        {
            if (!formats.ContainsKey(format))
                throw new NotSupportedException("format is not supported");
            return formats[format];
        }

        internal static TextureFormat Convert(PixelInternalFormat format)
        {
            if (!formats.ContainsValue(format))
                throw new NotImplementedException();
            return formats.First((f) => f.Value == format).Key;
        }

        internal static OpenTK.Graphics.OpenGL4.PrimitiveType Convert(PrimitiveType primitiveType)
        {
            if (!primitiveTypes.ContainsKey(primitiveType))
                throw new NotImplementedException();
            return primitiveTypes[primitiveType];
        }

        internal static PrimitiveType Convert(OpenTK.Graphics.OpenGL4.PrimitiveType primitiveType)
        {
            if (!primitiveTypes.ContainsValue(primitiveType))
                throw new NotImplementedException();
            return primitiveTypes.First((pT) => pT.Value == primitiveType).Key;
        }

        public static string Convert(VertexElementUsage usage)
        {
            if (!vertexElementUsages.ContainsKey(usage))
                throw new NotSupportedException("Usage is not supported.");
            return vertexElementUsages[usage];
        }

        internal static string ConvertToGLSLVersion(string shaderModel)
        {
            if (!shaderModels.ContainsKey(shaderModel))
                throw new NotSupportedException("shaderModel is not supported");

            return shaderModels[shaderModel];
        }

        internal static DrawElementsType Convert(IndexFormat indexFormat)
        {
            if (!indexFormats.ContainsKey(indexFormat))
                throw new NotSupportedException("indexFormat is not supported");

            return indexFormats[indexFormat];     
        }

        internal static IndexFormat Convert(DrawElementsType indexFormat)
        {
            if (!indexFormats.ContainsValue(indexFormat))
                throw new NotImplementedException();

            return indexFormats.First((f) => f.Value == indexFormat).Key;
        }

        internal static CullFaceMode Convert(CullMode cullMode)
        {
            if (!cullModes.ContainsKey(cullMode))
                throw new NotSupportedException("indexFormat is not supported");

            return cullModes[cullMode];
        }

        internal static CullMode Convert(CullFaceMode cullMode)
        {
            if (!cullModes.ContainsValue(cullMode))
                throw new NotImplementedException();

            return cullModes.First((f) => f.Value == cullMode).Key;
        }
    }
}
