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
    internal class AudioBuffer : AudioObject, IAudioObject
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
    }
}
