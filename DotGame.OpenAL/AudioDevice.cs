using DotGame.Audio;
using DotGame.Utils;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotGame.OpenAL
{
    public sealed class AudioDevice : IAudioDevice
    {
        public static IList<string> AvailableDevices { get { return AudioContext.AvailableDevices; } }
        public static string DefaultDevice { get { return AudioContext.DefaultDevice; } }

        public bool IsDisposed { get; private set; }
        public AudioCapabilities Capabilities { get { return capabilities; } }
        private AudioCapabilities capabilities;
        public IAudioFactory Factory { get; private set; }
        public string DeviceName { get; private set; }
        public string VendorName { get; private set; }
        public string Renderer { get; private set; }
        public string DriverVersion { get; private set; }
        public Version Version { get; private set; }
        public Version EfxVersion { get; private set; }
        public ReadOnlyCollection<string> Extensions { get; private set; }
        public int MaxRoutes { get; private set; }

        public IAudioListener Listener { get; private set; }

        internal readonly OpenTK.Audio.AudioContext Context;
        internal readonly OpenTK.Audio.OpenAL.EffectsExtension Efx;

        private readonly IntPtr deviceHandle;
        private readonly Task updateTask;
        private readonly CancellationTokenSource updateTaskCancelation;

        internal static void CheckALError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new InvalidOperationException(AL.GetErrorString(error));
            }
        }

        public AudioDevice() : this(DefaultDevice)
        {
        }

        public AudioDevice(string deviceName)
        {
            if (deviceName != null && !AvailableDevices.Contains(deviceName))
                throw new InvalidOperationException(string.Format("AudioDevice \"{0}\" does not exist.", deviceName));

            Context = new OpenTK.Audio.AudioContext(deviceName, 0, 15, true, true, AudioContext.MaxAuxiliarySends.UseDriverDefault);
            CheckAlcError();
            deviceHandle = Alc.GetContextsDevice(Alc.GetCurrentContext());
            CheckAlcError();
            Efx = new EffectsExtension();
            CheckAlcError();

            int[] val = new int[4];
            DeviceName = Context.CurrentDevice;
            VendorName = AL.Get(ALGetString.Vendor);
            Renderer = AL.Get(ALGetString.Renderer);
            DriverVersion = AL.Get(ALGetString.Version);
            int major, minor;
            Alc.GetInteger(deviceHandle, AlcGetInteger.MajorVersion, 1, val);
            major = val[0];
            Alc.GetInteger(deviceHandle, AlcGetInteger.MinorVersion, 1, val);
            minor = val[0];
            Version = new Version(major, minor);
            Alc.GetInteger(deviceHandle, AlcGetInteger.EfxMajorVersion, 1, val);
            major = val[0];
            Alc.GetInteger(deviceHandle, AlcGetInteger.EfxMinorVersion, 1, val);
            minor = val[0];
            EfxVersion = new Version(major, minor);
            Alc.GetInteger(deviceHandle, AlcGetInteger.EfxMaxAuxiliarySends, 1, val);
            MaxRoutes = val[0];
            Extensions = new List<string>(AL.Get(ALGetString.Extensions).Split(' ')).AsReadOnly();
            
            AL.DistanceModel(ALDistanceModel.ExponentDistance);

            CheckAudioCapabilities(LogLevel.Verbose);
            LogDiagnostics(LogLevel.Verbose);

            Factory = new AudioFactory(this);
            Listener = new AudioListener(this);
            Listener.Orientation(Vector3.UnitY, Vector3.UnitZ);

            updateTaskCancelation = new CancellationTokenSource();
            updateTask = Task.Factory.StartNew(Update);
        }

        ~AudioDevice()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        private void CheckAudioCapabilities(LogLevel logLevel)
        {
            capabilities = new AudioCapabilities();

            capabilities.SupportsEfx = Efx.IsInitialized;
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

            Log.WriteFields(logLevel, capabilities);
        }

        public T Cast<T>(IAudioObject obj, string parameterName) where T : class, IAudioObject
        {
            T ret = obj as T;
            if (ret == null)
                throw new ArgumentException("AudioObject is not part of this api.", parameterName);
            if (obj.AudioDevice != this)
                throw new ArgumentException("AudioDevice is not part of this audio device.", parameterName);
            return ret;
        }

        internal void CheckAlcError()
        {
            var error = Alc.GetError(deviceHandle);
            if (error != AlcError.NoError)
            {
                throw new InvalidOperationException(error.ToString());
            }
        }

        private void Update()
        {
            var token = updateTaskCancelation.Token;
            token.ThrowIfCancellationRequested();

            while (true)
            {
                if (updateTaskCancelation.Token.IsCancellationRequested)
                {
                    Log.Debug("Stopping Audiothread...");
                    break;
                }

                var factory = (AudioFactory)Factory;
                AudioObject obj;
                factory.ObjectsLock.EnterReadLock();
                try
                {
                    foreach (var reference in factory.Objects)
                    {
                        if (reference.TryGetTarget(out obj))
                            obj.Update();
                    }
                }
                finally
                {
                    factory.ObjectsLock.ExitReadLock();
                }

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            Log.Info("AudioDevice.Dispose() called!");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            updateTaskCancelation.Cancel();
            updateTask.Wait(2000);

            updateTaskCancelation.Dispose();
            Factory.Dispose();
            CheckAlcError();
            CheckALError();

            Context.Dispose();
            IsDisposed = true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is AudioDevice)
                return Equals((IAudioDevice)obj);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(IAudioDevice other)
        {
            if (other is AudioDevice)
                return deviceHandle == ((AudioDevice)other).deviceHandle;
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + deviceHandle.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[AudioDevice Handle: ");
            builder.Append(deviceHandle);
            builder.Append(", Device: ");
            builder.Append(DeviceName);
            builder.Append("]");

            return builder.ToString();
        }
    }
}
