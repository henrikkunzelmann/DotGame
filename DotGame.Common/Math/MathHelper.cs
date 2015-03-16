using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public static class MathHelper
    {
        public static readonly float PI = (float)Math.PI;

        public static float DegressToRadians(float degress)
        {
            return degress / 180f * PI;
        }

        public static float RadiansToDegress(float radians)
        {
            return radians / PI * 180f;
        }

        public static float Lerp(float A, float B, float V)
        {
            return A + (B - A) * V;
        }
    }
}
