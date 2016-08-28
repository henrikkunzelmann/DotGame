using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace DotGame.OpenAL
{
    internal class AudioBuffer<TSampleType> : AudioObject, IAudioObject, IEquatable<AudioBuffer<TSampleType>> where TSampleType : struct
    {
        public readonly bool IsWriteOnly;
        public int ID { get; private set; }

        public AudioFormat Format { get; private set; }
        public int Channels { get; private set; }
        public int Frequency { get; private set; }
        public ReadOnlyCollection<TSampleType> Data { get; private set; }

        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private long lastLength = -1;

        public AudioBuffer(AudioDevice audioDevice, bool isWriteOnly) : base(audioDevice)
        {
            this.IsWriteOnly = isWriteOnly;
            this.ID = AL.GenBuffer();
            DotGame.OpenAL.AudioDevice.CheckALError();
        }

        public void SetData(AudioFormat format, int channels, TSampleType[] data, int frequency)
        {
            AssertNotDisposed();

            locker.EnterWriteLock();
            try
            {
                DotGame.OpenAL.AudioDevice.CheckALError();
                AL.BufferData<TSampleType>(ID, EnumConverter.GetFormat(format, channels), data, Marshal.SizeOf(data[0]) * data.Length, frequency);
                DotGame.OpenAL.AudioDevice.CheckALError();

                this.Format = format;
                this.Channels = channels;
                this.Frequency = frequency;

                lastLength = data.LongLength;

                if (!IsWriteOnly)
                    this.Data = Array.AsReadOnly(data);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        private void AssertReadable()
        {
            if (IsWriteOnly)
                throw new InvalidOperationException("AudioBuffer is writeonly!");
        }

        protected override void Dispose(bool isDisposing)
        {
            AL.DeleteBuffer(ID);
            locker.Dispose();

            base.Dispose(isDisposing);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is AudioBuffer<TSampleType>)
                return Equals((AudioBuffer<TSampleType>)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(AudioBuffer<TSampleType> other)
        {
            return ID == other.ID;
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
            builder.Append("[AudioBuffer ID: ");
            builder.Append(ID);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
