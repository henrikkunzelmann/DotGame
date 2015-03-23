using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public class AudioListener : IAudioListener
    {
        public IAudioDevice AudioDevice { get; private set; }

        public float Gain { get { return Get(ALListenerf.Gain); } set { Set(ALListenerf.Gain, value); } }
        public Vector3 Position { get { return Get(ALListener3f.Position); } set { Set(ALListener3f.Position, value); } }
        public Vector3 Velocity { get { return Get(ALListener3f.Velocity); } set { Set(ALListener3f.Velocity, value); } }
        public Vector3 Up { get { Vector3 at, up; Get(ALListenerfv.Orientation, out at, out up); return at; } set { Set(ALListenerfv.Orientation, At, value); } }
        public Vector3 At { get { Vector3 at, up; Get(ALListenerfv.Orientation, out at, out up); return up; } set { Set(ALListenerfv.Orientation, Up, value); } }

        internal AudioListener(AudioDevice audioDevice)
        {
            this.AudioDevice = audioDevice;
        }

        public void Orientation(Vector3 up, Vector3 at)
        {
            Set(ALListenerfv.Orientation, up, at);
        }

        private void Set(ALListenerf param, float value)
        {
            AssertNotDisposed();

            AL.Listener(param, value);
        }

        private void Set(ALListener3f param, Vector3 value)
        {
            AssertNotDisposed();

            AL.Listener(param, value.X, value.Y, value.Z);
        }

        private void Set(ALListenerfv param, Vector3 at, Vector3 up)
        {
            AssertNotDisposed();

            float[] value = new float[6];
            value[0] = at.X;
            value[1] = at.Y;
            value[2] = at.Z;
            value[3] = up.X;
            value[4] = up.Y;
            value[5] = up.Z;
            AL.Listener(param, ref value);
        }

        private float Get(ALListenerf param)
        {
            AssertNotDisposed();

            float result;
            AL.GetListener(param, out result);
            return result;
        }

        private Vector3 Get(ALListener3f param)
        {
            AssertNotDisposed();
            
            Vector3 result;
            AL.GetListener(param, out result.X, out result.Y, out result.Z);
            return result;
        }

        private void Get(ALListenerfv param, out Vector3 at, out Vector3 up)
        {
            AssertNotDisposed();

            OpenTK.Vector3 _up, _at;
            AL.GetListener(param, out _at, out _up);
            at = new Vector3(_at.X, _at.Y, _at.Z);
            up = new Vector3(_up.X, _up.Y, _up.Z);
        }

        private void AssertNotDisposed()
        {
            if (AudioDevice.IsDisposed)
                throw new ObjectDisposedException(AudioDevice.GetType().FullName);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is IAudioListener)
                return Equals((IAudioListener)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(IAudioListener other)
        {
            if (other is AudioListener)
            {
                var o = (AudioListener)other;
                return AudioDevice == o.AudioDevice
                    && Gain == other.Gain
                    && Position == other.Position
                    && Velocity == other.Velocity
                    && Up == other.Up
                    && At == other.At;
            }
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + AudioDevice.GetHashCode();
                hash = hash * 23 + Gain.GetHashCode();
                hash = hash * 23 + Position.GetHashCode();
                hash = hash * 23 + Velocity.GetHashCode();
                hash = hash * 23 + Up.GetHashCode();
                hash = hash * 23 + At.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[AudioListener Gain: ");
            builder.Append(Gain);
            builder.Append(", Position: ");
            builder.Append(Position);
            builder.Append(", Velocity: ");
            builder.Append(Velocity);
            builder.Append(", Up: ");
            builder.Append(Up);
            builder.Append(", At: ");
            builder.Append(At);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
