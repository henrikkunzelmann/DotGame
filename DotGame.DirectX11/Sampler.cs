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
        public SamplerInfo Info { get; private set;}

        internal SamplerState Handle { get; private set; }

        public Sampler(GraphicsDevice graphicsDevice, SamplerInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Info = info;
        
            Handle = new SamplerState(graphicsDevice.Device, new SamplerStateDescription()
            {
                Filter = EnumConverter.Convert(info.Type, info.MinFilter, info.MagFilter, info.MipFilter),
                AddressU = EnumConverter.Convert(info.AddressU),
                AddressV = EnumConverter.Convert(info.AddressV),
                AddressW = EnumConverter.Convert(info.AddressW),

                BorderColor = new SharpDX.Mathematics.Interop.RawColor4(info.BorderColor.R, info.BorderColor.G, info.BorderColor.B, info.BorderColor.A),
                ComparisonFunction = EnumConverter.Convert(info.ComparisonFunction),
                MaximumAnisotropy = info.MaximumAnisotropy,
                MinimumLod = info.MinimumLod,
                MaximumLod = info.MaximumLod,
                MipLodBias = info.MipLodBias
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
