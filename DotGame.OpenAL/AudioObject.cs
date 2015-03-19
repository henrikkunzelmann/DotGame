using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;

namespace DotGame.OpenAL
{
    public abstract class AudioObject : IAudioObject
    {
        public IAudioDevice AudioDevice { get; private set; }
        public bool IsDisposed { get; private set; }
        public EventHandler<EventArgs> Disposing { get; set; }
        public object Tag { get; set; }

        internal readonly AudioDevice AudioDeviceInternal;

        public AudioObject(AudioDevice audioDevice)
        {
            if (audioDevice == null)
                throw new ArgumentNullException("audioDevice");
            if (audioDevice.IsDisposed)
                throw new ArgumentException("audioDevice is disposed.", "audioDevice");

            this.AudioDevice = audioDevice;
            this.AudioDeviceInternal = audioDevice;
        }

        ~AudioObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void AssertNotDisposed()
        {
            if (AudioDevice.IsDisposed)
                throw new ObjectDisposedException(AudioDevice.GetType().FullName);

            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (Disposing != null)
                Disposing.Invoke(this, EventArgs.Empty);

            IsDisposed = true;
        }
    }
}
