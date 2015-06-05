using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using System.Numerics;

namespace DotGame.Utils
{
    public static class RandomUtils
    {
        /// <summary>
        /// Gibt eine Zufallszahl zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <returns>Die Zahl.</returns>
        public static float NextFloat(this Random rand)
        {
            return (float)rand.NextDouble();
        }

        public static float NextFloat(this Random rand, float min, float max)
        {
            return MathHelper.Lerp(min, max, rand.NextFloat());
        }

        /// <summary>
        /// Gibt einen Vektor mit Zufallswerten zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <returns>Der Vektor.</returns>
        public static Vector2 NextVector2(this Random rand)
        {
            return new Vector2(rand.NextFloat(), rand.NextFloat());
        }

        public static Vector2 NextVector2(this Random rand, Vector2 min, Vector2 max)
        {
            return new Vector2(rand.NextFloat(min.X, max.X), rand.NextFloat(min.Y, max.Y));
        }
        
        /// <summary>
        /// Gibt einen Vektor mit Zufallswerten zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <returns>Der Vektor.</returns>
        public static Vector3 NextVector3(this Random rand)
        {
            return new Vector3(rand.NextFloat(), rand.NextFloat(), rand.NextFloat());
        }

        public static Vector3 NextVector3(this Random rand, Vector3 min, Vector3 max)
        {
            return new Vector3(rand.NextFloat(min.X, max.X), rand.NextFloat(min.Y, max.Y), rand.NextFloat(min.Z, max.Z));
        }

        /// <summary>
        /// Gibt eine zufällige Farbe zurück mit dem Alpha-Wert 1.
        /// </summary>
        /// <returns>Die zufällige Farbe.</returns>
        public static Color NextColorRGB(this Random rand)
        {
             return rand.NextColorRGB(1.0f);
        }

        /// <summary>
        /// Gibt eine zufällige Farbe zurück mit dem angegebenen Alpha-Wert.
        /// </summary>
        /// <param name="alphaValue">Der zu nutzende Alphawert.</param>
        /// <returns>Die zufällige Farbe.</returns>
        public static Color NextColorRGB(this Random rand, float alphaValue)
        {
            return Color.FromArgb(alphaValue, rand.NextFloat(), rand.NextFloat(), rand.NextFloat());
        }

        /// <summary>
        /// Gibt eine zufällige Farbe mit einem zufälligen Alpha-Wert zurück.
        /// </summary>
        /// <returns>Die zufällige Farbe.</returns>
        public static Color NextColorARGB(this Random random)
        {
            return random.NextColorRGB(random.NextFloat());
        }
    }
}
