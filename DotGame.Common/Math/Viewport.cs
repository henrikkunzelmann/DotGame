using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Viewport : IEquatable<Viewport>
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float MinDepth;
        public float MaxDepth;

        #region Operators
        public static bool operator ==(Viewport a, Viewport b)
        {
            return a.X == b.X
                && a.Y == b.Y
                && a.Width == b.Width
                && a.Height == b.Height
                && a.MinDepth == b.MinDepth
                && a.MaxDepth == b.MaxDepth;
        }

        public static bool operator !=(Viewport a, Viewport b)
        {
            return !(a == b);
        }
        #endregion

        public Viewport(float X, float Y, float Width, float Height, float MinDepth, float MaxDepth)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.MinDepth = MinDepth;
            this.MaxDepth = MaxDepth;
        }

        public bool Equals(Viewport other)
        {
            return X == other.X
                && Y == other.Y
                && Width == other.Width
                && Height == other.Height
                && MinDepth == other.MinDepth
                && MaxDepth == other.MaxDepth;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Viewport))
                return false;

            var other = (Viewport)obj;
            return X == other.X
                && Y == other.Y
                && Width == other.Width
                && Height == other.Height
                && MinDepth == other.MinDepth
                && MaxDepth == other.MaxDepth;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                hash = hash * 23 + MinDepth.GetHashCode();
                hash = hash * 23 + MaxDepth.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[X:");
            builder.Append(X);
            builder.Append(" Y:");
            builder.Append(Y);
            builder.Append(" Width:");
            builder.Append(Width);
            builder.Append(" Height:");
            builder.Append(Height);
            builder.Append(" MinDepth:");
            builder.Append(MinDepth);
            builder.Append(" MaxDepth:");
            builder.Append(MaxDepth);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
