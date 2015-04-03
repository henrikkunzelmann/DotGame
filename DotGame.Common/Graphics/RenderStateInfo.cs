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
        public CullMode CullMode;

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
            return this.Shader == other.Shader && this.PrimitiveType == other.PrimitiveType;
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
                return hash;
            }
        }
    }
}
