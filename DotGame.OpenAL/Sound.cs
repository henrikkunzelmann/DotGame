using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class Sound : AudioObject, ISound
    {
        public bool Supports3D { get; private set; }

        internal ReadOnlyCollection<AudioBuffer> Buffers { get { return buffers.AsReadOnly(); } }

        private readonly List<AudioBuffer> buffers;

        public Sound(AudioDevice audioDevice, string file, bool supports3D) : base(audioDevice)
        {
            var source = audioDevice.Factory.CreateSampleSource(file);
            var channels = source.Channels;
            var sampleRate = source.SampleRate;
            var samples = source.ReadAll();

            buffers = new List<AudioBuffer>();
            if (supports3D)
            {
                var format = OpenAL.AudioDevice.GetFormat(source.NativeFormat, 1);
                float[] samplesChannel = new float[samples.Length / channels];
                for (int i = 0; i < channels; i++)
                {
                    for (int j = 0; j < samplesChannel.Length; j++)
                    {
                        samplesChannel[j] = samples[i + j * channels];
                    }

                    var buffer = new AudioBuffer(audioDevice);
                    buffer.SetData(format, samplesChannel, sampleRate);
                    buffers.Add(buffer);
                }
            }
            else
            {
                var format = OpenAL.AudioDevice.GetFormat(source.NativeFormat, source.Channels);
                var buffer = new AudioBuffer(audioDevice);
                buffer.SetData(format, samples, sampleRate);
                buffers.Add(buffer);
            }

            this.Supports3D = supports3D;
        }

        public ISoundInstance CreateInstance(bool isPaused)
        {
            return new SoundInstance(this, isPaused);
        }

        protected override void Dispose(bool isDisposing)
        {
            foreach (var buffer in buffers)
            {
                buffer.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
