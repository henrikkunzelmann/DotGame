using DotGame.Graphics;
using SharpDX.Direct3D11;
using System.Diagnostics;


namespace DotGame.DirectX11
{
    public class DepthStencilState : GraphicsObject, IDepthStencilState
    {
        public DepthStencilStateInfo Info { get; private set; }

        internal SharpDX.Direct3D11.DepthStencilState Handle { get; private set; }

        public DepthStencilState(GraphicsDevice graphicsDevice, DepthStencilStateInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Info = info;

            this.Handle = new SharpDX.Direct3D11.DepthStencilState(graphicsDevice.Device, new DepthStencilStateDescription()
            {
                IsDepthEnabled = info.IsDepthEnabled,
                DepthComparison = EnumConverter.Convert(info.DepthComparsion),
                DepthWriteMask = EnumConverter.Convert(info.DepthWriteMask),
                IsStencilEnabled = info.IsStencilEnabled,
                StencilReadMask = info.StencilReadMask,
                StencilWriteMask = info.StencilWriteMask,
                FrontFace = new DepthStencilOperationDescription()
                {
                    Comparison = EnumConverter.Convert(info.FrontFace.Comparsion),
                    FailOperation = EnumConverter.Convert(info.FrontFace.FailOperation),
                    PassOperation = EnumConverter.Convert(info.FrontFace.PassOperation),
                    DepthFailOperation = EnumConverter.Convert(info.FrontFace.DepthFailOperation)
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    Comparison = EnumConverter.Convert(info.BackFace.Comparsion),
                    FailOperation = EnumConverter.Convert(info.BackFace.FailOperation),
                    PassOperation = EnumConverter.Convert(info.BackFace.PassOperation),
                    DepthFailOperation = EnumConverter.Convert(info.BackFace.DepthFailOperation)
                }
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (Handle != null)
                Handle.Dispose();
        }
    }
}
