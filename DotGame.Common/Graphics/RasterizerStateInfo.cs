using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public struct RasterizerStateInfo
    {
        public FillMode FillMode;
        public CullMode CullMode;
        public bool IsFrontCounterClockwise;
        public int DepthBias;
        public float DepthBiasClamp;
        public float SlopeScaledDepthBias;
        public bool IsDepthClipEnable;
        public bool IsScissorEnable;
        public bool IsMultisampleEnable;
        public bool IsAntialiasedLineEnable;
    }
}