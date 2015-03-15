using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Math
{
    /// <summary>
    /// Ein Vektor mit zwei Komponenten
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Die X-Komponente
        /// </summary>
        public float X;

        /// <summary>
        /// Die Y-Komponente
        /// </summary>
        public float Y;

        /// <summary>
        /// Erstellt einen Vektor und setzt die X und die Y Komponente auf den Wert value
        /// </summary>
        /// <param name="value">den Wert für X und Y</param>
        public Vector2(float value)
        {
            this.X = value;
            this.Y = value;
        }

        /// <summary>
        /// Erstellt einen Vektor und setzt die X und die Y Komponente
        /// </summary>
        /// <param name="X">die X Komponente</param>
        /// <param name="Y">die Y Komponente</param>
        public Vector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vector2)
                return Equals((Vector2)obj);
            return false;
        }

        public bool Equals(Vector2 vec)
        {
            return X == vec.X && Y == vec.Y;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return string.Format("[X: {0}, Y: {1}]", X, Y);
        }
    }
}
