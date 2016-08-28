using DotGame.Graphics;
using System;
using System.Numerics;
using System.Text;

namespace DotGame.Geometry
{
    [Serializable]
    public struct VertexPosition : IVertexType, IEquatable<VertexPosition>
    {
        public Vector3 Position;

        public static readonly VertexDescription Description = new VertexDescription(
            new VertexElement(VertexElementUsage.Position, 0, VertexElementType.Vector3));

        public VertexDescription VertexDescription { get { return Description; } }

        public VertexPosition(Vector3 position)
        {
            this.Position = position;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexPosition)
                return Equals((VertexPosition)obj);
            return false;
        }

        public bool Equals(VertexPosition other)
        {
            return Position == other.Position;
        }

        public static bool operator ==(VertexPosition a, VertexPosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(VertexPosition a, VertexPosition b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Position.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[Position: ");
            str.Append(Position);
            str.Append("]");
            return str.ToString();
        }
    }
}
