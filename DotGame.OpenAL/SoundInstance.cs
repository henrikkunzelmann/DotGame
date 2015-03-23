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

        public Vector3 Position { get { AssertNotDisposed(); Assert3D(); return Get(ALSource3f.Position); } set { AssertNotDisposed(); Assert3D(); Set(ALSource3f.Position, value); } }
        public Vector3 Velocity { get { AssertNotDisposed(); Assert3D(); return Get(ALSource3f.Velocity); } set { AssertNotDisposed(); Assert3D(); Set(ALSource3f.Velocity, value); } }
        public float Gain { get { AssertNotDisposed(); return Get(ALSourcef.Gain); } set { AssertNotDisposed(); Set(ALSourcef.Gain, value); } }
        public float Pitch { get { return Get(ALSourcef.Pitch); } set { AssertNotDisposed(); Set(ALSourcef.Pitch, value); } }
        public bool IsLooping { get { AssertNotDisposed(); return Get(ALSourceb.Looping); } set { AssertNotDisposed(); Set(ALSourceb.Looping, value); } }

        internal readonly List<int> IDs;

        public SoundInstance(Sound sound, bool isPaused) : base(sound == null ? null : (AudioDevice)sound.AudioDevice)
        {
            if (sound == null)
                throw new ArgumentNullException("sound");
            if (sound.IsDisposed)
                throw new ObjectDisposedException(sound.GetType().FullName);

            this.Sound = sound;

            IDs = new List<int>();
            var buffers = sound.Buffers;
            for (int i = 0; i < buffers.Count; i++)
            {
                var ID = AL.GenSource();
                DotGame.OpenAL.AudioDevice.CheckALError();
                AL.Source(ID, ALSourcei.Buffer, buffers[i].ID);
                DotGame.OpenAL.AudioDevice.CheckALError();
                IDs.Add(ID);
            }
            Route(0, AudioDevice.MasterChannel);

            if (!isPaused)
                Play();
        }

        public void Route(int slot, IMixerChannel route)
        {
            if (slot < 0 && slot >= AudioDevice.MaxRoutes)
                throw new ArgumentOutOfRangeException("slot", "slot must be between 0 and MaxRoutes.");

            for (int i = 0; i < IDs.Count; i++)
            {
                if (route == null)
                    AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(IDs[i], 0, slot, 0);
                else
                    AudioDeviceInternal.Efx.BindSourceToAuxiliarySlot(IDs[i], ((MixerChannel)route).ID, slot, 0);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Play()
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.SourcePlay(IDs[i]);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Pause()
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.SourcePause(IDs[i]);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void Stop()
        {
            AssertNotDisposed();
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.SourceStop(IDs[i]);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Assert3D()
        {
            if (!Sound.Supports3D)
                throw new NotSupportedException("This sound does not support 3D playback.");
        }

        private void Set(ALSourceb param, bool value)
        {
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSourcef param, float value)
        {
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private void Set(ALSource3f param, Vector3 value)
        {
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.Source(IDs[i], param, value.X, value.Y, value.Z);
            }
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        private bool Get(ALSourceb param)
        {
            bool result;
            AL.GetSource(IDs[0], param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private float Get(ALSourcef param)
        {
            float result;
            AL.GetSource(IDs[0], param, out result);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        private Vector3 Get(ALSource3f param)
        {
            Vector3 result;
            AL.GetSource(IDs[0], param, out result.X, out result.Y, out result.Z);
            DotGame.OpenAL.AudioDevice.CheckALError();
            return result;
        }

        protected override void Dispose(bool isDisposing)
        {
            for (int i = 0; i < IDs.Count; i++)
            {
                AL.DeleteSource(IDs[i]);
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
