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
                var format = OpenAL.AudioDevice.GetFormat(AudioFormat.Short16, 1);
                short[] samplesChannel = new short[samples.Length / channels];
                for (int i = 0; i < channels; i++)
                {
                    for (int j = 0; j < samplesChannel.Length; j++)
                    {
                        samplesChannel[j] = (short)(samples[i + j * channels] * short.MaxValue);
                    }

                    var buffer = new AudioBuffer(audioDevice);
                    buffer.SetData(format, samplesChannel, sampleRate);
                    buffers.Add(buffer);
                }
            }
            else
            {
                short[] samplesChannel = new short[samples.Length];
                for (int i = 0; i < samplesChannel.Length; i++)
                {
                    samplesChannel[i] = (short)(samples[i] * short.MaxValue);
                }

                var format = OpenAL.AudioDevice.GetFormat(AudioFormat.Short16, source.Channels);
                var buffer = new AudioBuffer(audioDevice);
                buffer.SetData(format, samplesChannel, sampleRate);
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is ISound)
                return Equals((ISound)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(ISound other)
        {
            if (other is Sound)
                return Supports3D == other.Supports3D && buffers.SequenceEqual(((Sound)other).buffers);
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Supports3D.GetHashCode();
                for (int i = 0; i < buffers.Count; i++)
                    hash = hash * 23 + buffers[i].GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Sound Supports3D: ");
            builder.Append(Supports3D);
            builder.Append(", Buffers: ");
            builder.Append(Buffers.Count);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
