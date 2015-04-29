using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.OpenAL
{
    public abstract class Effect : AudioObject, IEffect
    {
        internal readonly int ID;

        public Effect(AudioDevice audioDevice, EfxEffectType type) : base(audioDevice)
        {
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                ID = AudioDeviceInternal.Efx.GenEffect();
                AudioDeviceInternal.Efx.BindEffect(ID, type);
                OpenAL.AudioDevice.CheckALError();
            }
        }

        protected void Set(EfxEffecti param, int value)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                AudioDeviceInternal.Efx.Effect(ID, param, value);
                OpenAL.AudioDevice.CheckALError();
            }
        }

        protected void Set(EfxEffectf param, float value)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                AudioDeviceInternal.Efx.Effect(ID, param, value);
                OpenAL.AudioDevice.CheckALError();
            }
        }

        protected void Set(EfxEffect3f param, Vector3 value)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                OpenTK.Vector3 v = new OpenTK.Vector3(value.X, value.Y, value.Z);
                AudioDeviceInternal.Efx.Effect(ID, param, ref v);
                OpenAL.AudioDevice.CheckALError();
            }
        }

        protected int Get(EfxEffecti param)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                int value;
                AudioDeviceInternal.Efx.GetEffect(ID, param, out value);
                OpenAL.AudioDevice.CheckALError();
                return value;
            }
            return 0;
        }

        protected float Get(EfxEffectf param)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                float value;
                AudioDeviceInternal.Efx.GetEffect(ID, param, out value);
                OpenAL.AudioDevice.CheckALError();
                return value;
            }
            return 0;
        }

        protected Vector3 Get(EfxEffect3f param)
        {
            AssertNotDisposed();
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                OpenTK.Vector3 value;
                AudioDeviceInternal.Efx.GetEffect(ID, param, out value);
                OpenAL.AudioDevice.CheckALError();
                return new Vector3(value.X, value.Y, value.Z);
            }
            return Vector3.Zero;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (AudioDevice.Capabilities.SupportsEfx)
            {
                if (!AudioDeviceInternal.IsDisposed)
                    AudioDeviceInternal.Efx.DeleteEffect(ID);
            }

            base.Dispose(isDisposing);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is IEffect)
                return Equals((IEffect)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(IEffect other)
        {
            if (other is Effect)
                return ID == ((Effect)other).ID;
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ID.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Effect ID: ");
            builder.Append(ID);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
