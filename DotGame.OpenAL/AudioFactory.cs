using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using System.Collections.ObjectModel;
using System.IO;

namespace DotGame.OpenAL
{
    public sealed class AudioFactory : IAudioFactory
    {
        public IAudioDevice AudioDevice { get; private set; }
        public bool IsDisposed { get; private set; }

        internal AudioDevice AudioDeviceInternal { get; private set; }

        internal ReadOnlyCollection<WeakReference<AudioObject>> Objects { get { return objects.AsReadOnly(); } }
        private readonly List<WeakReference<AudioObject>> objects;

        public AudioFactory(AudioDevice audioDevice)
        {
            this.AudioDevice = audioDevice;
            this.AudioDeviceInternal = audioDevice;

            objects = new List<WeakReference<AudioObject>>();
        }

        ~AudioFactory()
        {
            Dispose(false);
        }

        public ISound CreateSound(string file, SoundFlags flags)
        {
            return Register(new Sound(AudioDeviceInternal, file, flags));
        }

        public ISampleSource CreateSampleSource(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            var magic = GetMagic(file);
            if (magic == ".wav")
                return Register(new WaveSampleSource(AudioDeviceInternal, file));
            else if (magic == ".ogg")
                return Register(new VorbisSampleSource(AudioDeviceInternal, file));

            throw new NotSupportedException(magic);
        }

        public IMixerChannel CreateMixerChannel(string name)
        {
            return Register(new MixerChannel(AudioDeviceInternal, name));
        }

        public IEffectReverb CreateReverb()
        {
            return Register(new EffectReverb(AudioDeviceInternal));
        }

        private T Register<T>(T obj) where T : AudioObject
        {
            objects.Add(new WeakReference<AudioObject>(obj));
            return obj;
        }

        private string GetMagic(string file)
        {
            // TODO (Joex3): besseres handlen.
            return System.IO.Path.GetExtension(file);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void AssertNotDisposed()
        {
            if (AudioDevice.IsDisposed)
                throw new ObjectDisposedException(AudioDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private void Dispose(bool isDisposing)
        {
            foreach (var obj in objects)
            {
                AudioObject target;
                if (obj.TryGetTarget(out target))
                {
                    target.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is IAudioFactory)
                return Equals((IAudioFactory)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(IAudioFactory other)
        {
            if (other is AudioFactory)
                return AudioDevice == ((AudioFactory)other).AudioDevice;
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + AudioDevice.GetHashCode();
                hash = hash * 23 + 3357;
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[AudioFactory Device: ");
            builder.Append(AudioDevice);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
