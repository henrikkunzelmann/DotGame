using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using NVorbis;

namespace DotGame.Audio.SampleSources
{
    public class VorbisSampleSource : SampleSourceBase, ISampleSource
    {
        public long TotalSamples { get; private set; }
        public long Position { get { AssertNotDisposed(); return reader.DecodedPosition * Channels; } set { AssertNotDisposed(); reader.DecodedPosition = value / Channels; } }

        public AudioFormat NativeFormat { get; private set; }
        public int Channels { get; private set; }
        public int SampleRate { get; private set; }

        private VorbisReader reader;

        public VorbisSampleSource(string file)
        {
            reader = new VorbisReader(file);
            
            this.Channels = reader.Channels;
            this.TotalSamples = reader.TotalSamples * Channels;
            this.NativeFormat = AudioFormat.Float32;
            this.SampleRate = reader.SampleRate;
        }

        public float[] ReadSamples(int count)
        {
            AssertNotDisposed();
            count = Math.Min(count, (int)(TotalSamples - Position));
            float[] samples = new float[count];
            ReadSamples(0, count, samples);
            return samples;
        }

        public void ReadSamples(int offset, int count, float[] buffer)
        {
            AssertNotDisposed();
            reader.ReadSamples(buffer, offset, count);
        }

        public float[] ReadAll()
        {
            AssertNotDisposed();
            float[] samples = new float[(int)(TotalSamples - Position)];
            ReadAll(0, samples);
            return samples;
        }

        public void ReadAll(int offset, float[] buffer)
        {
            AssertNotDisposed();
            reader.ReadSamples(buffer, offset, (int)TotalSamples);
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
