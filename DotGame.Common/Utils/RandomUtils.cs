using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

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

        /// <summary>
        /// Gibt einen Vektor mit Zufallswerten zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <returns>Der Vektor.</returns>
        public static Vector2 NextVector2(this Random rand)
        {
            return new Vector2(rand.NextFloat(), rand.NextFloat());
        }
        
        /// <summary>
        /// Gibt einen Vektor mit Zufallswerten zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <returns>Der Vektor.</returns>
        public static Vector3 NextVector3(this Random rand)
        {
            return new Vector3(rand.NextFloat(), rand.NextFloat(), rand.NextFloat());
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
        /// Gibt eine zufällige Farbe zurück mit dem angegebenen Alphawert.
        /// </summary>
        /// <param name="AlphaValue">Der zu nutzende Alphawert.</param>
        /// <returns>Die zufällige Farbe.</returns>
        public static Color NextColorRGB(this Random rand, float AlphaValue)
        {
            return Color.FromArgb(AlphaValue, rand.NextFloat(), rand.NextFloat(), rand.NextFloat());
        }

        /// <summary>
        /// Gibt eine zufällige Farbe mit einem zufälligen Alphawert zurück.
        /// </summary>
        /// <returns>Die zufällige Farbe.</returns>
        public static Color NextColorARGB(this Random random)
        {
            return random.NextColorRGB(random.NextFloat());
        }
    }
}
