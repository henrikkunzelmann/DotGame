using System;

namespace DotGame.Audio.SampleSources
{
    public abstract class SampleSourceBase
    {
        public bool IsDisposed { get; private set; }

        ~SampleSourceBase()
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
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            IsDisposed = true;
        }
    }
}
