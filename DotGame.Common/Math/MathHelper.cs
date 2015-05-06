using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public static class MathHelper
    {
        public const float PI = (float)Math.PI;

        public const float DEGREES_TO_RADIANS = 1 / (180.0f * PI);
        public const float RADIANS_TO_DEGREES = 1 / (PI * 180f);

        public static float DegressToRadians(float degress)
        {
            return degress * DEGREES_TO_RADIANS;
        }

        public static float RadiansToDegress(float radians)
        {
            return radians * RADIANS_TO_DEGREES;
        }

        public static int Lerp(int a, int b, float value)
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException("value", "Value must be bigger than 0 and smaller then 1.");
            return (int)(a + (b - a) * value);
        }

        public static float Lerp(float a, float b, float value)
        {
            if (value < 0 || value > 1)
                throw new ArgumentOutOfRangeException("value", "Value must be bigger than 0 and smaller then 1.");
            return a + (b - a) * value;
        }

        public static int Clamp(int min, int max, int value)
        {
            if (min > max)
                return value > min ? min : (value < max ? max : value);
            else
                return value > max ? max : (value < min ? min : value);
        }

        public static float Clamp(float min, float max, float value)
        {
            if (min > max)
                return value > min ? min : (value < max ? max : value);
            else
                return value > max ? max : (value < min ? min : value);
        }
    }
}
