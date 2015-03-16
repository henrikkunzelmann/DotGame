using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Ein Vektor mit 4 Komponenten.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4 : IEquatable<Vector4>
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
		
        /// <summary>
        /// Die W-Komponente des Vektors.
        /// </summary>
        public float W;
		
        #region Konstanten
        /// <summary>
        /// Die Größte des Vector4-Structs in Bytes.
        /// </summary>
        public static int SizeInBytes { get { return sizeInBytes; } }

        /// <summary>
        /// Vector4(0, 0, 0, 0).
        /// </summary>
        public static Vector4 Zero { get { return zero; } }

        /// <summary>
        /// Vector4(1, 0, 0, 0).
        /// </summary>
        public static Vector4 UnitX { get { return unitX; } }

        /// <summary>
        /// Vector4(0, 1, 0, 0).
        /// </summary>
        public static Vector4 UnitY { get { return unitY; } }
		
        /// <summary>
        /// Vector4(0, 0, 1, 0).
        /// </summary>
        public static Vector4 UnitZ { get { return unitZ; } }
		
        /// <summary>
        /// Vector4(0, 0, 0, 1).
        /// </summary>
        public static Vector4 UnitW { get { return unitW; } }
		
        /// <summary>
        /// Vector4(1, 1, 1, 1).
        /// </summary>
        public static Vector4 One { get { return one; } }

        private static readonly int sizeInBytes = Marshal.SizeOf(typeof(Vector4));
        private static readonly Vector4 zero = new Vector4(0);
        private static readonly Vector4 unitX = new Vector4(1, 0, 0, 0);
        private static readonly Vector4 unitY = new Vector4(0, 1, 0, 0);
        private static readonly Vector4 unitZ = new Vector4(0, 0, 1, 0);
        private static readonly Vector4 unitW = new Vector4(0, 0, 0, 1);
        private static readonly Vector4 one = new Vector4(1);
        #endregion

        /// <summary>
        /// Erstellt einen Vektor und setzt alle Komponenten auf value.
        /// </summary>
        /// <param name="value">Der Wert für alle Komponenten.</param>
        public Vector4(float value)
        {
            this.X = value;
            this.Y = value;
            this.Z = value;
            this.W = value;
        }

        /// <summary>
        /// Erstellt einen Vektor mit den angegebenen Werten.
        /// </summary>
        /// <param name="x">Die X Komponente.</param>
        /// <param name="y">Die Y Komponente.</param>
        /// <param name="z">Die Z Komponente.</param>
        /// <param name="w">Die W Komponente.</param>
        public Vector4(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
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
            return X * X + Y * Y + Z * Z + W * W;
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
            W /= length;
        }

        #region Operatoren
        public static bool operator ==(Vector4 value1, Vector4 value2)
        {
            return value1.X == value2.X
                && value1.Y == value2.Y
                && value1.Z == value2.Z
                && value1.W == value2.W
;
        }
        public static bool operator !=(Vector4 value1, Vector4 value2)
        {
            return !(value1 == value2);
        }
        public static Vector4 operator +(Vector4 a)
        {
            return a;
        }
        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
			return new Vector4(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        }
        public static Vector4 operator -(Vector4 a)
        {
            return new Vector4(-a.X, -a.Y, -a.Z, -a.W);
        }
        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
			return new Vector4(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        }
        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
			return new Vector4(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
        }
        public static Vector4 operator *(Vector4 a, float scalar)
        {
			return new Vector4(a.X * scalar, a.Y * scalar, a.Z * scalar, a.W * scalar);
        }
        public static Vector4 operator /(Vector4 a, Vector4 b)
        {
			return new Vector4(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
        }
        public static Vector4 operator /(Vector4 a, float scalar)
        {
			return new Vector4(a.X / scalar, a.Y / scalar, a.Z / scalar, a.W / scalar);
        }
        public static Vector4 operator %(Vector4 a, Vector4 b)
        {
			return new Vector4(a.X % b.X, a.Y % b.Y, a.Z % b.Z, a.W % b.W);
        }
        public static Vector4 operator %(Vector4 a, float scalar)
        {
			return new Vector4(a.X % scalar, a.Y % scalar, a.Z % scalar, a.W % scalar);
        }
        #endregion

        #region Statische Methoden
        /// <summary>
        /// Gibt das Minimum zweier Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Minimum der Komponenten als Vector4.</returns>
        public static Vector4 Min(Vector4 value1, Vector4 value2)
        {
            Vector4 result;
            Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gibt das Minimum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="result">Das Minimum der Komponenten als Vector4.</param>
        public static void Min(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z < value2.Z ? value1.Z : value2.Z;
            result.W = value1.W < value2.W ? value1.W : value2.W;
        }

        /// <summary>
        /// Gibt das Maximum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Maximum der Komponenten als Vector4.</returns>
        public static Vector4 Max(Vector4 value1, Vector4 value2)
        {
            Vector4 result;
            Max(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gibt das Maximum zweiter Vektoren zurück.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="result">Das Maximum der Komponenten als Vector4.</param>
        public static void Max(ref Vector4 value1, ref Vector4 value2, out Vector4 result)
        {
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
            result.Z = value1.Z > value2.Z ? value1.Z : value2.Z;
            result.W = value1.W > value2.W ? value1.W : value2.W;
        }

        /// <summary>
        /// Beschränkt die Komponenten eines Vektors auf einen bestimmten Bereich.
        /// </summary>
        /// <param name="value">Der Vektor.</param>
        /// <param name="min">Die untere Grenze.</param>
        /// <param name="max">Die obere Grenze.</param>
        /// <returns>Der Vektor im Bereich von min und max.</returns>
        public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max)
        {
            Vector4 result;
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
        public static void Clamp(ref Vector4 value, ref Vector4 min, ref Vector4 max, out Vector4 result)
        {
            result.X = value.X > min.X ? value.X < max.X ? value.X : max.X : min.X;
            result.Y = value.Y > min.Y ? value.Y < max.Y ? value.Y : max.Y : min.Y;
            result.Z = value.Z > min.Z ? value.Z < max.Z ? value.Z : max.Z : min.Z;
            result.W = value.W > min.W ? value.W < max.W ? value.W : max.W : min.W;
        }

        /// <summary>
        /// Interpoliert linear zwischen zwei Werten.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <param name="amt">Der Gewichtungswert (0 = value1, 1 = value2).</param>
        /// <returns>Der interpolierte Wert.</returns>
        public static Vector4 Lerp(Vector4 value1, Vector4 value2, float amt)
        {
            Vector4 result;
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
        public static void Lerp(ref Vector4 value1, ref Vector4 value2, float amt, out Vector4 result)
        {
            float namt = 1 - amt;
            result.X = namt * value1.X + amt * value2.X;
            result.Y = namt * value1.Y + amt * value2.Y;
            result.Z = namt * value1.Z + amt * value2.Z;
            result.W = namt * value1.W + amt * value2.W;
        }

        /// <summary>
        /// Gibt das Punktprodukt der angegebenen Vektoren zurück. Handelt es sich dabei um Einheitsvektoren wird der Kosinus des eingeschlossenen Winkels zurückgegeben.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Das Punktprodukt.</returns>
        public static float Dot(Vector4 value1, Vector4 value2)
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
        public static void Dot(ref Vector4 value1, ref Vector4 value2, out float result)
        {
            result = value1.X * value2.X + value1.Y * value2.Y + value1.Z * value2.Z + value1.W * value2.W;
        }

        /// <summary>
        /// Normalisiert den angegebenen Vektor. Seine Länge beträgt danach 1.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Der normalisierte Vektor.</returns>
        public static Vector4 Normalize(Vector4 value)
        {
            return value / value.Length();
        }

        /// <summary>
        /// Normalisiert den angegebenen Vektor. Seine Länge beträgt danach 1.
        /// </summary>
        /// <param name="value1">Der erste Vektor.</param>
        /// <param name="value2">Der zweite Vektor.</param>
        /// <returns>Der normalisierte Vektor.</returns>
        public static void Normalize(ref Vector4 value, out Vector4 result)
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
            if (obj is Vector4)
                return Equals((Vector4)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(Vector4 other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
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
                hash = hash * 23 + W.GetHashCode();
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
            builder.Append(", W: ");
            builder.Append(W);
            builder.Append("]");

            return builder.ToString();
        }
    }
}