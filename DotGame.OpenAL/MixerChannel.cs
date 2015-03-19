using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class MixerChannel : AudioObject, IMixerChannel
    {
        public float Gain { get { return Get(EfxAuxiliaryf.EffectslotGain); } set { Set(EfxAuxiliaryf.EffectslotGain, value); } }

        internal readonly int ID;

        public MixerChannel(AudioDevice audioDevice, string name) : base(audioDevice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            ID = AudioDeviceInternal.Efx.GenAuxiliaryEffectSlot();
            DotGame.OpenAL.AudioDevice.CheckALError();

            AudioDeviceInternal.Efx.AuxiliaryEffectSlot(ID, EfxAuxiliaryi.EffectslotAuxiliarySendAuto, 1);
        }

        private void Set(EfxAuxiliaryf param, float value)
        {
            AudioDeviceInternal.Efx.AuxiliaryEffectSlot(ID, param, value);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private float Get(EfxAuxiliaryf param)
        {
            float value;
            AudioDeviceInternal.Efx.GetAuxiliaryEffectSlot(ID, param, out value);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return value;
        }
    }
}
