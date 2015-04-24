using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class SoundInstance : AudioObject, ISoundInstance
    {
        public const int RINGBUFFER_COUNT = 6;
        public const int RINGBUFFER_SAMPLES = 44100 * 3; // 4sec
        public const int PEAK_FRAME_SIZE = 1000;

        public ISound Sound { get; private set; }

        public float Peak { get { return GetPeak(); } }

        public Vector3 Position { get { Assert3D(); return Get(ALSource3f.Position); } set { Assert3D(); Set(ALSource3f.Position, value); } }
        public Vector3 Velocity { get { Assert3D(); return Get(ALSource3f.Velocity); } set { Assert3D(); Set(ALSource3f.Velocity, value); } }
        public float Gain { get { return Get(ALSourcef.Gain); } set { Set(ALSourcef.Gain, value); } }
        public float Pitch { get { return Get(ALSourcef.Pitch); } set { Set(ALSourcef.Pitch, value); } }
        public bool IsLooping { get { return isLooping; } set { lock (locker) { if (!Sound.IsStreamed) Set(ALSourceb.Looping, value); isLooping = value; } } }
        private bool isLooping = false;

        public int StreamBufferCount { get { AssertStreamed(); return Get(ALGetSourcei.BuffersQueued); } }
        public int StreamBuffersProcessed { get { AssertStreamed(); return Get(ALGetSourcei.BuffersProcessed); } }

        internal readonly List<int> IDs;

        private readonly ISampleSource source;
        private readonly AudioBuffer<short>[,] ringbuffers;
        private int ringBufferIndex;
        private bool manualPause = false;

        private readonly int directFilter;

        private readonly object locker = new object();

        internal SoundInstance(AudioDevice audioDevice, Sound sound)  : base(audioDevice)
        {
            if (sound == null)
                throw new ArgumentNullException("sound");
            if (sound.IsDisposed)
                throw new ArgumentException("Sound is disposed.", "sound");

            this.Sound = sound;

            IDs = new List<int>();
            if (sound.IsStreamed)
                source = SampleSourceFactory.FromFile(sound.File);
            int bufferCount;
            if (sound.IsStreamed)
                bufferCount = sound.Supports3D ? source.Channels : 1;
            else
                bufferCount = sound.Buffers.Count;
            
            IDs.AddRange(AL.GenSources(bufferCount));
            DotGame.OpenAL.AudioDevice.CheckALError();
            
            if (sound.IsStreamed)
            {
                ringbuffers = new AudioBuffer<short>[bufferCount, RINGBUFFER_COUNT];
                for (int i = 0; i < bufferCount; i++)
                    for (int j = 0; j < RINGBUFFER_COUNT; j++)
                        ringbuffers[i, j] = new AudioBuffer<short>(AudioDeviceInternal, false);
                ringBufferIndex = 0;
            }
            else
            {
                var buffers = sound.Buffers;
                for (int i = 0; i < buffers.Count; i++)
                {
                    AL.Source(IDs[i], ALSourcei.Buffer, buffers[i].ID);
                    DotGame.OpenAL.AudioDevice.CheckALError();
                }
            }

            if (AudioDevice.Capabilities.SupportsEfx)
            {
                directFilter = AudioDeviceInternal.Efx.GenFilter();
                AudioDeviceInternal.Efx.Filter(directFilter, EfxFilteri.FilterType, (int)EfxFilterType.Lowpass);
                Set(ALSourcei.EfxDirectFilter, directFilter);
            }

            sound.Register(this);
        }

        public void Route(int slot, IMixerChannel route)
        {
            lock (locker)
            {
                AssertNotDisposed();

                if (AudioDevice.Capabilities.SupportsEfx)
                {
                    if (slot < 0 && slot >= AudioDevice.MaxRoutes)
                        throw new ArgumentOutOfRangeException("slot", "Slot must be between 0 and MaxRoutes.");

                    for (int i = 0; i < IDs.Count; i++)
                    {
                        if (route == null)
                            AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(IDs[i], 0, slot, 0);
                        else
                            AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(IDs[i], ((MixerChannel)route).ID, slot, 0);
                    }
                    DotGame.OpenAL.AudioDevice.CheckALError();
                }
            }
        }

        public void Play()
        {
            lock (locker)
            {
                AssertNotDisposed();

                var state = AL.GetSourceState(IDs[0]);
                if (Sound.IsStreamed && ((state == ALSourceState.Stopped && !manualPause) || state == ALSourceState.Playing))
                    Stop();
                manualPause = false;

                DotGame.OpenAL.AudioDevice.CheckALError();

                for (int i = 0; i < IDs.Count; i++)
                    AL.SourcePlay(IDs[i]);

                DotGame.OpenAL.AudioDevice.CheckALError();
            }
        }

        public void Pause()
        {
            lock (locker)
            {
                AssertNotDisposed();
                manualPause = true;

                for (int i = 0; i < IDs.Count; i++)
                    AL.SourcePause(IDs[i]);
                DotGame.OpenAL.AudioDevice.CheckALError();
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                AssertNotDisposed();
                manualPause = false;

                for (int i = 0; i < IDs.Count; i++)
                {
                    AL.SourceStop(IDs[i]);
                    DotGame.OpenAL.AudioDevice.CheckALError();

                    if (Sound.IsStreamed)
                    {
                        int processed;
                        AL.GetSource(IDs[i], ALGetSourcei.BuffersProcessed, out processed);
                        DotGame.OpenAL.AudioDevice.CheckALError();
                        if (processed > 0)
                            AL.SourceUnqueueBuffers(IDs[i], processed);
                        DotGame.OpenAL.AudioDevice.CheckALError();
                    }
                }

                if (Sound.IsStreamed)
                {
                    source.Position = 0;
                    Refill(1);
                }
            }
        }

        internal override void Update()
        {
            lock (locker)
            {
                AssertNotDisposed();

                if (Sound.IsStreamed)
                {
                    int processed = Get(ALGetSourcei.BuffersProcessed);
                    if (processed > 0)
                    {
                        int[] ids = new int[processed];
                        foreach (var ID in IDs)
                            AL.SourceUnqueueBuffers(ID, processed, ids);
                        OpenAL.AudioDevice.CheckALError();

                        Refill(processed);
                    }

                    int extra = RINGBUFFER_COUNT - Get(ALGetSourcei.BuffersQueued);
                    if (extra > 0)
                        Refill(extra);
                }
            }
        }

        private void Refill(int count)
        {
            lock (locker)
            {
                if (source.Position >= source.TotalSamples)
                {
                    if (isLooping)
                        source.Position = 0;
                    else
                        return;
                }

                var state = AL.GetSourceState(IDs[0]);
                for (int a = 0; a < count; a++)
                {
                    var sampleRate = source.SampleRate;
                    var bufferCount = Sound.Supports3D ? source.Channels : 1;
                    var channelCount = Sound.Supports3D ? 1 : source.Channels;
                    float[] samples;
                    samples = source.ReadSamples(RINGBUFFER_SAMPLES);
                    if (samples.Length == 0)
                        return;

                    short[] samplesChannel = new short[samples.Length / bufferCount];
                    for (int i = 0; i < bufferCount; i++)
                    {
                        SampleConverter.To16Bit(samples, samplesChannel, i, bufferCount);
                        var buffer = ringbuffers[i, ringBufferIndex];
                        buffer.SetData(AudioFormat.Short16, channelCount, samplesChannel, sampleRate);
                        AL.SourceQueueBuffer(IDs[i], buffer.ID);
                    }
                    ringBufferIndex = (ringBufferIndex + 1) % RINGBUFFER_COUNT;
                }
                var newState = AL.GetSourceState(IDs[0]);
                if (newState != ALSourceState.Playing && state == ALSourceState.Playing)
                    Play();
            }
        }

        private void Assert3D()
        {
            if (!Sound.Supports3D)
                throw new NotSupportedException("This sound does not support 3D playback.");
        }

        private void AssertStreamed()
        {
            if (!Sound.IsStreamed)
                throw new NotSupportedException("This sound is not streamed.");
        }

        private void AssertReadable()
        {
            if (!Sound.AllowRead)
                throw new NotSupportedException("This sound does not support reading sample data.");
        }

        private void Set(ALSourceb param, bool value)
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSourcei param, int value)
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSourcef param, float value)
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSource3f param, Vector3 value)
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value.X, value.Y, value.Z);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private bool Get(ALSourceb param)
        {
            AssertNotDisposed();
            bool result;
            AL.GetSource(IDs[0], param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private int Get(ALGetSourcei param)
        {
            AssertNotDisposed();
            int result;
            AL.GetSource(IDs[0], param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private float Get(ALSourcef param)
        {
            AssertNotDisposed();
            float result;
            AL.GetSource(IDs[0], param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private Vector3 Get(ALSource3f param)
        {
            AssertNotDisposed();
            Vector3 result;
            AL.GetSource(IDs[0], param, out result.X, out result.Y, out result.Z);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private float GetPeak()
        {
            // TODO (Joex3): Der Code muss überarbeitet bzw optimiert werden.
            AssertNotDisposed();
            AssertReadable();
            if (Sound.IsStreamed)
            {
                int sample = Get(ALGetSourcei.SampleOffset);
                int bufferIndex = (ringBufferIndex + Get(ALGetSourcei.BuffersProcessed)) % RINGBUFFER_COUNT;
                var b = ringbuffers[0, bufferIndex];
                if (b.Data == null) // Buffer hat noch keine Daten -> 0.0f zurückgeben.
                    return 0.0f;
                while (sample >= b.Data.Count / b.Channels)
                {
                    sample -= b.Data.Count / b.Channels;
                    if (bufferIndex + 1 == ringBufferIndex)
                        throw new Exception();
                    bufferIndex = (bufferIndex + 1) % RINGBUFFER_COUNT;
                    b = ringbuffers[0, bufferIndex];
                }
                float maxPeak = 0.0f;
                var bufferCount = Sound.Supports3D ? source.Channels : 1;
                for (int i = 0; i < bufferCount; i++)
                {
                    var buffer = ringbuffers[i, bufferIndex];
                    int frameSize = Math.Min(Math.Max((int)(buffer.Frequency * Pitch / PEAK_FRAME_SIZE), 1), buffer.Data.Count / buffer.Channels);
                    int start = Math.Max(0, sample - frameSize);
                    for (int j = 0; j < frameSize; j++)
                        for (int k = 0; k < buffer.Channels; k++)
                            maxPeak = Math.Max(maxPeak, Math.Abs(buffer.Data[k + (start + j) * buffer.Channels] / (float)short.MaxValue));
                }

                return maxPeak;
            }
            else
            {
                var sound = (Sound)Sound;
                int sample = Get(ALGetSourcei.SampleOffset);
                float maxPeak = 0.0f;
                foreach (var buffer in sound.Buffers)
                {
                    int frameSize = Math.Min(Math.Max((int)(buffer.Frequency * Pitch / PEAK_FRAME_SIZE), 1), buffer.Data.Count / buffer.Channels);
                    int start = Math.Max(0, sample - frameSize);
                    for (int i = 0; i < frameSize; i++)
                        for (int j = 0; j < buffer.Channels; j++)
                            maxPeak = Math.Max(maxPeak, Math.Abs(buffer.Data[j + (start + i) * buffer.Channels] / (float)short.MaxValue));
                }

                return maxPeak;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            for (int i = 0; i < IDs.Count; i++)
                AL.DeleteSource(IDs[i]);
            OpenAL.AudioDevice.CheckALError();

            if (Sound.IsStreamed)
            {
                foreach (var buffer in ringbuffers)
                    buffer.Dispose();
            }

            if (AudioDevice.Capabilities.SupportsEfx)
            {
                AudioDeviceInternal.Efx.DeleteFilter(directFilter);
                OpenAL.AudioDevice.CheckALError();
            }

            base.Dispose(isDisposing);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is ISoundInstance)
                return Equals((ISoundInstance)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(ISoundInstance other)
        {
            if (other is SoundInstance)
                return IDs.SequenceEqual(((SoundInstance)other).IDs);
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < IDs.Count; i++)
                    hash = hash * 23 + IDs[i].GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[SoundInstance Sound: ");
            builder.Append(Sound);
            builder.Append(", IDs: ");
            builder.Append(IDs.Count);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
