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
        public bool IsDepthClipEnabled;
        public bool IsScissorEnabled;
        public bool IsMultisampleEnabled;
        public bool IsAntialiasedLineEnabled;
    }
}