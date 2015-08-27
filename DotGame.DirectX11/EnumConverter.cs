using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;

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
            {TextureFormat.DXT1, Format.BC1_UNorm},
            {TextureFormat.DXT3, Format.BC2_UNorm},
            {TextureFormat.DXT5, Format.BC3_UNorm},
            { TextureFormat.Depth16, Format.R16_Typeless },
            { TextureFormat.Depth24Stencil8, Format.R24G8_Typeless },
            { TextureFormat.Depth32, Format.R32_Typeless }
            // TODO (henrik1235) Mehr TextureFormate hinzufügen
        };

        private static readonly Dictionary<TextureFormat, Format> depthFormats = new Dictionary<TextureFormat, Format>()
        {
            { TextureFormat.Depth16, Format.D16_UNorm},
            { TextureFormat.Depth24Stencil8, Format.D24_UNorm_S8_UInt },
            { TextureFormat.Depth32, Format.D32_Float }
        };

        private static readonly Dictionary<TextureFormat, Format> depthShaderViewFormats = new Dictionary<TextureFormat, Format>()
        {
            { TextureFormat.Depth16, Format.R16_UNorm },
            { TextureFormat.Depth24Stencil8, Format.R24_UNorm_X8_Typeless },
            { TextureFormat.Depth32, Format.R32_Float }
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

        private static readonly Dictionary<DotGame.Graphics.CullMode, SharpDX.Direct3D11.CullMode> cullModes = new Dictionary<DotGame.Graphics.CullMode, SharpDX.Direct3D11.CullMode>()
        {
            { DotGame.Graphics.CullMode.None, SharpDX.Direct3D11.CullMode.None },
            { DotGame.Graphics.CullMode.Back, SharpDX.Direct3D11.CullMode.Back },
            { DotGame.Graphics.CullMode.Front, SharpDX.Direct3D11.CullMode.Front },
        };

        private static readonly Dictionary<DotGame.Graphics.FillMode, SharpDX.Direct3D11.FillMode> fillModes = new Dictionary<Graphics.FillMode, SharpDX.Direct3D11.FillMode>()
        {
            { DotGame.Graphics.FillMode.WireFrame, SharpDX.Direct3D11.FillMode.Wireframe },
            { DotGame.Graphics.FillMode.Solid, SharpDX.Direct3D11.FillMode.Solid },
        };

        private static readonly Dictionary<DotGame.Graphics.DepthWriteMask, SharpDX.Direct3D11.DepthWriteMask> depthWriteMasks = new Dictionary<Graphics.DepthWriteMask, SharpDX.Direct3D11.DepthWriteMask>()
        {
            { DotGame.Graphics.DepthWriteMask.Zero, SharpDX.Direct3D11.DepthWriteMask.Zero },
            { DotGame.Graphics.DepthWriteMask.All, SharpDX.Direct3D11.DepthWriteMask.All },
        };

        private static readonly Dictionary<DotGame.Graphics.StencilOperation, SharpDX.Direct3D11.StencilOperation> stencilOperations = new Dictionary<DotGame.Graphics.StencilOperation, SharpDX.Direct3D11.StencilOperation>()
        {
            { DotGame.Graphics.StencilOperation.Zero, SharpDX.Direct3D11.StencilOperation.Zero },
            { DotGame.Graphics.StencilOperation.Replace, SharpDX.Direct3D11.StencilOperation.Replace },
            { DotGame.Graphics.StencilOperation.Keep, SharpDX.Direct3D11.StencilOperation.Keep },
            { DotGame.Graphics.StencilOperation.Invert, SharpDX.Direct3D11.StencilOperation.Invert },
            { DotGame.Graphics.StencilOperation.IncrSat, SharpDX.Direct3D11.StencilOperation.IncrementAndClamp },
            { DotGame.Graphics.StencilOperation.DecrSat, SharpDX.Direct3D11.StencilOperation.DecrementAndClamp},
            { DotGame.Graphics.StencilOperation.Incr, SharpDX.Direct3D11.StencilOperation.Increment},
            { DotGame.Graphics.StencilOperation.Decr, SharpDX.Direct3D11.StencilOperation.Decrement},
        };

        private static readonly Dictionary<DotGame.Graphics.BlendOp, SharpDX.Direct3D11.BlendOperation> blendOperations = new Dictionary<DotGame.Graphics.BlendOp, SharpDX.Direct3D11.BlendOperation>()
        {
            { DotGame.Graphics.BlendOp.Add, SharpDX.Direct3D11.BlendOperation.Add },
            { DotGame.Graphics.BlendOp.Max, SharpDX.Direct3D11.BlendOperation.Maximum },
            { DotGame.Graphics.BlendOp.Min, SharpDX.Direct3D11.BlendOperation.Minimum },
            { DotGame.Graphics.BlendOp.RevSubtract, SharpDX.Direct3D11.BlendOperation.ReverseSubtract },
            { DotGame.Graphics.BlendOp.Subtract, SharpDX.Direct3D11.BlendOperation.Subtract },
        };

        private static readonly Dictionary<DotGame.Graphics.Blend, SharpDX.Direct3D11.BlendOption> blendOptions = new Dictionary<DotGame.Graphics.Blend, SharpDX.Direct3D11.BlendOption>() 
        {
             { DotGame.Graphics.Blend.BlendFactor,    SharpDX.Direct3D11.BlendOption.BlendFactor   },
             { DotGame.Graphics.Blend.DestAlpha,      SharpDX.Direct3D11.BlendOption.DestinationAlpha  },
             { DotGame.Graphics.Blend.DestColor,      SharpDX.Direct3D11.BlendOption.DestinationColor     },
             { DotGame.Graphics.Blend.InvBlendFactor, SharpDX.Direct3D11.BlendOption.InverseBlendFactor },
             { DotGame.Graphics.Blend.InvDestAlpha,   SharpDX.Direct3D11.BlendOption.InverseDestinationAlpha  },
             { DotGame.Graphics.Blend.InvDestColor,   SharpDX.Direct3D11.BlendOption.InverseDestinationColor  },
             { DotGame.Graphics.Blend.InvSrc1Alpha,   SharpDX.Direct3D11.BlendOption.InverseSecondarySourceAlpha  },
             { DotGame.Graphics.Blend.InvSrc1Color,   SharpDX.Direct3D11.BlendOption.InverseSecondarySourceColor  },
             { DotGame.Graphics.Blend.InvSrcAlpha,    SharpDX.Direct3D11.BlendOption.InverseSourceAlpha },
             { DotGame.Graphics.Blend.InvSrcColor,    SharpDX.Direct3D11.BlendOption.InverseSourceColor   },
             { DotGame.Graphics.Blend.One,            SharpDX.Direct3D11.BlendOption.One        },
             { DotGame.Graphics.Blend.Src1Alpha,      SharpDX.Direct3D11.BlendOption.SecondarySourceAlpha     },
             { DotGame.Graphics.Blend.Src1Color,      SharpDX.Direct3D11.BlendOption.SecondarySourceColor     },
             { DotGame.Graphics.Blend.SrcAlpha,       SharpDX.Direct3D11.BlendOption.SourceAlpha      },
             { DotGame.Graphics.Blend.SrcAlphaSat,    SharpDX.Direct3D11.BlendOption.SourceAlphaSaturate   },
             { DotGame.Graphics.Blend.SrcColor,       SharpDX.Direct3D11.BlendOption.SourceColor      },
             { DotGame.Graphics.Blend.Zero,           SharpDX.Direct3D11.BlendOption.Zero         },
        };

        private static Dictionary<Graphics.ResourceUsage, SharpDX.Direct3D11.ResourceUsage> resourceUsages = new Dictionary<Graphics.ResourceUsage, SharpDX.Direct3D11.ResourceUsage>()
        {
            { Graphics.ResourceUsage.Normal, SharpDX.Direct3D11.ResourceUsage.Default },
            { Graphics.ResourceUsage.Dynamic, SharpDX.Direct3D11.ResourceUsage.Dynamic },
            { Graphics.ResourceUsage.Staging, SharpDX.Direct3D11.ResourceUsage.Staging },
            { Graphics.ResourceUsage.Immutable, SharpDX.Direct3D11.ResourceUsage.Immutable },
        };

        private static Dictionary<Graphics.ResourceUsage, CpuAccessFlags> accessFlags = new Dictionary<Graphics.ResourceUsage, CpuAccessFlags>()
        {
            { Graphics.ResourceUsage.Normal, CpuAccessFlags.None},
            { Graphics.ResourceUsage.Immutable, CpuAccessFlags.None},
            { Graphics.ResourceUsage.Dynamic, CpuAccessFlags.Write},
            { Graphics.ResourceUsage.Staging, CpuAccessFlags.Write},
        };

        public static SharpDX.Direct3D11.DeviceCreationFlags Convert(DotGame.Graphics.DeviceCreationFlags flags)
        {
            SharpDX.Direct3D11.DeviceCreationFlags f = 0;
            if (flags.HasFlag(DotGame.Graphics.DeviceCreationFlags.Debug))
                f |= SharpDX.Direct3D11.DeviceCreationFlags.Debug;
            return f;
        }

        public static Format Convert(TextureFormat format)
        {
            Format f;
            if (!textureFormats.TryGetValue(format, out f))
                throw new NotSupportedException("Format is not supported.");
            return f;
        }

        public static Format ConvertDepthView(TextureFormat format)
        {
            Format f;
            if (!depthFormats.TryGetValue(format, out f))
                throw new NotSupportedException("Format is not supported.");
            return f;
        }

        public static Format ConvertDepthShaderView(TextureFormat format)
        {
            Format f;
            if (!depthShaderViewFormats.TryGetValue(format, out f))
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

        public static SharpDX.Direct3D11.CullMode Convert(DotGame.Graphics.CullMode cullMode)
        {
            SharpDX.Direct3D11.CullMode c;
            if (!cullModes.TryGetValue(cullMode, out c))
                throw new NotSupportedException("Cull mode not supported.");
            return c;
        }

        public static SharpDX.Direct3D11.FillMode Convert(DotGame.Graphics.FillMode fillMode)
        {
            SharpDX.Direct3D11.FillMode f;
            if (!fillModes.TryGetValue(fillMode, out f))
                throw new NotSupportedException("Fill mode not supported.");
            return f;
        }

        public static SharpDX.Direct3D11.DepthWriteMask Convert(DotGame.Graphics.DepthWriteMask depthWriteMask)
        {
            SharpDX.Direct3D11.DepthWriteMask f;
            if (!depthWriteMasks.TryGetValue(depthWriteMask, out f))
                throw new NotSupportedException("Depth write mask not supported.");
            return f;
        }

        public static SharpDX.Direct3D11.StencilOperation Convert(DotGame.Graphics.StencilOperation stencilOperation)
        {
            SharpDX.Direct3D11.StencilOperation f;
            if (!stencilOperations.TryGetValue(stencilOperation, out f))
                throw new NotSupportedException("Stencil operation not supported.");
            return f;
        }

        public static SharpDX.Direct3D11.BlendOperation Convert(DotGame.Graphics.BlendOp blendOp)
        {
            SharpDX.Direct3D11.BlendOperation f;
            if (!blendOperations.TryGetValue(blendOp, out f))
                throw new NotSupportedException("Blend operation not supported.");
            return f;
        }

        public static SharpDX.Direct3D11.BlendOption Convert(DotGame.Graphics.Blend blend)
        {
            SharpDX.Direct3D11.BlendOption f;
            if (!blendOptions.TryGetValue(blend, out f))
                throw new NotSupportedException("Blend option not supported.");
            return f;
        }

        public static SharpDX.Direct3D11.ColorWriteMaskFlags Convert(DotGame.Graphics.ColorWriteMaskFlags colorWriteFlags)
        {
            SharpDX.Direct3D11.ColorWriteMaskFlags f = 0;
            if (colorWriteFlags.HasFlag(DotGame.Graphics.ColorWriteMaskFlags.Red))
                f |= SharpDX.Direct3D11.ColorWriteMaskFlags.Red;
            if (colorWriteFlags.HasFlag(DotGame.Graphics.ColorWriteMaskFlags.Green))
                f |= SharpDX.Direct3D11.ColorWriteMaskFlags.Green;
            if (colorWriteFlags.HasFlag(DotGame.Graphics.ColorWriteMaskFlags.Blue))
                f |= SharpDX.Direct3D11.ColorWriteMaskFlags.Blue;
            if (colorWriteFlags.HasFlag(DotGame.Graphics.ColorWriteMaskFlags.Alpha))
                f |= SharpDX.Direct3D11.ColorWriteMaskFlags.Alpha;
            return f;
        }

        public static SharpDX.Direct3D11.ResourceUsage Convert(Graphics.ResourceUsage usage)
        {
            SharpDX.Direct3D11.ResourceUsage f;
            if (!resourceUsages.TryGetValue(usage, out f))
                throw new NotSupportedException("Buffer usage not supported.");
            return f;
        }
        public static Graphics.ResourceUsage Convert(SharpDX.Direct3D11.ResourceUsage usage)
        {
            if (!resourceUsages.ContainsValue(usage))
                throw new NotImplementedException();
            return resourceUsages.First((f) => f.Value == usage).Key;
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

        public static DotGame.Graphics.CullMode Convert(SharpDX.Direct3D11.CullMode cullMode)
        {
            if (!cullModes.ContainsValue(cullMode))
                throw new NotImplementedException();
            return cullModes.First((f) => f.Value == cullMode).Key;
        }

        public static CpuAccessFlags ConvertToAccessFlag(Graphics.ResourceUsage usage)
        {
            CpuAccessFlags accessFlag;
            if (!accessFlags.TryGetValue(usage, out accessFlag))
                throw new NotImplementedException();

            return accessFlag;
        }
        public static Graphics.ResourceUsage ConvertToAccessFlag(CpuAccessFlags accessFlag)
        {
            if (!accessFlags.ContainsValue(accessFlag))
                throw new NotImplementedException();
            return accessFlags.First((f) => f.Value == accessFlag).Key;
        }
    }
}
