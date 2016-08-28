using DotGame.Audio.SampleSources;
using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt statische Methoden zum Erstellen von diversend ISampleSource-Instanzen bereit.
    /// </summary>
    public static class SampleSourceFactory
    {
        /// <summary>
        /// Erstellt eine ISampleSource-Instanz für die angegebene Datei, die zum Laden bzw. Streamen von Audio genutzt werden kann.
        /// </summary>
        /// <param name="file">Die Datei.</param>
        /// <returns>Die SampleSource.</returns>
        /// <remarks>Unterstütze Formate: wav, ogg/vorbis</remarks>
        public static ISampleSource FromFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            var magic = GetMagic(file);
            if (magic == ".wav")
                return new WaveSampleSource(file);
            else if (magic == ".ogg")
                return new VorbisSampleSource(file);

            throw new NotSupportedException(magic);
        }

        private static string GetMagic(string file)
        {
            return System.IO.Path.GetExtension(file);
        }
    }
}
