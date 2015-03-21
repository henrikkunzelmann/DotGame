using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Gibt ein Element der VertexDescription an.
    /// </summary>
    [Serializable]
    public struct VertexElement : IEquatable<VertexElement>
    {
        /// <summary>
        /// Der Zweck für den dieses Element benutzt wird.
        /// </summary>
        public VertexElementUsage Usage { get; set; }
        public int UsageIndex { get; set; }
        public VertexElementType Type { get; set;  }

        public VertexElement(VertexElementUsage usage, int usageIndex, VertexElementType type)
            : this()
        {
            this.Usage = usage;
            this.UsageIndex = usageIndex;
            this.Type = type;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is VertexElement)
                return Equals((VertexElement)obj);
            return false;
        }

        public bool Equals(VertexElement other)
        {
            return Usage == other.Usage && UsageIndex == other.UsageIndex && Type == other.Type;
        }

        public static bool operator ==(VertexElement a, VertexElement b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexElement a, VertexElement b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Usage.GetHashCode();
                hash = hash * 23 + UsageIndex.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[Usage: ");
            str.Append(Usage);
            str.Append(", UsageIndex: ");
            str.Append(UsageIndex);
            str.Append(", Type: ");
            str.Append(Type);
            str.Append("]");
            return str.ToString();
        }
    }
}
