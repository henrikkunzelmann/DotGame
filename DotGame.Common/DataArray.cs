using System;
using System.Runtime.InteropServices;

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
        
        public static DataArray FromArray<T>(T[] array, out GCHandle handle)
        {
            if (array != null)
            {
                handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                return new DataArray(handle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(T)) * array.Length);
            }
            else
            {
                handle = new GCHandle();
                return new DataArray(IntPtr.Zero, 0);
            }
        }
        public static DataArray FromObject<T>(T data, out GCHandle handle)
        {
            if (data != null)
            {
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                return new DataArray(handle.AddrOfPinnedObject(), Marshal.SizeOf(typeof(T)));
            }
            else
            {
                handle = new GCHandle();
                return new DataArray(IntPtr.Zero, 0);
            }
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
