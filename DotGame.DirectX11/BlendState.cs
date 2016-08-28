using DotGame.Graphics;
using System.Diagnostics;

namespace DotGame.DirectX11
{
    public class BlendState : GraphicsObject, IBlendState
    {
        public BlendStateInfo Info { get; private set; }

        internal SharpDX.Direct3D11.BlendState Handle { get; private set; }

        public BlendState(GraphicsDevice graphicsDevice, BlendStateInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Info = info;

            SharpDX.Direct3D11.BlendStateDescription desc = new SharpDX.Direct3D11.BlendStateDescription();
            desc.AlphaToCoverageEnable = info.AlphaToCoverageEnable;
            desc.IndependentBlendEnable = info.IndependentBlendEnable;

            for (int i = 0; i < info.RenderTargets.Length; i++)
            {
                RTBlendInfo rtInfo = info.RenderTargets[i];
                desc.RenderTarget[i] = new SharpDX.Direct3D11.RenderTargetBlendDescription()
                {
                    IsBlendEnabled = rtInfo.IsBlendEnabled,
                    SourceBlend = EnumConverter.Convert(rtInfo.SrcBlend),
                    DestinationBlend = EnumConverter.Convert(rtInfo.DestBlend),
                    BlendOperation = EnumConverter.Convert(rtInfo.BlendOp),
                    SourceAlphaBlend = EnumConverter.Convert(rtInfo.SrcBlendAlpha),
                    DestinationAlphaBlend = EnumConverter.Convert(rtInfo.DestBlendAlpha),
                    AlphaBlendOperation = EnumConverter.Convert(rtInfo.BlendOpAlpha),
                    RenderTargetWriteMask = EnumConverter.Convert(rtInfo.RenderTargetWriteMask)
                };
            }

            Handle = new SharpDX.Direct3D11.BlendState(graphicsDevice.Device, desc);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
