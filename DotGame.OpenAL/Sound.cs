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
        public readonly bool ForceMono;

        internal readonly AudioBuffer Buffer;

        public Sound(AudioDevice audioDevice, string file, bool forceMono) : base(audioDevice)
        {
            var source = audioDevice.Factory.CreateSampleSource(file);
            var channels = forceMono ? 1 : source.Channels;
            var sampleRate = source.SampleRate;
            var samples = source.ReadAll();
            if (forceMono)
            {
                float[] newSamples = new float[(int)(samples.Length / (float)source.Channels)];
                Parallel.For(0, newSamples.Length, (i) => {
                    float val = 0;
                    for (int j = 0; j < source.Channels; j++)
                        val += samples[i * source.Channels + j];
                    newSamples[i] = val / source.Channels;
                });
                samples = newSamples;
            }

            Buffer = new AudioBuffer(audioDevice);
            Buffer.SetData(forceMono ? ALFormat.MonoFloat32Ext : ALFormat.StereoFloat32Ext, samples, sampleRate);
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
