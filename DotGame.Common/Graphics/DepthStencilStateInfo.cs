using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public struct DepthStencilStateInfo
    {
        public bool IsDepthEnabled;
        public DepthWriteMask DepthWriteMask;
        public Comparison DepthComparsion;
        public bool IsStencilEnabled;
        public byte StencilReadMask;
        public byte StencilWriteMask;
        public DepthStencilOperator FrontFace;
        public DepthStencilOperator BackFace;
    }

    public struct DepthStencilOperator
    {
        public StencilOperation FailOperation;
        public StencilOperation DepthFailOperation;
        public StencilOperation PassOperation;
        public Comparison Comparsion;
    }
}
