using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class EffectReverb : Effect, IEffectReverb
    {
        public bool EnableAirAbsorption { get { return Get(EfxEffecti.ReverbDecayHFLimit) == 1; } set { Set(EfxEffecti.ReverbDecayHFLimit, value ? 1 : 0); } }
        public float AirAbsorptionGain { get { return Get(EfxEffectf.ReverbAirAbsorptionGainHF); } set { Set(EfxEffectf.ReverbAirAbsorptionGainHF, value); } }
        public float DecayFrequencyRatio { get { return Get(EfxEffectf.ReverbDecayHFRatio); } set { Set(EfxEffectf.ReverbDecayHFRatio, value); } }
        public float DecayTime { get { return Get(EfxEffectf.ReverbDecayTime); } set { Set(EfxEffectf.ReverbDecayTime, value); } }
        public float Density { get { return Get(EfxEffectf.ReverbDensity); } set { Set(EfxEffectf.ReverbDensity, value); } }
        public float Diffusion { get { return Get(EfxEffectf.ReverbDiffusion); } set { Set(EfxEffectf.ReverbDiffusion, value); } }
        public float Gain { get { return Get(EfxEffectf.ReverbGain); } set { Set(EfxEffectf.ReverbGain, value); } }
        public float GainDamp { get { return Get(EfxEffectf.ReverbGainHF); } set { Set(EfxEffectf.ReverbGainHF, value); } }
        public float LateReverbDelay { get { return Get(EfxEffectf.ReverbLateReverbDelay); } set { Set(EfxEffectf.ReverbLateReverbDelay, value); } }
        public float LateReverbGain { get { return Get(EfxEffectf.ReverbLateReverbGain); } set { Set(EfxEffectf.ReverbLateReverbGain, value); } }
        public float ReflectionsDelay { get { return Get(EfxEffectf.ReverbReflectionsDelay); } set { Set(EfxEffectf.ReverbReflectionsDelay, value); } }
        public float ReflectionsGain { get { return Get(EfxEffectf.ReverbReflectionsGain); } set { Set(EfxEffectf.ReverbReflectionsGain, value); } }
        public float RoomRolloffFactor { get { return Get(EfxEffectf.ReverbRoomRolloffFactor); } set { Set(EfxEffectf.ReverbRoomRolloffFactor, value); } }

        public EffectReverb(AudioDevice audioDevice) : base(audioDevice, EfxEffectType.Reverb)
        {
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Effect ID: ");
            builder.Append(ID);
            builder.Append(", Type: Reverb]");

            return builder.ToString();
        }
    }
}
