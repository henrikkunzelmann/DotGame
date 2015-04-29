using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Stellt ein Array an Daten dar.
    /// </summary>
    public struct DataArray : IEquatable<DataArray>
    {
        /// <summary>
        /// Gibt den Pointer zu den Daten zurück.
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// Gibt die Größe der Daten in Bytes zurück.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gibt zurück ob der Datenpointer ein NULL-Pointer ist.
        /// </summary>
        public bool IsNull
        {
            get { return Pointer == IntPtr.Zero; }
        }

        public DataArray(IntPtr pointer, int size)
            : this()
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            this.Pointer = pointer;
            this.Size = size;
        }

        public static bool operator ==(DataArray a, DataArray b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DataArray a, DataArray b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is DataArray)
                return Equals((DataArray)obj);
            return base.Equals(obj);
        }

        public bool Equals(DataArray other)
        {
            return other.Pointer == Pointer && other.Size == Size;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Pointer.GetHashCode();
                hash = hash * 23 + Size.GetHashCode();
                return hash;
            }
        }
    }
}
