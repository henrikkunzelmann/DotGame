using DotGame.Graphics;
using SharpDX.Direct3D11;
using System.Diagnostics;

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
                BorderColor = SharpDX.Color.FromRgba(info.BorderColor.ToRgba()),
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
