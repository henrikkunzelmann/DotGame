using DotGame.Graphics;
using SharpDX.Direct3D11;
using System.Diagnostics;

namespace DotGame.DirectX11
{
    public class RasterizerState : GraphicsObject, IRasterizerState
    {
        public RasterizerStateInfo Info { get; private set; }

        internal SharpDX.Direct3D11.RasterizerState Handle { get; private set; }

        public RasterizerState(GraphicsDevice graphicsdevice, RasterizerStateInfo info)
            : base(graphicsdevice, new StackTrace(1))
        {
            this.Info = info;

            Handle = new SharpDX.Direct3D11.RasterizerState(graphicsdevice.Device, new RasterizerStateDescription()
            {
                FillMode = EnumConverter.Convert(info.FillMode),
                CullMode = EnumConverter.Convert(info.CullMode),
                IsFrontCounterClockwise = info.IsFrontCounterClockwise,
                DepthBias = info.DepthBias,
                DepthBiasClamp = info.DepthBiasClamp,
                SlopeScaledDepthBias = info.SlopeScaledDepthBias,
                IsAntialiasedLineEnabled = info.IsAntialiasedLineEnabled,
                IsDepthClipEnabled = info.IsDepthClipEnabled,
                IsScissorEnabled = info.IsScissorEnabled,
                IsMultisampleEnabled = info.IsMultisampleEnabled
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
