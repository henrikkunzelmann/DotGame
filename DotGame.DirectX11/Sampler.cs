using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using DotGame.Graphics;
using DotGame.Utils;
using Comparison = DotGame.Graphics.Comparison;
using Color = DotGame.Graphics.Color;

namespace DotGame.DirectX11
{
    public class Sampler : GraphicsObject, ISampler
    {
        public SamplerType Type { get; private set; }
        public TextureFilter MinFilter { get; private set; }
        public TextureFilter MagFilter { get; private set; }
        public TextureFilter MipFilter { get; private set; }
        public Comparison ComparisonFunction { get; private set; }
        public Color BorderColor { get; private set; }
        public AddressMode AddressU { get; private set; }
        public AddressMode AddressV { get; private set; }
        public AddressMode AddressW { get; private set; }

        public int MinimumLod { get; private set; }
        public int MaximumLod { get; private set; }
        public int MaximumAnisotropy { get; private set; }

        public Sampler(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {

        }

        protected override void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }
    }
}
