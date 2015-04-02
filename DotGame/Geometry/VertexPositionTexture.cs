using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Geometry 
{
    [Serializable]
    public struct VertexPositionTexture : IVertexType, IEquatable<VertexPositionTexture>
    {
        public Vector3 Position;
        public Vector2 UV;

        public static readonly VertexDescription Description = new VertexDescription(
            new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3),
            new VertexElement(VertexElementUsage.TexCoord, 0, VertexElementType.Vector2));

        public VertexDescription VertexDescription { get { return Description; } }

        public VertexPositionTexture(Vector3 position, Vector2 uv)
        {
            this.Position = position;
            this.UV = uv;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexPositionTexture)
                return Equals((VertexPositionTexture)obj);
            return false;
        }

        public bool Equals(VertexPositionTexture other)
        {
            return Position == other.Position && UV == other.UV;
        }

        public static bool operator ==(VertexPositionTexture a, VertexPositionTexture b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexPositionTexture a, VertexPositionTexture b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Position.GetHashCode();
                hash = hash * 23 + UV.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[Position: ");
            str.Append(Position);
            str.Append(", UV: ");
            str.Append(UV);
            str.Append("]");
            return str.ToString();
        }
    }
}
