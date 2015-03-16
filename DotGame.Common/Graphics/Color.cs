﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DotGame.Graphics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color : IEquatable<Color>
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public Color(float r, float g, float b, float a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public Color(float r, float g, float b) : this(r, g, b, 1.0f)
        {
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            this.R = (float)r / byte.MaxValue;
            this.G = (float)g / byte.MaxValue;
            this.B = (float)b / byte.MaxValue;
            this.A = (float)a / byte.MaxValue;
        }

        public Color(byte r, byte g, byte b) : this(r, g, b, byte.MaxValue)
        {
        }

        /*public static override Color operator +(Color color1, Color color2)
        {
            return new Color(color1.R + color2.R,
                             color1.G + color2.G,
                             color1.B + color2.B,
                             color1.A + color2.A);
        }*/

        public override bool Equals(object obj)
        {
            if (obj is Color)
                return Equals((Color)obj);
            return false;
        }

        public bool Equals(Color color)
        {
            return R == color.R && G == color.G && B == color.B && A == color.A;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + R.GetHashCode();
                hash = hash * 23 + G.GetHashCode();
                hash = hash * 23 + B.GetHashCode();
                hash = hash * 23 + A.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("[R: ");
            str.Append(R);
            str.Append(", G:");
            str.Append(G);
            str.Append(", B:");
            str.Append(B);
            str.Append(", A:");
            str.Append(A);
            str.Append("]");
            return str.ToString();
        }

        public static Color Lerp(Color color1, Color color2, float value)
        {
            return new Color(MathHelper.Lerp(color1.R, color2.R, value),
                             MathHelper.Lerp(color1.G, color2.G, value),
                             MathHelper.Lerp(color1.B, color2.B, value),
                             MathHelper.Lerp(color1.A, color2.A, value));
        }
    }
}
