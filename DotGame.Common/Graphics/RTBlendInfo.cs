namespace DotGame.Graphics
{
    public struct RTBlendInfo
    {
        public bool IsBlendEnabled;
        public Blend SrcBlend;
        public Blend DestBlend;
        public BlendOp BlendOp;
        public Blend SrcBlendAlpha;
        public Blend DestBlendAlpha;
        public BlendOp BlendOpAlpha;
        public ColorWriteMaskFlags RenderTargetWriteMask;
    }
}