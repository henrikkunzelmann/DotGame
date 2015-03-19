using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using DotGame.Utils;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace DotGame.OpenAL
{
    public sealed class AudioDevice : IAudioDevice
    {
        public static IList<string> AvailableDevices { get { return AudioContext.AvailableDevices; } }
        public static string DefaultDevice { get { return AudioContext.DefaultDevice; } }

        public bool IsDisposed { get; private set; }
        public IAudioFactory Factory { get; private set; }
        public Version ApiVersion { get; private set; }
        public IMixerChannel MasterChannel { get; private set; }
        public int MaxRoutes { get; private set; }
        
        internal readonly OpenTK.Audio.AudioContext Context;
        internal readonly OpenTK.Audio.OpenAL.EffectsExtension Efx;

        private readonly IntPtr deviceHandle;

        internal static void CheckALError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new InvalidOperationException(AL.GetErrorString(error));
            }
        }

        public AudioDevice() : this(null)
        {
        }

        public AudioDevice(string device)
        {
            if (device == null)
                device = DefaultDevice;

            if (!AvailableDevices.Contains(device))
                throw new InvalidOperationException(string.Format("AudioDevice \"{0}\" does not exist.", device));

            Context = new OpenTK.Audio.AudioContext(device, 0, 15, true, true, AudioContext.MaxAuxiliarySends.UseDriverDefault);
            Efx = new EffectsExtension();

            deviceHandle = Alc.GetContextsDevice(Alc.GetCurrentContext());
            int[] val = new int[4];
            int major, minor;
            Alc.GetInteger(deviceHandle, AlcGetInteger.MajorVersion, 1, val);
            major = val[0];
            Alc.GetInteger(deviceHandle, AlcGetInteger.MinorVersion, 1, val);
            minor = val[1];
            ApiVersion = new Version(major, minor);
            Alc.GetInteger(deviceHandle, AlcGetInteger.EfxMaxAuxiliarySends, 1, val);
            MaxRoutes = val[0];

            AL.DistanceModel(ALDistanceModel.ExponentDistance);

            Log.Debug("Got context: [Device: \"{0}\", Version: {1}, MaxRoutes: {2}]",
                Context.CurrentDevice,
                ApiVersion,
                MaxRoutes);

            Factory = new AudioFactory(this);
            MasterChannel = Factory.CreateMixerChannel("Master");
        }

        ~AudioDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Log.Info("AudioDevice.Dispose() called!");
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            Context.Dispose();
            IsDisposed = true;
        }
    }
}
