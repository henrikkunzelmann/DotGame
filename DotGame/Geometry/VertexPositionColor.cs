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
    public struct VertexPositionColor : IVertexType, IEquatable<VertexPositionColor>
    {
        public Vector3 Position;
        public Color Color;

        public static readonly VertexDescription Description = new VertexDescription(
            new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3),
            new VertexElement(VertexElementUsage.Color, 0, VertexElementType.Vector4));

        public VertexDescription VertexDescription { get { return Description; } }

        public VertexPositionColor(Vector3 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexPositionColor)
                return Equals((VertexPositionColor)obj);
            return false;
        }

        public bool Equals(VertexPositionColor other)
        {
            return Position == other.Position && Color == other.Color;
        }

        public static bool operator ==(VertexPositionColor a, VertexPositionColor b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexPositionColor a, VertexPositionColor b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Position.GetHashCode();
                hash = hash * 23 + Color.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[Position: ");
            str.Append(Position);
            str.Append(", Color: ");
            str.Append(Color);
            str.Append("]");
            return str.ToString();
        }
    }
}
