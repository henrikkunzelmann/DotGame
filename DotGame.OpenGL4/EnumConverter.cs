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
            {TextureFormat.RGBA32_Float, PixelInternalFormat.Rgba32f},
            {TextureFormat.DXT1, PixelInternalFormat.CompressedRgbaS3tcDxt1Ext},
            {TextureFormat.DXT3, PixelInternalFormat.CompressedRgbaS3tcDxt3Ext},
            {TextureFormat.DXT5, PixelInternalFormat.CompressedRgbaS3tcDxt5Ext},
            {TextureFormat.RGBA16_UIntNorm, PixelInternalFormat.Rgba16},
            {TextureFormat.RGBA8_UIntNorm, PixelInternalFormat.Rgba8},
            {TextureFormat.Depth16, PixelInternalFormat.DepthComponent16},
            {TextureFormat.Depth32, PixelInternalFormat.DepthComponent32},
            {TextureFormat.Depth24Stencil8, PixelInternalFormat.Depth24Stencil8},
        };
        private static readonly Dictionary<TextureFormat, Tuple<PixelFormat, PixelType>> dataFormats = new Dictionary<TextureFormat, Tuple<PixelFormat, PixelType>>() 
        {
            {TextureFormat.RGB32_Float, Tuple.Create(PixelFormat.Rgb, PixelType.Float)},
            {TextureFormat.RGBA32_Float, Tuple.Create(PixelFormat.Rgba, PixelType.Float)},
            {TextureFormat.RGBA16_UIntNorm, Tuple.Create(PixelFormat.Rgba, PixelType.UnsignedInt)},
            {TextureFormat.RGBA8_UIntNorm, Tuple.Create(PixelFormat.Rgba, PixelType.UnsignedInt)},
            {TextureFormat.Depth16, Tuple.Create(PixelFormat.DepthComponent, PixelType.UnsignedInt)},
            {TextureFormat.Depth32, Tuple.Create(PixelFormat.DepthComponent, PixelType.Float)},
            {TextureFormat.Depth24Stencil8, Tuple.Create(PixelFormat.DepthStencil, PixelType.UnsignedInt248)},
        };

        private static readonly Dictionary<string, string> shaderModels = new Dictionary<string, string>() 
        {
            {"vs_4_0","330"},
            {"vs_5_0","430"},
            {"ps_4_0","330"},
            {"ps_5_0","430"},
        };

        private static readonly Dictionary<DotGame.Graphics.PrimitiveType, OpenTK.Graphics.OpenGL4.PrimitiveType> primitiveTypes = new Dictionary<DotGame.Graphics.PrimitiveType, OpenTK.Graphics.OpenGL4.PrimitiveType>()
        {
            {DotGame.Graphics.PrimitiveType.TriangleList, OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles},
            {DotGame.Graphics.PrimitiveType.TriangleStrip, OpenTK.Graphics.OpenGL4.PrimitiveType.TriangleStrip},
            {DotGame.Graphics.PrimitiveType.PointList, OpenTK.Graphics.OpenGL4.PrimitiveType.Points},
            {DotGame.Graphics.PrimitiveType.LineList, OpenTK.Graphics.OpenGL4.PrimitiveType.Lines},
            {DotGame.Graphics.PrimitiveType.LineStrip, OpenTK.Graphics.OpenGL4.PrimitiveType.LineStrip},
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

        private static readonly Dictionary<FillMode, PolygonMode> fillModes = new Dictionary<FillMode, PolygonMode>()
        {
            {FillMode.Solid, PolygonMode.Fill},
            {FillMode.WireFrame, PolygonMode.Line},
        };

        private static readonly Dictionary<AddressMode, TextureWrapMode> addressModes = new Dictionary<AddressMode, TextureWrapMode>()
        {
            {AddressMode.Border, TextureWrapMode.ClampToBorder},
            {AddressMode.Clamp, TextureWrapMode.ClampToEdge},
            {AddressMode.Mirror,  TextureWrapMode.MirroredRepeat},
            {AddressMode.Wrap, TextureWrapMode.Repeat},
        };

        /// <summary>
        /// Min, Mag, Mip
        /// </summary>
        private static readonly Dictionary<Tuple<TextureFilter, TextureFilter, TextureFilter>, Tuple<TextureMinFilter, TextureMagFilter>> filters = new Dictionary<Tuple<TextureFilter, TextureFilter, TextureFilter>, Tuple<TextureMinFilter, TextureMagFilter>>()
        {
            { Tuple.Create(TextureFilter.Point, TextureFilter.Point, TextureFilter.Point), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.NearestMipmapNearest, TextureMagFilter.Nearest)},
            { Tuple.Create(TextureFilter.Point, TextureFilter.Point, TextureFilter.Linear), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Nearest)},
            { Tuple.Create(TextureFilter.Point, TextureFilter.Linear, TextureFilter.Point), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.NearestMipmapNearest, TextureMagFilter.Linear)},
            { Tuple.Create(TextureFilter.Point, TextureFilter.Linear, TextureFilter.Linear), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.NearestMipmapLinear, TextureMagFilter.Linear)},
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Point, TextureFilter.Point), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.LinearMipmapNearest, TextureMagFilter.Nearest)},
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Point, TextureFilter.Linear), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Nearest)},
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Linear, TextureFilter.Point), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.LinearMipmapNearest, TextureMagFilter.Linear)},
            { Tuple.Create(TextureFilter.Linear, TextureFilter.Linear, TextureFilter.Linear), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear)},
            { Tuple.Create(TextureFilter.Anisotropic, TextureFilter.Anisotropic, TextureFilter.Anisotropic), Tuple.Create<TextureMinFilter, TextureMagFilter>(TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear)},
        };

        private static readonly Dictionary<Comparison, int> comparisons = new Dictionary<Comparison, int>()
        {
            {Comparison.Always, (int)All.Always},
            {Comparison.Equal, (int)All.Equal},
            {Comparison.Greater, (int)All.Greater},
            {Comparison.GreaterEqual, (int)All.Gequal},
            {Comparison.Less, (int)All.Less},
            {Comparison.LessEqual, (int)All.Lequal},
            {Comparison.Never, (int)All.Never},
            {Comparison.NotEqual, (int)All.Notequal},
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

        internal static Tuple<PixelFormat, PixelType> ConvertPixelDataFormat(TextureFormat format)
        {
            if (!dataFormats.ContainsKey(format))
                throw new NotSupportedException("format is not supported");
            return dataFormats[format];
        }

        internal static TextureFormat Convert(PixelFormat format, PixelType type)
        {
            var tuple = Tuple.Create<PixelFormat, PixelType>(format, type);

            if (!dataFormats.ContainsValue(tuple))
                throw new NotImplementedException();
            return dataFormats.First((f) => f.Value == tuple).Key;
        }

        internal static OpenTK.Graphics.OpenGL4.PrimitiveType Convert(DotGame.Graphics.PrimitiveType primitiveType)
        {
            if (!primitiveTypes.ContainsKey(primitiveType))
                throw new NotImplementedException();
            return primitiveTypes[primitiveType];
        }

        internal static DotGame.Graphics.PrimitiveType Convert(OpenTK.Graphics.OpenGL4.PrimitiveType primitiveType)
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

        internal static PolygonMode Convert(FillMode fillMode)
        {
            if (!fillModes.ContainsKey(fillMode))
                throw new NotSupportedException("indexFormat is not supported");

            return fillModes[fillMode];
        }

        internal static FillMode Convert(PolygonMode fillMode)
        {
            if (!fillModes.ContainsValue(fillMode))
                throw new NotImplementedException();

            return fillModes.First((f) => f.Value == fillMode).Key;
        }

        internal static TextureWrapMode Convert(AddressMode addressMode)
        {
            if (!addressModes.ContainsKey(addressMode))
                throw new NotSupportedException("indexFormat is not supported");

            return addressModes[addressMode];
        }

        internal static AddressMode Convert(TextureWrapMode addressMode)
        {
            if (!addressModes.ContainsValue(addressMode))
                throw new NotImplementedException();

            return addressModes.First((f) => f.Value == addressMode).Key;
        }

        internal static Tuple<TextureMinFilter, TextureMagFilter> Convert(TextureFilter min, TextureFilter mag, TextureFilter mip)
        {
            Tuple<TextureFilter, TextureFilter, TextureFilter> tuple = new Tuple<TextureFilter, TextureFilter, TextureFilter>(min, mag, mip);

            if (!filters.ContainsKey(tuple))
                throw new NotSupportedException("indexFormat is not supported");

            return filters[tuple];
        }

        internal static Tuple<TextureFilter, TextureFilter, TextureFilter> Convert(TextureMinFilter min, TextureMagFilter mag)
        {
            Tuple<TextureMinFilter, TextureMagFilter> tuple = new Tuple<TextureMinFilter, TextureMagFilter>(min, mag);

            if (!filters.ContainsValue(tuple))
                throw new NotImplementedException();

            return filters.First((f) => f.Value == tuple).Key;
        }

        internal static int Convert(Comparison comparison)
        {
            if (!comparisons.ContainsKey(comparison))
                throw new NotSupportedException("indexFormat is not supported");

            return comparisons[comparison];
        }

        internal static Comparison Convert(int comparison)
        {
            if (!comparisons.ContainsValue(comparison))
                throw new NotImplementedException();

            return comparisons.First((f) => f.Value == comparison).Key;
        }
    }
}
