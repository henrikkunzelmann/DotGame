using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        #region Properties
        public float Left
        {
            get { return X; }
            set { X = value; }
        }

        public float Right
        {
            get { return (X + Width); }
            set { Width = value - X; }
        }

        public float Top
        {
            get { return Y; }
            set { Y = value; }
        }

        public float Bottom
        {
            get { return (Y + Height); }
            set { Height = value - Y; }
        }

        public Vector2 Center
        {
            get { return new Vector2(X + Width / 2, Y + Height / 2); }
        }
        #endregion

        public Rectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        #region Intersects & Contains
        public bool Intersects(Rectangle other)
        {
            return Intersects(ref other);
        }

        public bool Intersects(ref Rectangle other)
        {
            return other.Left < Right &&
                   Left < other.Right &&
                   other.Top < Bottom &&
                   Top < other.Bottom;
        }

        public bool Contains(Vector2 value)
        {
            return value.X >= Left && value.X <= Right &&
                   value.Y >= Top && value.Y <= Bottom;
        }

        public bool Contains(Rectangle other)
        {
            return Contains(ref other);
        }

        public bool Contains(ref Rectangle other)
        {
            return X <= other.X && other.Right <= Right &&
                   Y <= other.Y && other.Bottom <= Bottom;
        }
        #endregion

        public void Expand(float Horizontal, float Vertical)
        {
            X -= Horizontal;
            Y -= Vertical;
            Width += Horizontal * 2;
            Height += Vertical * 2;
        }

        public Vector2[] GetCorners()
        {
            return new Vector2[] 
            { 
                new Vector2(Left, Top),
                new Vector2(Right, Top),
                new Vector2(Right, Bottom),
                new Vector2(Left, Bottom)
            };
        }

        public bool Equals(Rectangle other)
        {
            return (X == other.X && Y == other.Y && Width == other.Width && Height == other.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else
                if (obj is Rectangle)
                    return Equals((Rectangle)obj);
                else
                    return false;
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
                return hash;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[X: ");
            builder.Append(X);
            builder.Append(", Y: ");
            builder.Append(Y);
            builder.Append(", Width: ");
            builder.Append(Width);
            builder.Append(", Height: ");
            builder.Append(Height);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
