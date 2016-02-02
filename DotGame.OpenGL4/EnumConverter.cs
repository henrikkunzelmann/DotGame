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
        private static readonly Dictionary<TextureFormat, Tuple<PixelInternalFormat, PixelFormat, PixelType>> formats = new Dictionary<TextureFormat, Tuple<PixelInternalFormat, PixelFormat, PixelType>>() 
        {
            {TextureFormat.RGB32_Float, Tuple.Create(PixelInternalFormat.Rgb, PixelFormat.Rgb, PixelType.Float)},
            {TextureFormat.RGBA32_Float, Tuple.Create(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.Float)},
            {TextureFormat.RGBA16_UIntNorm, Tuple.Create(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedShort)},
            {TextureFormat.RGBA8_UIntNorm, Tuple.Create(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte)},
            {TextureFormat.BGRA8_UIntNorm, Tuple.Create(PixelInternalFormat.Rgb, PixelFormat.Bgra, PixelType.UnsignedByte)},
            {TextureFormat.DXT1, Tuple.Create(PixelInternalFormat.CompressedRgbS3tcDxt1Ext, PixelFormat.Bgra, PixelType.UnsignedByte)},
            {TextureFormat.DXT3, Tuple.Create(PixelInternalFormat.CompressedRgbaS3tcDxt3Ext, PixelFormat.Bgra, PixelType.UnsignedByte)},
            {TextureFormat.DXT5, Tuple.Create(PixelInternalFormat.CompressedRgbaS3tcDxt5Ext, PixelFormat.Bgra, PixelType.UnsignedByte)},
            {TextureFormat.Depth16, Tuple.Create(PixelInternalFormat.DepthComponent, PixelFormat.DepthComponent, PixelType.UnsignedInt)},
            {TextureFormat.Depth32, Tuple.Create(PixelInternalFormat.DepthComponent, PixelFormat.DepthComponent, PixelType.Float)},
            {TextureFormat.Depth24Stencil8, Tuple.Create(PixelInternalFormat.DepthStencil, PixelFormat.DepthStencil, PixelType.UnsignedInt248)},
        };
        private static readonly Dictionary<TextureFormat, SizedInternalFormat> sizedInternalFormats = new Dictionary<TextureFormat, SizedInternalFormat>() 
        {
            {TextureFormat.RGB32_Float, SizedInternalFormat.Rgba32f},
            {TextureFormat.RGBA32_Float, SizedInternalFormat.Rgba32f},
            {TextureFormat.RGBA16_UIntNorm, SizedInternalFormat.Rgba16},
            {TextureFormat.RGBA8_UIntNorm, SizedInternalFormat.Rgba8},
            {TextureFormat.BGRA8_UIntNorm, SizedInternalFormat.Rgba8},
            {TextureFormat.Depth16, SizedInternalFormat.R16},
            {TextureFormat.Depth32, SizedInternalFormat.R32f},
            {TextureFormat.Depth24Stencil8, SizedInternalFormat.R32f},
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
            { VertexElementUsage.Color, "in_color" },
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

        /// Min, Mag, Mip
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

        private static readonly Dictionary<DotGame.Graphics.Comparison, DepthFunction> comparisons = new Dictionary<DotGame.Graphics.Comparison, DepthFunction>() 
        {
            {Comparison.Always, DepthFunction.Always},
            {Comparison.Equal, DepthFunction.Equal},
            {Comparison.Greater, DepthFunction.Greater},
            {Comparison.GreaterEqual, DepthFunction.Gequal},
            {Comparison.Less, DepthFunction.Less},
            {Comparison.LessEqual, DepthFunction.Lequal},
            {Comparison.Never, DepthFunction.Never},
            {Comparison.NotEqual, DepthFunction.Notequal},
        };

        private static readonly Dictionary<Blend, OpenTK.Graphics.OpenGL4.BlendingFactorSrc> blends = new Dictionary<Blend, BlendingFactorSrc>() 
        {
            {Blend.Zero, BlendingFactorSrc.Zero},
            {Blend.One, BlendingFactorSrc.One},
            {Blend.SrcColor, BlendingFactorSrc.SrcColor},
            {Blend.InvSrcColor, BlendingFactorSrc.OneMinusSrcColor},
            {Blend.SrcAlpha, BlendingFactorSrc.SrcAlpha},
            {Blend.InvSrcAlpha, BlendingFactorSrc.OneMinusSrcAlpha},
            {Blend.DestAlpha, BlendingFactorSrc.DstAlpha},
            {Blend.InvDestAlpha, BlendingFactorSrc.OneMinusDstAlpha},
            {Blend.DestColor, BlendingFactorSrc.DstColor},
            {Blend.InvDestColor, BlendingFactorSrc.OneMinusDstColor},
            {Blend.SrcAlphaSat, BlendingFactorSrc.SrcAlphaSaturate},
            {Blend.BlendFactor, BlendingFactorSrc.ConstantColor},
            {Blend.InvBlendFactor, BlendingFactorSrc.OneMinusConstantColor},
            {Blend.Src1Color, BlendingFactorSrc.Src1Color},
            {Blend.InvSrc1Color, BlendingFactorSrc.OneMinusSrc1Color},
            {Blend.Src1Alpha, BlendingFactorSrc.Src1Alpha},
            {Blend.InvSrc1Alpha, BlendingFactorSrc.OneMinusSrc1Alpha},
        };
        private static readonly Dictionary<BlendOp, OpenTK.Graphics.OpenGL4.BlendEquationMode> blendOps = new Dictionary<BlendOp, BlendEquationMode>() 
        {
            {BlendOp.Add, BlendEquationMode.FuncAdd},
            {BlendOp.Subtract, BlendEquationMode.FuncSubtract},
            {BlendOp.RevSubtract, BlendEquationMode.FuncReverseSubtract},
            {BlendOp.Min, BlendEquationMode.Min},
            {BlendOp.Max, BlendEquationMode.Max},
        };
        private static readonly Dictionary<StencilOperation, StencilOp> stencilOperations = new Dictionary<StencilOperation, StencilOp>() 
        {
            {StencilOperation.Decr, StencilOp.Decr},
            {StencilOperation.DecrSat, StencilOp.DecrWrap},
            {StencilOperation.Incr, StencilOp.Incr},
            {StencilOperation.IncrSat, StencilOp.IncrWrap},
            {StencilOperation.Invert, StencilOp.Invert},
            {StencilOperation.Keep, StencilOp.Keep},
            {StencilOperation.Replace, StencilOp.Replace},
            {StencilOperation.Zero, StencilOp.Zero},
        };
        private static readonly Dictionary<ResourceUsage, BufferUsageHint> bufferUsages = new Dictionary<ResourceUsage, BufferUsageHint>() 
        {
            {ResourceUsage.Dynamic, BufferUsageHint.StreamDraw},
            {ResourceUsage.Staging, BufferUsageHint.DynamicDraw},
            {ResourceUsage.Normal, BufferUsageHint.StaticDraw},
            {ResourceUsage.Immutable, BufferUsageHint.StaticDraw},
        };
        private static readonly Dictionary<VertexElementType, ActiveAttribType> attribTypes = new Dictionary<VertexElementType, ActiveAttribType>()
        {
            { VertexElementType.Single, ActiveAttribType.Float },
            { VertexElementType.Vector2, ActiveAttribType.FloatVec2 },
            { VertexElementType.Vector3, ActiveAttribType.FloatVec3 },
            { VertexElementType.Vector4, ActiveAttribType.FloatVec4 },
        };

        internal static Tuple<PixelInternalFormat, PixelFormat, PixelType> Convert(TextureFormat format)
        {
            if (!formats.ContainsKey(format))
                throw new NotSupportedException("format is not supported");
            return formats[format];
        }
        internal static TextureFormat Convert(PixelInternalFormat internalFormat, PixelFormat pixelFormat, PixelType pixelType)
        {
            var tuple = Tuple.Create<PixelInternalFormat, PixelFormat, PixelType>(internalFormat, pixelFormat, pixelType);

            if (!formats.ContainsValue(tuple))
                throw new NotSupportedException("format is not supported");
            return formats.First((f) => f.Value == tuple).Key;
        }

        internal static SizedInternalFormat ConvertSizedInternalFormat(TextureFormat format)
        {
            if (!sizedInternalFormats.ContainsKey(format))
                throw new NotSupportedException("format is not supported");

            return sizedInternalFormats[format];
        }
        internal static TextureFormat ConvertSizedInternalFormat(SizedInternalFormat format)
        {
            if (!sizedInternalFormats.ContainsValue(format))
                throw new NotSupportedException("format is not supported");
            return sizedInternalFormats.First((f) => f.Value == format).Key;
        }

        internal static OpenTK.Graphics.OpenGL4.PrimitiveType Convert(DotGame.Graphics.PrimitiveType primitiveType)
        {
            if (!primitiveTypes.ContainsKey(primitiveType))
                throw new NotSupportedException("primitiveType is not supported");
            return primitiveTypes[primitiveType];
        }

        internal static DotGame.Graphics.PrimitiveType Convert(OpenTK.Graphics.OpenGL4.PrimitiveType primitiveType)
        {
            if (!primitiveTypes.ContainsValue(primitiveType))
                throw new NotSupportedException("primitiveType is not supported");
            return primitiveTypes.First((pT) => pT.Value == primitiveType).Key;
        }

        public static string Convert(VertexElementUsage usage)
        {
            if (!vertexElementUsages.ContainsKey(usage))
                throw new NotSupportedException("Usage is not supported.");
            return vertexElementUsages[usage];
        }
        internal static VertexElementUsage Convert(string usage)
        {
            if (!vertexElementUsages.ContainsValue(usage))
                throw new NotSupportedException("usage is not supported");

            return vertexElementUsages.First((f) => f.Value == usage).Key;
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
                throw new NotSupportedException("cullMode is not supported");

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
                throw new NotSupportedException("cullMode is not supported");

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
                throw new NotSupportedException("fillMode is not supported");

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
                throw new NotSupportedException("addressMode is not supported");

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
                throw new NotSupportedException("filters are not supported");

            return filters.First((f) => f.Value == tuple).Key;
        }

        internal static DepthFunction Convert(Comparison comparison)
        {
            if (!comparisons.ContainsKey(comparison))
                throw new NotSupportedException("indexFormat is not supported");

            return comparisons[comparison];
        }

        internal static Comparison Convert(DepthFunction comparison)
        {
            if (!comparisons.ContainsValue(comparison))
                throw new NotSupportedException("comparison are not supported");

            return comparisons.First((f) => f.Value == comparison).Key;
        }

        internal static BlendingFactorSrc Convert(Blend blend)
        {
            if (!blends.ContainsKey(blend))
                throw new NotSupportedException("indexFormat is not supported");

            return blends[blend];
        }

        internal static Blend Convert(BlendingFactorSrc blend)
        {
            if (!blends.ContainsValue(blend))
                throw new NotSupportedException("blend is not supported");

            return blends.First((f) => f.Value == blend).Key;
        }

        internal static BlendEquationMode Convert(BlendOp blendOp)
        {
            if (!blendOps.ContainsKey(blendOp))
                throw new NotSupportedException("indexFormat is not supported");

            return blendOps[blendOp];
        }

        internal static BlendOp Convert(BlendEquationMode blendOp)
        {
            if (!blendOps.ContainsValue(blendOp))
                throw new NotSupportedException("blend is not supported");

            return blendOps.First((f) => f.Value == blendOp).Key;
        }

        internal static StencilOp Convert(StencilOperation stencilOperation)
        {
            if (!stencilOperations.ContainsKey(stencilOperation))
                throw new NotSupportedException("indexFormat is not supported");

            return stencilOperations[stencilOperation];
        }

        internal static StencilOperation Convert(StencilOp stencilOperation)
        {
            if (!stencilOperations.ContainsValue(stencilOperation))
                throw new NotSupportedException("stencilOperation is not supported");

            return stencilOperations.First((f) => f.Value == stencilOperation).Key;
        }

        internal static BufferUsageHint Convert(ResourceUsage bufferUsage)
        {
            if (!bufferUsages.ContainsKey(bufferUsage))
                throw new NotSupportedException("bufferUsage is not supported");

            return bufferUsages[bufferUsage];
        }

        internal static ResourceUsage Convert(BufferUsageHint bufferUsage)
        {
            if (!bufferUsages.ContainsValue(bufferUsage))
                throw new NotSupportedException("bufferUsage is not supported");

            return bufferUsages.First((f) => f.Value == bufferUsage).Key;
        }

        internal static ActiveAttribType Convert(VertexElementType type)
        {
            if (!attribTypes.ContainsKey(type))
                throw new NotSupportedException("type is not supported");

            return attribTypes[type];
        }
        internal static VertexElementType Convert(ActiveAttribType type)
        {
            if (!attribTypes.ContainsValue(type))
                throw new NotSupportedException("type is not supported");

            return attribTypes.First((f) => f.Value == type).Key;
        }
    }
}
