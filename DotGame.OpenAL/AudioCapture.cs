using DotGame.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioCaptureInternal = OpenTK.Audio.AudioCapture;

namespace DotGame.OpenAL
{
    public class AudioCapture : AudioObject, IAudioCapture, ISampleSource
    {
        public static IList<string> AvailableDevices { get { return AudioCaptureInternal.AvailableDevices; } }
        public static string DefaultDevice { get { return AudioCaptureInternal.DefaultDevice; } }

        public long TotalSamples { get { AssertNotDisposed(); return readSamples + handle.AvailableSamples; } }
        private long readSamples = 0;

        public long Position { get { AssertNotDisposed(); return position; } set { throw new InvalidOperationException("AudioCapture does not support setting the position within the SampleSource."); } }
        private long position = 0;

        public AudioFormat NativeFormat { get; private set; }
        public int Channels { get; private set; }
        public int SampleRate { get; private set; }

        private readonly AudioCaptureInternal handle;

        internal AudioCapture(AudioDevice audioDevice, string deviceName, int sampleRate, AudioFormat bitDepth, int channels, int bufferSize) : base(audioDevice)
        {
            // TODO (Joex3): Den deviceName parameter eventuell als Suche benutzen, um trotzdem ein Device zu finden, wenn der Name nicht 100% übereinstimmt.
            if (deviceName != null && !AvailableDevices.Contains(deviceName))
                throw new InvalidOperationException(string.Format("CaptureDevice \"{0}\" does not exist.", deviceName));

            if (sampleRate <= 0 || sampleRate >= 44100)
                throw new ArgumentOutOfRangeException("sampleRate", "SampleRate must be larger than 0 and smaller or equals to 44100.");

            if (bitDepth != AudioFormat.Byte8 || bitDepth != AudioFormat.Short16)
                throw new ArgumentException("BitDepth must be either Byte8 oder Short16.", "bitDepth");

            if (channels <= 0 || channels > 2)
                throw new ArgumentOutOfRangeException("channels", "AudioCapture only supports 1 or 2 channels.");

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "BufferSize must be larger than 0.");

            var format = EnumConverter.GetFormat(bitDepth, channels);
            this.handle = new AudioCaptureInternal(deviceName, sampleRate, format, bufferSize);
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

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count must be >= 0.");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException("count", "offset + count must be <= buffer.Length.");

            count = Math.Min(count, (int)(TotalSamples - Position));

            switch (NativeFormat)
            {
                case AudioFormat.Byte8:
                    {
                        var arr = new byte[count];
                        handle.ReadSamples(arr, count);
                        Parallel.For(0, arr.Length, (i) => buffer[offset + i] = (float)(arr[i] / (float)byte.MaxValue * 2) - 1);
                        break;
                    }

                case AudioFormat.Short16:
                    {
                        var arr = new short[count];
                        handle.ReadSamples(arr, count);
                        Parallel.For(0, arr.Length, (i) => buffer[offset + i] = arr[i] / (float)short.MaxValue - 1);
                        break;
                    }
            }
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
            ReadSamples(offset, (int)TotalSamples, buffer);
        }

        protected override void Dispose(bool isDisposing)
        {
            handle.Dispose();

            base.Dispose(isDisposing);
        }
    }
}
