using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotGame.Graphics
{
    public struct BlendStateInfo
    {
        public static BlendStateInfo Default { get; private set; }
        public static BlendStateInfo Add { get; private set; }
        public static BlendStateInfo AlphaBlend { get; private set; }

        public bool AlphaToCoverageEnable;
        public bool IndependentBlendEnable;

        public RTBlendInfo[] RenderTargets;

        public BlendStateInfo(int renderTargetCount)
            : this()
        {
            this.RenderTargets = new RTBlendInfo[renderTargetCount];
        }

        static BlendStateInfo()
        {
            var defaultState = new BlendStateInfo(1);
            defaultState.AlphaToCoverageEnable = false;
            defaultState.IndependentBlendEnable = false;
            defaultState.RenderTargets[0].IsBlendEnabled = false;
            defaultState.RenderTargets[0].SrcBlend = Blend.Zero;
            defaultState.RenderTargets[0].DestBlend = Blend.Zero;
            defaultState.RenderTargets[0].BlendOp = BlendOp.Add;
            defaultState.RenderTargets[0].SrcBlendAlpha = Blend.One;
            defaultState.RenderTargets[0].DestBlendAlpha = Blend.Zero;
            defaultState.RenderTargets[0].BlendOpAlpha = BlendOp.Add;
            defaultState.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            Default = defaultState;

            var add = new BlendStateInfo(1);
            add.AlphaToCoverageEnable = false;
            add.IndependentBlendEnable = false;
            add.RenderTargets[0].IsBlendEnabled = true;
            add.RenderTargets[0].SrcBlend = Blend.One;
            add.RenderTargets[0].DestBlend = Blend.One;
            add.RenderTargets[0].BlendOp = BlendOp.Add;
            add.RenderTargets[0].SrcBlendAlpha = Blend.One;
            add.RenderTargets[0].DestBlendAlpha = Blend.Zero;
            add.RenderTargets[0].BlendOpAlpha = BlendOp.Add;
            add.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            Add = add;

            var alphaBlend = new BlendStateInfo(1);
            alphaBlend.AlphaToCoverageEnable = false;
            alphaBlend.IndependentBlendEnable = false;
            alphaBlend.RenderTargets[0].IsBlendEnabled = true;
            alphaBlend.RenderTargets[0].SrcBlend = Blend.SrcAlpha;
            alphaBlend.RenderTargets[0].DestBlend = Blend.InvSrcAlpha;
            alphaBlend.RenderTargets[0].BlendOp = BlendOp.Add;
            alphaBlend.RenderTargets[0].SrcBlendAlpha = Blend.SrcAlpha;
            alphaBlend.RenderTargets[0].DestBlendAlpha = Blend.One;
            alphaBlend.RenderTargets[0].BlendOpAlpha = BlendOp.Add;
            alphaBlend.RenderTargets[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            AlphaBlend = alphaBlend;
        }
    }
}
