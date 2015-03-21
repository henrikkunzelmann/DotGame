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
            return new Sound(AudioDeviceInternal, file, supports3D);
        }

        public ISampleSource CreateSampleSource(string file)
        {
            return new VorbisSampleSource(AudioDeviceInternal, file);
        }

        public IMixerChannel CreateMixerChannel(string name)
        {
            return new MixerChannel(AudioDeviceInternal, name);
        }

        public IEffectReverb CreateReverb()
        {
            return new EffectReverb(AudioDeviceInternal);
        }
    }
}
