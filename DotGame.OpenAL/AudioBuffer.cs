using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System.Runtime.InteropServices;

namespace DotGame.OpenAL
{
    internal class AudioBuffer : AudioObject, IAudioObject, IEquatable<AudioBuffer>
    {
        internal readonly int ID;

        public AudioBuffer(AudioDevice audioDevice) : base(audioDevice)
        {
            this.ID = AL.GenBuffer();
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void SetData<T>(ALFormat format, T[] data, int frequency) where T : struct
        {
            AL.BufferData<T>(ID, format, data, Marshal.SizeOf(data[0]) * data.Length, frequency);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is AudioBuffer)
                return Equals((AudioBuffer)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(AudioBuffer other)
        {
            return ID == other.ID;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ID.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[AudioBuffer ID: ");
            builder.Append(ID);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
