using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    /// <summary>
    /// Stellt ein Rechteck an Daten dar.
    /// </summary>
    public struct DataRectangle : IEquatable<DataRectangle>
    {
        /// <summary>
        /// Gibt den Pointer zu den Daten zurück.
        /// </summary>
        public IntPtr Pointer { get; private set; }

        /// <summary>
        /// Gibt die Anzahl an Bytes per Zeile zurück.
        /// </summary>
        public int Pitch { get; private set; }

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

        public DataRectangle(IntPtr pointer, int pitch, int size)
            : this()
        {
            if (pitch < 0)
                throw new ArgumentOutOfRangeException("pitch");
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            this.Pointer = pointer;
            this.Pitch = pitch;
            this.Size = size;
        }
        
        public static DataRectangle FromArray<T>(T[] array, int pitch, out GCHandle handle)
        {
            if (array != null)
            {
                handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                return new DataRectangle(handle.AddrOfPinnedObject(), pitch, Marshal.SizeOf(typeof(T)) * array.Length);
            }
            else
            {
                handle = new GCHandle();
                return new DataRectangle(IntPtr.Zero, 0, 0);
            }
        }

        public static bool operator ==(DataRectangle a, DataRectangle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DataRectangle a, DataRectangle b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is DataRectangle)
                return Equals((DataRectangle)obj);
            return base.Equals(obj);
        }

        public bool Equals(DataRectangle other)
        {
            return other.Pointer == Pointer && other.Pitch == Pitch && other.Size == Size;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Pointer.GetHashCode();
                hash = hash * 23 + Pitch.GetHashCode();
                hash = hash * 23 + Size.GetHashCode();
                return hash;
            }
        }
    }
}
