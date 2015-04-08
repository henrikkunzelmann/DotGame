using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Speichert die ganze Information über die Pipeline.
    /// </summary>
    public struct RenderStateInfo : IEquatable<RenderStateInfo>
    {
        public IShader Shader;
        public PrimitiveType PrimitiveType;
        public IRasterizerState Rasterizer;
        public IDepthStencilState DepthStencil;
        public IBlendState Blend;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is RenderStateInfo)
                return Equals((RenderStateInfo)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(RenderStateInfo other)
        {
            return this.Shader == other.Shader
                && this.PrimitiveType == other.PrimitiveType
                && this.Rasterizer == other.Rasterizer
                && this.DepthStencil == other.DepthStencil
                && this.Blend == other.Blend;
        }

        public static bool operator ==(RenderStateInfo a, RenderStateInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RenderStateInfo a, RenderStateInfo b)
        {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Shader.GetHashCode();
                hash = hash * 23 + PrimitiveType.GetHashCode();
                hash = hash * 23 + Rasterizer.GetHashCode();
                hash = hash * 23 + DepthStencil.GetHashCode();
                hash = hash * 23 + Blend.GetHashCode();
                return hash;
            }
        }
    }
}
