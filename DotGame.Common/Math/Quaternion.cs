using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Ein Quaternion mit vier Komponenten.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        #region Arithmetic
        public static Quaternion Add(Quaternion quaternion1, Quaternion quaternion2)
		{
            Quaternion result;
            Add(ref quaternion1, ref quaternion2, out result);
            return result;
		}
		
		public static void Add(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
            result = new Quaternion(quaternion1.X + quaternion2.X, 
                                    quaternion1.Y + quaternion2.Y, 
                                    quaternion1.Z + quaternion2.Z, 
                                    quaternion1.W + quaternion2.W);
		}
		
		public static Quaternion Subtract(Quaternion quaternion1, Quaternion quaternion2)
		{
            Quaternion result;
            Subtract(ref quaternion1, ref quaternion2, out result);
            return result;
		}
		
		public static void Subtract(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
            result = new Quaternion(quaternion1.X - quaternion2.X, 
                                    quaternion1.Y - quaternion2.Y, 
                                    quaternion1.Z - quaternion2.Z, 
                                    quaternion1.W - quaternion2.W);
		}
		
		public static Quaternion Multiply(Quaternion quaternion1, Quaternion quaternion2)
		{
            Quaternion result;
            Multiply(ref quaternion1, ref quaternion2, out result);
            return result;
		}
		
		public static void Multiply(ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			result = new Quaternion(quaternion1.W * quaternion2.X + quaternion1.X * quaternion2.W + quaternion1.Y * quaternion2.Z - quaternion1.Z * quaternion2.Y,
		                            quaternion1.W * quaternion2.Y - quaternion1.X * quaternion2.Z + quaternion1.Y * quaternion2.W + quaternion1.Z * quaternion2.X,
		                            quaternion1.W * quaternion2.Z + quaternion1.X * quaternion2.Y - quaternion1.Y * quaternion2.X + quaternion1.Z * quaternion2.W,
		                            quaternion1.W * quaternion2.W - quaternion1.X * quaternion2.X - quaternion1.Y * quaternion2.Y - quaternion1.Z * quaternion2.Z);
		}
		
		public static Quaternion Multiply (Quaternion quaternion, float scale)
		{
            Quaternion result;
            Multiply(ref quaternion, scale, out result);
            return result;
		}

        public static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result)
		{
            result = new Quaternion(quaternion.X * scale,
                                    quaternion.Y * scale,
                                    quaternion.Z * scale,
                                    quaternion.W * scale);
		}
		
		public static Quaternion Divide (Quaternion quaternion1, Quaternion quaternion2)
		{
			Quaternion result;
			Divide (ref quaternion1, ref quaternion2, out result);
			return result;
		}
		
		public static void Divide (ref Quaternion quaternion1, ref Quaternion quaternion2, out Quaternion result)
		{
			Quaternion inv;
			Inverse (ref quaternion2, out inv);
			Multiply (ref quaternion1, ref inv, out result);
		}
		
		public static Quaternion Divide (Quaternion quaternion, float scale)
		{
            Quaternion result;
            Divide(ref quaternion, scale, out result);
            return result;
		}
		
		public static void Divide (ref Quaternion quaternion, float scale, out Quaternion result)
		{
            result = new Quaternion(quaternion.X / scale, quaternion.Y / scale, quaternion.Z / scale, quaternion.W / scale);
		}
		
		public static Quaternion Negate (Quaternion quaternion)
		{
            Quaternion result;
            Negate(ref quaternion, out result);
            return result;
		}
		
		public static void Negate (ref Quaternion quaternion, out Quaternion result)
		{
            result = new Quaternion(-quaternion.X, -quaternion.Y, -quaternion.Z, -quaternion.W);
		}
		
		#endregion

        #region Operator overloads
        public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
        {
            return Add(quaternion1, quaternion2);
        }

        public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion result;
            Divide(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        public static Quaternion operator /(Quaternion quaternion, float scaleFactor)
        {
            return Divide(quaternion, scaleFactor);
        }

        public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
        {
            // TODO: SIMD optimization
            return new Quaternion(
                quaternion1.W * quaternion2.X + quaternion1.X * quaternion2.W + quaternion1.Y * quaternion2.Z - quaternion1.Z * quaternion2.Y,
                quaternion1.W * quaternion2.Y - quaternion1.X * quaternion2.Z + quaternion1.Y * quaternion2.W + quaternion1.Z * quaternion2.X,
                quaternion1.W * quaternion2.Z + quaternion1.X * quaternion2.Y - quaternion1.Y * quaternion2.X + quaternion1.Z * quaternion2.W,
                quaternion1.W * quaternion2.W - quaternion1.X * quaternion2.X - quaternion1.Y * quaternion2.Y - quaternion1.Z * quaternion2.Z);
        }

        public static Quaternion operator *(Quaternion quaternion, float scaleFactor)
        {
            return Multiply(quaternion, scaleFactor);
        }

        public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
        {
            return Subtract(quaternion1, quaternion2);
        }

        public static Quaternion operator -(Quaternion quaternion)
        {
            return Negate(quaternion);
        }

        #endregion

        #region Math
        public static Quaternion Concatenate(Quaternion value1, Quaternion value2)
        {
            Quaternion result;
            Concatenate(ref value1, ref value2, out result);
            return result;
        }

        public static void Concatenate(ref Quaternion value1, ref Quaternion value2, out Quaternion result)
        {
            Multiply(ref value1, ref value2, out result);
        }

        public void Conjugate()
        {
            Conjugate(ref this, out this);
        }

        public static Quaternion Conjugate(Quaternion value)
        {
            Conjugate(ref value, out value);
            return value;
        }

        public static void Conjugate(ref Quaternion value, out Quaternion result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = value.W;
        }

        public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
        {
            float result;
            Dot(ref quaternion1, ref quaternion2, out result);
            return result;
        }

        public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out float result)
        {
            result = (quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y) +
                     (quaternion1.Z * quaternion2.Z) + (quaternion1.W * quaternion2.W);
        }

        public static Quaternion Inverse(Quaternion quaternion)
        {
            Inverse(ref quaternion, out quaternion);
            return quaternion;
        }

        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            // http://www.ncsa.illinois.edu/~kindr/emtc/quaternions/quaternion.c++
            Quaternion conj = new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
            conj.Conjugate();

            result = conj * (1.0f / quaternion.LengthSquared());
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        public static Quaternion Lerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
        {
            Quaternion result;
            Lerp(ref quaternion1, ref quaternion2, amount, out result);
            return result;
        }

        public static void Lerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
        {
            Quaternion q1;
            Multiply(ref quaternion1, 1.0f - amount, out q1);

            Quaternion q2;
            Multiply(ref quaternion2, amount, out q2);

            Quaternion q1q2;
            Add(ref q1, ref q2, out q1q2);
            Normalize(ref q1q2, out result);
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static Quaternion Normalize(Quaternion quaternion)
        {
            Normalize(ref quaternion, out quaternion);
            return quaternion;
        }

        public static void Normalize(ref Quaternion quaternion, out Quaternion result)
        {
            // TODO: SIMD optimization
            Multiply(ref quaternion, 1.0f / quaternion.Length(), out result);
        }

        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
        {
            Quaternion result;
            Slerp(ref quaternion1, ref quaternion2, amount, out result);
            return result;
        }

        public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
        {
            float dot;
            Dot(ref quaternion1, ref quaternion2, out dot);

            Quaternion q3;

            if (dot < 0.0f)
            {
                dot = -dot;
                Negate(ref quaternion2, out q3);
            }
            else
            {
                q3 = quaternion2;
            }

            if (dot < 0.999999f)
            {
                float angle = (float)System.Math.Acos(dot);
                float sin1 = (float)System.Math.Sin(angle * (1.0f - amount));
                float sin2 = (float)System.Math.Sin(angle * amount);
                float sin3 = (float)System.Math.Sin(angle);

                Quaternion q1;
                Multiply(ref quaternion1, sin1, out q1);

                Quaternion q2;
                Multiply(ref q3, sin2, out q2);

                Quaternion q4;
                Add(ref q1, ref q2, out q4);

                Divide(ref q4, sin3, out result);
            }
            else
            {
                Lerp(ref quaternion1, ref q3, amount, out result);
            }
        }

        #endregion

        public bool Equals(Quaternion other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            return obj is Quaternion && ((Quaternion)obj) == this;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }

        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W);
        }

        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }

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
