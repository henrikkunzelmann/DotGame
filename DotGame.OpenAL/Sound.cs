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
        public string File { get; private set; }
        public SoundFlags Flags { get; private set; }
        public bool Supports3D { get; private set; }
        public bool IsStreamed { get; private set; }
        public bool AllowRead { get; private set; }

        internal ReadOnlyCollection<AudioBuffer<short>> Buffers { get { return buffers.AsReadOnly(); } }
        private readonly List<AudioBuffer<short>> buffers;
        private readonly List<WeakReference<SoundInstance>> instances;

        internal Sound(AudioDevice audioDevice, string file, SoundFlags flags) : base(audioDevice)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");
            if (!System.IO.File.Exists(file))
                throw new System.IO.FileNotFoundException("file", file);

            this.File = file;
            this.Flags = flags;
            this.Supports3D = flags.HasFlag(SoundFlags.Support3D);
            this.IsStreamed = flags.HasFlag(SoundFlags.Streamed);
            this.AllowRead = flags.HasFlag(SoundFlags.AllowRead);

            instances = new List<WeakReference<SoundInstance>>();

            if (!IsStreamed)
            {
                using (var source = SampleSourceFactory.FromFile(file))
                {
                    var bufferCount = Supports3D ? source.Channels : 1;
                    var channelCount = Supports3D ? 1 : source.Channels;
                    var sampleRate = source.SampleRate;
                    var samples = source.ReadAll();

                    buffers = new List<AudioBuffer<short>>();
                    short[] samplesChannel = new short[samples.Length / bufferCount];
                    for (int i = 0; i < bufferCount; i++)
                    {
                        SampleConverter.To16Bit(samples, samplesChannel, i, bufferCount);
                        var buffer = new AudioBuffer<short>(audioDevice, !AllowRead);
                        buffer.SetData(AudioFormat.Short16, channelCount, samplesChannel, sampleRate);
                        buffers.Add(buffer);
                    }
                }
            }
        }

        public ISoundInstance CreateInstance()
        {
            return new SoundInstance(AudioDeviceInternal, this);
        }

        internal void Register(SoundInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");
            if (instance.IsDisposed)
                throw new ArgumentException("instance is disposed.", "instance");

            instances.Add(new WeakReference<SoundInstance>(instance));
        }

        internal override void Update()
        {
            if (IsStreamed)
            {
                SoundInstance instance;
                for (int i = 0; i < instances.Count; i++)
                {
                    if (instances[i].TryGetTarget(out instance))
                    {
                        if (instance.IsDisposed)
                            instances.RemoveAt(i--);
                        else
                            instance.Update();
                    }
                    else
                    {
                        instances.RemoveAt(i--);
                    }
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            foreach (var inst in instances)
            {
                SoundInstance target;
                if (inst.TryGetTarget(out target))
                {
                    target.Dispose();
                }
            }

            if (!IsStreamed)
            {
                foreach (var buffer in buffers)
                {
                    buffer.Dispose();
                }
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
