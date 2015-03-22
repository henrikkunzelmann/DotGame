using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Audio;
using DotGame.Utils;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Collections.ObjectModel;

namespace DotGame.OpenAL
{
    public sealed class AudioDevice : IAudioDevice
    {
        public static IList<string> AvailableDevices { get { return AudioContext.AvailableDevices; } }
        public static string DefaultDevice { get { return AudioContext.DefaultDevice; } }

        public bool IsDisposed { get; private set; }
        public IAudioFactory Factory { get; private set; }
        public string DeviceName { get; private set; }
        public string VendorName { get; private set; }
        public string Renderer { get; private set; }
        public string DriverVersion { get; private set; }
        public Version Version { get; private set; }
        public Version EfxVersion { get; private set; }
        public ReadOnlyCollection<string> Extensions { get; private set; }
        public int MaxRoutes { get; private set; }

        public IMixerChannel MasterChannel { get; private set; }
        public IAudioListener Listener { get; private set; }

        internal readonly OpenTK.Audio.AudioContext Context;
        internal readonly OpenTK.Audio.OpenAL.EffectsExtension Efx;

        private readonly IntPtr DeviceHandle;

        internal static void CheckALError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new InvalidOperationException(AL.GetErrorString(error));
            }
        }

        internal void CheckAlcError()
        {
            var error = Alc.GetError(DeviceHandle);
            if (error != AlcError.NoError)
            {
                throw new InvalidOperationException(error.ToString());
            }
        }

        internal static ALFormat GetFormat(AudioFormat format, int channels)
        {
            switch (format)
            {
                case AudioFormat.Byte8:
                    if (channels == 1)
                        return ALFormat.Mono8;
                    else if (channels == 2)
                        return ALFormat.Stereo8;
                    break;
                case AudioFormat.Short16:
                    if (channels == 1)
                        return ALFormat.Mono16;
                    else if (channels == 2)
                        return ALFormat.Stereo16;
                    break;
                case AudioFormat.Float32:
                    if (channels == 1)
                        return ALFormat.MonoFloat32Ext;
                    else if (channels == 2)
                        return ALFormat.StereoFloat32Ext;
                    break;
            }

            throw new NotImplementedException("Format {0} with {1} channels is not supported!");
        }

        public AudioDevice() : this(null)
        {
        }

        public AudioDevice(string device)
        {
            if (device != null && !AvailableDevices.Contains(device))
                throw new InvalidOperationException(string.Format("AudioDevice \"{0}\" does not exist.", device));

            Context = new OpenTK.Audio.AudioContext(device, 0, 15, true, true, AudioContext.MaxAuxiliarySends.UseDriverDefault);
            CheckAlcError();
            Efx = new EffectsExtension();
            CheckAlcError();

            DeviceHandle = Alc.GetContextsDevice(Alc.GetCurrentContext());
            int[] val = new int[4];
            DeviceName = Context.CurrentDevice;
            VendorName = AL.Get(ALGetString.Vendor);
            Renderer = AL.Get(ALGetString.Renderer);
            DriverVersion = AL.Get(ALGetString.Version);
            int major, minor;
            Alc.GetInteger(DeviceHandle, AlcGetInteger.MajorVersion, 1, val);
            major = val[0];
            Alc.GetInteger(DeviceHandle, AlcGetInteger.MinorVersion, 1, val);
            minor = val[0];
            Version = new Version(major, minor);
            Alc.GetInteger(DeviceHandle, AlcGetInteger.EfxMajorVersion, 1, val);
            major = val[0];
            Alc.GetInteger(DeviceHandle, AlcGetInteger.EfxMinorVersion, 1, val);
            minor = val[0];
            EfxVersion = new Version(major, minor);
            Alc.GetInteger(DeviceHandle, AlcGetInteger.EfxMaxAuxiliarySends, 1, val);
            MaxRoutes = val[0];
            Extensions = new List<string>(AL.Get(ALGetString.Extensions).Split(' ')).AsReadOnly();

            MaxRoutes = val[0];

            AL.DistanceModel(ALDistanceModel.ExponentDistance);

            LogDiagnostics(LogLevel.Verbose);

            Factory = new AudioFactory(this);
            MasterChannel = Factory.CreateMixerChannel("Master");
            Listener = new AudioListener(this);
        }

        ~AudioDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void LogDiagnostics(LogLevel logLevel)
        {
            Log.Write(logLevel, "OpenAL Diagnostics:");
            Log.Write(logLevel, "\tDevice Name: " + DeviceName);
            Log.Write(logLevel, "\tVendor Name: " + VendorName);
            Log.Write(logLevel, "\tRenderer: " + Renderer);
            Log.Write(logLevel, "\tDriver Version: " + DriverVersion);
            Log.Write(logLevel, "\tOpenAL Version: " + Version);
            Log.Write(logLevel, "\tEfx Version: " + EfxVersion);
            Log.Write(logLevel, "\tExtensions supported:");
            for (int i = 0; i < Extensions.Count; i++)
            {
                Log.Write(logLevel, "\t\t" + Extensions[i] + ((i < Extensions.Count - 1) ? "," : ""));
            }
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
