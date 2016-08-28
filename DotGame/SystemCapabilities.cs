using DotGame.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotGame
{
    public class SystemCapabilities
    {
        private static List<GraphicsAPI> graphicsAPIs = new List<GraphicsAPI>();
        private static List<AudioAPI> audioAPIs = new List<AudioAPI>();

        static SystemCapabilities()
        {
            InitGraphicsAPIs();
            InitAudioAPIs();
        }

        /// <summary>
        /// Prüft, ob die gegebene Grafik-API zur Verfügung steht.
        /// </summary>
        /// <param name="GraphicsAPI">Die Grafik-API.</param>
        /// <returns>True, wenn die Grafik-API zur Verfügung steht, ansonsten false.</returns>
        public static bool IsSupported(GraphicsAPI GraphicsAPI)
        {
            return graphicsAPIs.Contains(GraphicsAPI);
        }

        /// <summary>
        /// Prüft, ob die gegebene Audio-API zur Verfügung steht.
        /// </summary>
        /// <param name="AudioAPI">Die Audio-API.</param>
        /// <returns>True, wenn die Audio-API zur Verfügung steht, ansonsten false.</returns>
        public static bool IsSupported(AudioAPI AudioAPI)
        {
            return audioAPIs.Contains(AudioAPI);
        }

        private static void InitGraphicsAPIs()
        {
            if (CheckLibrary("d3d11.dll"))
                graphicsAPIs.Add(GraphicsAPI.Direct3D11);

            if (!IsWindows || CheckLibrary("opengl32.dll"))
                graphicsAPIs.Add(GraphicsAPI.OpenGL4);
        }

        private static void InitAudioAPIs()
        {
            if (!IsWindows || CheckLibrary("OpenAL32.dll"))
                audioAPIs.Add(AudioAPI.OpenAL);
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32", SetLastError = true)]
        private static extern void FreeLibrary(IntPtr module);

        /// <summary>
        /// Returns true if the system is a windows system.
        /// </summary>
        private static bool IsWindows
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Win32NT;
            }
        }

        /// <summary>
        /// Checks the existence of a windows library.
        /// </summary>
        /// <param name="dll"></param>
        /// <returns></returns>
        public static bool CheckLibrary(string fileName)
        {
            if (!IsWindows)
                return false;

            try
            {
                IntPtr ptr = LoadLibrary(fileName);
                if (ptr == IntPtr.Zero) // Library not loaded
                    return false;

                FreeLibrary(ptr);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return false;
        }

    }
}
