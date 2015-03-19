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
        public ISound Sound { get; private set; }

        public Vector3 Position { get { return Get(ALSource3f.Position); } set { Set(ALSource3f.Position, value); } }
        public Vector3 Velocity { get { return Get(ALSource3f.Velocity); } set { Set(ALSource3f.Velocity, value); } }
        public float DopplerScale { get; set; }
        public float Gain { get { return Get(ALSourcef.Gain); } set { Set(ALSourcef.Gain, value); } }
        public float Pitch { get { return Get(ALSourcef.Pitch); } set { Set(ALSourcef.Pitch, value); } }
        public bool IsLooping { get { return Get(ALSourceb.Looping); } set { Set(ALSourceb.Looping, value); } }

        internal readonly int ID;

        public SoundInstance(Sound sound, bool isPaused) : base(sound == null ? null : (AudioDevice)sound.AudioDevice)
        {
            if (sound == null)
                throw new ArgumentNullException("sound");
            if (sound.IsDisposed)
                throw new ObjectDisposedException(sound.GetType().FullName);

            this.Sound = sound;

            ID = AL.GenSource();
            DotGame.OpenAL.AudioDevice.CheckALError();
            AL.Source(ID, ALSourcei.Buffer, sound.Buffer.ID);
            DotGame.OpenAL.AudioDevice.CheckALError();
            Route(0, AudioDevice.MasterChannel);

            if (!isPaused)
                Play();
        }

        public void Route(int slot, IMixerChannel route)
        {
            if (slot < 0 && slot >= AudioDevice.MaxRoutes)
                throw new ArgumentOutOfRangeException("slot", "slot must be between 0 and MaxRoutes.");

            if (route == null)
                AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(ID, 0, slot, 0);
            else
                AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(ID, ((MixerChannel)route).ID, slot, 0);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Play()
        {
            AssertNotDisposed();
            AL.SourcePlay(ID);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Pause()
        {
            AssertNotDisposed();
            AL.SourcePause(ID);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Stop()
        {
            AssertNotDisposed();
            AL.SourceStop(ID);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSourceb param, bool value)
        {
            AL.Source(ID, param, value);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSourcef param, float value)
        {
            AL.Source(ID, param, value);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSource3f param, Vector3 value)
        {
            AL.Source(ID, param, value.X, value.Y, value.Z);
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private bool Get(ALSourceb param)
        {
            bool result;
            AL.GetSource(ID, param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private float Get(ALSourcef param)
        {
            float result;
            AL.GetSource(ID, param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private Vector3 Get(ALSource3f param)
        {
            Vector3 result;
            AL.GetSource(ID, param, out result.X, out result.Y, out result.Z);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }
    }
}
