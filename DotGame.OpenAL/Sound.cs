using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class Sound : AudioObject, ISound
    {
        internal readonly AudioBuffer Buffer;

        public Sound(AudioDevice audioDevice, string file) : base(audioDevice)
        {
            var source = audioDevice.Factory.CreateSampleSource(file);
            var channels = source.Channels;
            var sampleRate = source.SampleRate;
            var samples = source.ReadAll();

            Buffer = new AudioBuffer(audioDevice);
            Buffer.SetData(ALFormat.StereoFloat32Ext, samples, sampleRate);
        }

        public ISoundInstance CreateInstance(bool isPaused)
        {
            return new SoundInstance(this, isPaused);
        }

        protected override void Dispose(bool isDisposing)
        {
            Buffer.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
