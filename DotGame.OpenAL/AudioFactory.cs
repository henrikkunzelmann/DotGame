using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;

namespace DotGame.OpenAL
{
    public class AudioFactory : AudioObject, IAudioFactory
    {
        public AudioFactory(AudioDevice audioDevice) : base(audioDevice)
        {
        }

        public ISound CreateSound(string file, bool supports3D)
        {
            var source = CreateSampleSource(file);
            return new Sound(AudioDeviceInternal, source, supports3D);
        }

        public ISound CreateSound(ISampleSource source, bool supports3D)
        {
            return new Sound(AudioDeviceInternal, source, supports3D);
        }

        public ISampleSource CreateSampleSource(string file)
        {
            var magic = GetMagic(file);
            if (magic == ".wav")
                return new WaveSampleSource(AudioDeviceInternal, file);
            else if (magic == ".ogg")
                return new VorbisSampleSource(AudioDeviceInternal, file);

            throw new NotSupportedException(magic);
        }

        public IMixerChannel CreateMixerChannel(string name)
        {
            return new MixerChannel(AudioDeviceInternal, name);
        }

        public IEffectReverb CreateReverb()
        {
            return new EffectReverb(AudioDeviceInternal);
        }

        private string GetMagic(string file)
        {
            // TODO (Joex3): besseres handlen.
            return System.IO.Path.GetExtension(file);
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
