using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using System.Numerics;

namespace DotGame.Geometry 
{
    [Serializable]
    public struct VertexPositionTextureNormal : IVertexType, IEquatable<VertexPositionTextureNormal>
    {
        public Vector3 Position;
        public Vector2 UV;
        public Vector3 Normal;

        public static readonly VertexDescription Description = new VertexDescription(
            new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3),
            new VertexElement(VertexElementUsage.TexCoord, 0, VertexElementType.Vector2),
            new VertexElement(VertexElementUsage.Normal, 0, VertexElementType.Vector3));

        public VertexDescription VertexDescription { get { return Description; } }

        public VertexPositionTextureNormal(Vector3 position, Vector2 uv, Vector3 normal)
        {
            this.Position = position;
            this.UV = uv;
            this.Normal = normal;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexPositionTextureNormal)
                return Equals((VertexPositionTextureNormal)obj);
            return false;
        }

        public bool Equals(VertexPositionTextureNormal other)
        {
            return Position == other.Position && UV == other.UV && Normal == other.Normal;
        }

        public static bool operator ==(VertexPositionTextureNormal a, VertexPositionTextureNormal b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexPositionTextureNormal a, VertexPositionTextureNormal b)
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
                hash = hash * 23 + Normal.GetHashCode();
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
            str.Append(", Normal: ");
            str.Append(Normal);
            str.Append("]");
            return str.ToString();
        }
    }
}
