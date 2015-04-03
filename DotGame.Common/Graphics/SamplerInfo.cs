using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    [Serializable]
    public struct SamplerInfo : IEquatable<SamplerInfo>
    {
        public SamplerType Type;
        public TextureFilter MinFilter;
        public TextureFilter MagFilter;
        public TextureFilter MipFilter;
        public AddressMode AddressU;
        public AddressMode AddressV;
        public AddressMode AddressW;
        public Color BorderColor;
        public Comparison ComparisonFunction;
        public int MaximumAnisotropy;
        public float MipLodBias;
        public float MinimumLod;
        public float MaximumLod;

        public SamplerInfo(TextureFilter minMagMipFilter)
            : this(SamplerType.Normal, minMagMipFilter, minMagMipFilter, minMagMipFilter)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minMagMipFilter)
            : this(type, minMagMipFilter, minMagMipFilter, minMagMipFilter)
        {
        }

        public SamplerInfo(TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter)
            : this(SamplerType.Normal, minFilter, magFilter, mipFilter)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter)
            : this(type, minFilter, magFilter, mipFilter, AddressMode.Wrap, AddressMode.Wrap)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, AddressMode.Wrap)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, addressW, Color.Black)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW, Color borderColor)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, addressW, borderColor, Comparison.Always)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW, Color borderColor, Comparison comparisonFunction)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, addressW, borderColor, comparisonFunction, 16)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW, Color borderColor, Comparison comparisonFunction, int maximumAnisotropy)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, addressW, borderColor, comparisonFunction, maximumAnisotropy, 0, 0, int.MaxValue)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW, Color borderColor, Comparison comparisonFunction, int maximumAnisotropy,
            float mipLodBias)
            : this(type, minFilter, magFilter, mipFilter, addressU, addressV, addressW, borderColor, comparisonFunction, maximumAnisotropy, mipLodBias, 0, float.MaxValue)
        {
        }

        public SamplerInfo(SamplerType type, TextureFilter minFilter, TextureFilter magFilter, TextureFilter mipFilter, AddressMode addressU, AddressMode addressV,
            AddressMode addressW, Color borderColor, Comparison comparisonFunction, int maximumAnisotropy,
            float mipLodBias, float minimumLod, float maximumLod)
        {
            this.Type = type;
            this.MinFilter = minFilter;
            this.MagFilter = magFilter;
            this.MipFilter = mipFilter;
            this.AddressU = addressU;
            this.AddressV = addressV;
            this.AddressW = addressW;
            this.BorderColor = borderColor;
            this.ComparisonFunction = comparisonFunction;
            this.MaximumAnisotropy = maximumAnisotropy;
            this.MipLodBias = mipLodBias;
            this.MinimumLod = minimumLod;
            this.MaximumLod = maximumLod;
        }

        public override bool Equals(object obj)
        {
            if (obj is SamplerInfo)
                return Equals((SamplerInfo)obj);
            return false;
        }

        public static bool operator ==(SamplerInfo a, SamplerInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SamplerInfo a, SamplerInfo b)
        {
            return !(a == b);
        }

        public bool Equals(SamplerInfo obj)
        {
            return obj.Type == Type &&
                obj.MinFilter == MinFilter &&
                obj.MagFilter == MagFilter &&
                obj.MipFilter == MipFilter &&
                obj.AddressU == AddressU &&
                obj.AddressV == AddressV &&
                obj.AddressW == AddressW &&
                obj.AddressU == AddressU &&
                obj.BorderColor == BorderColor &&
                obj.ComparisonFunction == ComparisonFunction &&
                obj.MaximumAnisotropy == MaximumAnisotropy &&
                obj.MipLodBias == MipLodBias &&
                obj.MinimumLod == MinimumLod &&
                obj.MaximumLod == MaximumLod;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + MinFilter.GetHashCode();
                hash = hash * 23 + MagFilter.GetHashCode();
                hash = hash * 23 + MipFilter.GetHashCode();
                hash = hash * 23 + AddressU.GetHashCode();
                hash = hash * 23 + AddressV.GetHashCode();
                hash = hash * 23 + AddressW.GetHashCode();
                hash = hash * 23 + BorderColor.GetHashCode();
                hash = hash * 23 + ComparisonFunction.GetHashCode();
                hash = hash * 23 + MaximumAnisotropy.GetHashCode();
                hash = hash * 23 + MipLodBias.GetHashCode();
                hash = hash * 23 + MinimumLod.GetHashCode();
                hash = hash * 23 + MaximumLod.GetHashCode();
                return hash;
            }
        }
    }
}
