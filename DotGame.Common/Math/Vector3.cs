using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Ein Vektor mit 3 Komponenten.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// Die X-Komponente des Vektors.
        /// </summary>
        public float X;

        /// <summary>
        /// Die Y-Komponente des Vektors.
        /// </summary>
        public float Y;
		
        /// <summary>
        /// Die Z-Komponente des Vektors.
        /// </summary>
        public float Z;
		
        #region Konstanten
        /// <summary>
        /// Die Größte des Vector3-Structs in Bytes.
        /// </summary>
        public static int SizeInBytes { get { return sizeInBytes; } }

        /// <summary>
        /// Vector3(0, 0, 0).
        /// </summary>
        public static Vector3 Zero { get { return zero; } }

        /// <summary>
        /// Vector3(1, 0, 0).
        /// </summary>
        public static Vector3 UnitX { get { return unitX; } }

        /// <summary>
        /// Vector3(0, 1, 0).
        /// </summary>
        public static Vector3 UnitY { get { return unitY; } }
		
        /// <summary>
        /// Vector3(0, 0, 1).
        /// </summary>
        public static Vector3 UnitZ { get { return unitZ; } }
		
        /// <summary>
        /// Vector3(1, 1, 1).
        /// </summary>
        public static Vector3 One { get { return one; } }

        private static readonly int sizeInBytes = Marshal.SizeOf(typeof(Vector3));
        private static readonly Vector3 zero = new Vector3(0);
        private static readonly Vector3 unitX = new Vector3(1, 0, 0);
        private static readonly Vector3 unitY = new Vector3(0, 1, 0);
        private static readonly Vector3 unitZ = new Vector3(0, 0, 1);
        private static readonly Vector3 one = new Vector3(1);
        #endregion

        /// <summary>
        /// Erstellt einen Vektor und setzt alle Komponenten auf value.
        /// </summary>
        /// <param name="value">Der Wert für alle Komponenten.</param>
        public Vector3(float value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
        }

        /// <summary>
        /// Erstellt einen Vektor mit den angegebenen Werten.
        /// </summary>
        /// <param name="x">Die X Komponente.</param>
        /// <param name="y">Die Y Komponente.</param>
        /// <param name="z">Die Z Komponente.</param>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Gibt die Länge des Vektors zurück.
        /// </summary>
        /// <returns>Die Länge.</returns>
        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        /// <summary>
        /// Gibt die quadrierte Länge zurück.
        /// </summary>
        /// <returns>Die quadrierte Länge.</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Normalisiert den Vektor. Seine Länge beträgt danach 1.
        /// </summary>
        public void Normalize()
        {
            float length = Length();
            X /= length;
            Y /= length;
            Z /= length;
        }

        #region Operatoren
        public static bool operator ==(Vector3 value1, Vector3 value2)
        {
            return value1.X == value2.X
                && value1.Y == value2.Y
                && value1.Z == value2.Z
;
        }
        public static bool operator !=(Vector3 value1, Vector3 value2)
        {
            return !(value1 == value2);
        }
        public static Vector3 operator +(Vector3 a)
        {
            return a;
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
			return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.X, -a.Y, -a.Z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
			return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
			return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }
        public static Vector3 operator *(Vector3 a, float scalar)
        {
			return new Vector3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
			return new Vector3(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }
        public static Vector3 operator /(Vector3 a, float scalar)
        {
			return new Vector3(a.X / scalar, a.Y / scalar, a.Z / scalar);
        }
        public static Vector3 operator %(Vector3 a, Vector3 b)
        {
			return new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }
        public static Vector3 operator %(Vector3 a, float scalar)
        {
			return new Vector3(a.X % scalar, a.Y % scalar, a.Z % scalar);
        }
        #endregion

        #region Statische Methoden
        /// <summary>
        /// Gibt das Minimum zweier Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Minimum der Komponenten als Vector3.</returns>
        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gibt das Minimum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="result">Das Minimum der Komponenten als Vector3.</param>
        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Gibt das Maximum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Maximum der Komponenten als Vector3.</returns>
        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            Vector3 result;
            Max(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gibt das Maximum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="result">Das Maximum der Komponenten als Vector3.</param>
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
        }

        /// <summary>
        /// Beschränkt die Komponenten eines Vektors auf einen bestimmten Bereich.
        /// </summary>
        /// <param name="value">Der Vektor.</param>
        /// <param name="min">Die untere Grenze.</param>
        /// <param name="max">Die obere Grenze.</param>
        /// <returns>Der Vektor im Bereich von min und max.</returns>
        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            Vector3 result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Beschränkt die Komponenten eines Vektors auf einen bestimmten Bereich.
        /// </summary>
        /// <param name="value">Der Vektor.</param>
        /// <param name="min">Die untere Grenze.</param>
        /// <param name="max">Die obere Grenze.</param>
        /// <returns>Der Vektor im Bereich von min und max.</returns>
        public static void Clamp(ref Vector3 value, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            result.X = value.X > min.X ? value.X < max.X ? value.X : max.X : min.X;
            result.Y = value.Y > min.Y ? value.Y < max.Y ? value.Y : max.Y : min.Y;
            result.Z = value.Z > min.Z ? value.Z < max.Z ? value.Z : max.Z : min.Z;
        }

        /// <summary>
        /// Interpoliert linear zwischen zwei Werten.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="amt">Der Gewichtungswert (0 = value1, 1 = value2).</param>
        /// <returns>Der interpolierte Wert.</returns>
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amt)
        {
            Vector3 result;
            Lerp(ref value1, ref value2, amt, out result);
            return result;
        }

        /// <summary>
        /// Interpoliert linear zwischen zwei Werten.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="amt">Der Gewichtungswert (0 = value1, 1 = value2).</param>
        /// <returns>Der interpolierte Wert.</returns>
        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amt, out Vector3 result)
        {
            float namt = 1 - amt;
            result.X = namt * value1.X + amt * value2.X;
            result.Y = namt * value1.Y + amt * value2.Y;
            result.Z = namt * value1.Z + amt * value2.Z;
        }

        /// <summary>
        /// Gibt das Punktprodukt der angegebenen Vektoren zurück. Handelt es sich dabei um Einheitsvektoren wird der Kosinus des eingeschlossenen Winkels zurückgegeben.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Punktprodukt.</returns>
        public static float Dot(Vector3 value1, Vector3 value2)
        {
            float result;
            Dot(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gibt das Punktprodukt der angegebenen Vektoren zurück. Handelt es sich dabei um Einheitsvektoren wird der Kosinus des eingeschlossenen Winkels zurückgegeben.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Punktprodukt.</returns>
        public static void Dot(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z;
        }

		/// <summary>
        /// Gibt das Kreuzprodukt der angegebenen Vektoren zurück. Dies ist die rechtshändige Senkrechte auf der von value1 und value2 definierten Ebene.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Kreuzprodukt.</returns>
        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            Cross(ref vector1, ref vector2, out vector1);
            return vector1;
        }

		/// <summary>
        /// Gibt das Kreuzprodukt der angegebenen Vektoren zurück. Dies ist die rechtshändige Senkrechte auf der von value1 und value2 definierten Ebene.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Kreuzprodukt.</returns>
        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            var x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
            var y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
            var z = vector1.X * vector2.Y - vector2.X * vector1.Y;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }

        /// <summary>
        /// Normalisiert den angegebenen Vektor. Seine Länge beträgt danach 1.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Der normalisierte Vektor.</returns>
        public static Vector3 Normalize(Vector3 value)
        {
            return value / value.Length();
        }

        /// <summary>
        /// Normalisiert den angegebenen Vektor. Seine Länge beträgt danach 1.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Der normalisierte Vektor.</returns>
        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            result = value / value.Length();
        }

        // TODO: Transform + noch mehr Methoden.
        #endregion

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vector3)
                return Equals((Vector3)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Vector3 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                hash = hash * 23 + Z.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[X: ");
            builder.Append(X);
            builder.Append(", Y: ");
            builder.Append(Y);
            builder.Append(", Z: ");
            builder.Append(Z);
            builder.Append("]");

            return builder.ToString();
        }
    }
}