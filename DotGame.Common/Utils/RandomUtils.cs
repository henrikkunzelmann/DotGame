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
        /// Gibt eine Farbe mit Zufallswerten zwischen 0.0f und 1.0f zurück.
        /// </summary>
        /// <param name="alpha1">Gibt an, ob der Alphaanteil mit 1.0f initialisiert werden soll.</param>
        /// <returns>Die Farbe.</returns>
        public static Color NextColor(this Random rand, bool alpha1 = true)
        {
            return new Color(rand.NextFloat(), rand.NextFloat(), rand.NextFloat(), alpha1 ? 1.0f : rand.NextFloat());
        }
    }
}
