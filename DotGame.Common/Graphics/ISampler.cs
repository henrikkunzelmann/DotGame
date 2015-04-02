using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public interface ISampler
    {
        SamplerType Type { get; }
        TextureFilter MinFilter { get; }
        TextureFilter MagFilter { get; }
        TextureFilter MipFilter { get; }
        Comparison ComparisonFunction { get; }

        Color BorderColor { get; }
        AddressMode AddressU { get; }
        AddressMode AddressV { get; }
        AddressMode AddressW { get; }

        int MinimumLod { get; }
        int MaximumLod { get; }
        int MaximumAnisotropy { get; }
    }
}
