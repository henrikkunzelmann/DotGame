using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public static class MathHelper
    {
        public static float Lerp(float A, float B, float V)
        {
            return A + (B - A) * V;
        }
    }
}
