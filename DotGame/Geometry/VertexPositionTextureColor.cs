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
    public struct VertexPositionTextureColor : IVertexType, IEquatable<VertexPositionTextureColor>
    {
        public Vector3 Position;
        public Vector2 UV;
        public Color Color;

        public static readonly VertexDescription Description = new VertexDescription(
            new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3),
            new VertexElement(VertexElementUsage.TexCoord, 0, VertexElementType.Vector2),
            new VertexElement(VertexElementUsage.Color, 0, VertexElementType.Vector4));

        public VertexDescription VertexDescription { get { return Description; } }

        public VertexPositionTextureColor(Vector3 position, Vector2 uv, Color color)
        {
            this.Position = position;
            this.UV = uv;
            this.Color = color;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexPositionTextureColor)
                return Equals((VertexPositionTextureColor)obj);
            return false;
        }

        public bool Equals(VertexPositionTextureColor other)
        {
            return Position == other.Position && UV == other.UV && Color == other.Color;
        }

        public static bool operator ==(VertexPositionTextureColor a, VertexPositionTextureColor b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexPositionTextureColor a, VertexPositionTextureColor b)
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
                hash = hash * 23 + Color.GetHashCode();
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
            str.Append(", Color: ");
            str.Append(Color);
            str.Append("]");
            return str.ToString();
        }
    }
}
