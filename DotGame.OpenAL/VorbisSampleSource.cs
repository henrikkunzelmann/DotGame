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
    }
}
