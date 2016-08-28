using System;
using System.Runtime.InteropServices;

namespace DotGame
{
    /// <summary>
    /// Stellt eine Box an Daten dar.
    /// </summary>
    public struct DataBox : IEquatable<DataBox>
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
        /// Gibt die Anzahl an Bytes pro Schicht zurück.
        /// </summary>
        public int Slice { get; private set; }

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

        public DataBox(IntPtr pointer, int pitch, int slice, int size)
            : this()
        {
            if (pitch < 0)
                throw new ArgumentOutOfRangeException("pitch");
            if (slice < 0)
                throw new ArgumentOutOfRangeException("slice");
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            this.Pointer = pointer;
            this.Pitch = pitch;
            this.Slice = slice;
            this.Size = size;
        }

        public static DataBox FromArray<T>(T[] array, int pitch, int slice, out GCHandle handle)
        {
            if (array != null)
            {
                handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                return new DataBox(handle.AddrOfPinnedObject(), pitch, slice, Marshal.SizeOf(typeof(T)) * array.Length);
            }
            else
            {
                handle = new GCHandle();
                return new DataBox(IntPtr.Zero, 0, 0, 0);
            }
        }

        public static bool operator ==(DataBox a, DataBox b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DataBox a, DataBox b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is DataBox)
                return Equals((DataBox)obj);
            return base.Equals(obj);
        }

        public bool Equals(DataBox other)
        {
            return other.Pointer == Pointer && other.Pitch == Pitch && other.Slice == Slice && other.Size == Size;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Pointer.GetHashCode();
                hash = hash * 23 + Pitch.GetHashCode();
                hash = hash * 23 + Slice.GetHashCode();
                hash = hash * 23 + Size.GetHashCode();
                return hash;
            }
        }
    }
}
