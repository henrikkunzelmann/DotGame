using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using NVorbis;

namespace DotGame.OpenAL
{
    public class VorbisSampleSource : AudioObject, ISampleSource
    {
        public long TotalSamples { get; private set; }
        public long Position { get { AssertNotDisposed(); return reader.DecodedPosition; } set { AssertNotDisposed(); reader.DecodedPosition = value; } }

        public AudioFormat NativeFormat { get; private set; }
        public int Channels { get; private set; }
        public int SampleRate { get; private set; }

        private VorbisReader reader;

        public VorbisSampleSource(AudioDevice audioDevice, string file) : base(audioDevice)
        {
            reader = new VorbisReader(file);

            this.TotalSamples = reader.TotalSamples;
            this.Position = reader.DecodedPosition;
            this.NativeFormat = AudioFormat.Float32;
            this.Channels = reader.Channels;
            this.SampleRate = reader.SampleRate;
        }

        public float[] ReadSamples(int count)
        {
            AssertNotDisposed();
            float[] samples = new float[count];
            ReadSamples(samples, 0, count);
            return samples;
        }

        public void ReadSamples(float[] buffer, int offset, int count)
        {
            AssertNotDisposed();
            reader.ReadSamples(buffer, offset, count);
        }

        public float[] ReadAll()
        {
            AssertNotDisposed();
            float[] samples = new float[TotalSamples * Channels];
            ReadAll(samples, 0);
            return samples;
        }

        public void ReadAll(float[] buffer, int offset)
        {
            AssertNotDisposed();
            reader.ReadSamples(buffer, offset, (int)TotalSamples * Channels);
        }

        protected override void Dispose(bool isDisposing)
        {
            reader.Dispose();

            base.Dispose(isDisposing);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is VorbisSampleSource)
                return Equals((VorbisSampleSource)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(VorbisSampleSource other)
        {
            return reader == other.reader;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + reader.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[SampleSource Type: ogg/vorbis, NativeFormat:");
            builder.Append(NativeFormat);
            builder.Append(", Channels: ");
            builder.Append(Channels);
            builder.Append(", SampleRate: ");
            builder.Append(SampleRate);
            builder.Append(", TotalSamples: ");
            builder.Append(TotalSamples);
            builder.Append(", Position: ");
            builder.Append(Position);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
