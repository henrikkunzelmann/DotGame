using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt Methoden zum Erstellen von Ressourcen bereit.
    /// </summary>
    public interface IAudioFactory : IEquatable<IAudioFactory>
    {
        /// <summary>
        /// Erstellt einen Sound, der später zum Abspielen instanziiert werden kann.
        /// </summary>
        /// <param name="file">Die Datei, aus der der Sound geladen wird.</param>
        /// <param name="supports3D">Gibt an, ob der Sound für lokalisierbares 3D konfiguriert werden soll. Wenn true, wird je ein Buffer für jeden Channel erzeugt.</param>
        /// <returns>Den Sound.</returns>
        /// <remarks>Unterstütze Formate: wav, ogg/vorbis</remarks>
        [Obsolete("Use CreateSound(ISampleSource, bool) instead.")]
        ISound CreateSound(string file, bool supports3D);

        /// <summary>
        /// Erstellt einen Sound, der später zum Abspielen instanziiert werden kann.
        /// </summary>
        /// <param name="source">Die SampleSource, aus der die Rohdaten geladen werden.</param>
        /// <param name="supports3D">Gibt an, ob der Sound für lokalisierbares 3D konfiguriert werden soll. Wenn true, wird je ein Buffer für jeden Channel erzeugt.</param>
        /// <returns>Den Sound.</returns>
        /// <remarks>Unterstütze Formate: wav, ogg/vorbis</remarks>
        ISound CreateSound(ISampleSource source, bool supports3D);

        /// <summary>
        /// Erstellt eine ISampleSource-Instanz für die angegebene Datei, die zum Laden bzw. Streamen von Audio genutzt werden kann.
        /// </summary>
        /// <param name="file">Die Datei.</param>
        /// <returns>Die SampleSource.</returns>
        /// <remarks>Unterstütze Formate: wav, ogg/vorbis</remarks>
        ISampleSource CreateSampleSource(string file);

        /// <summary>
        /// Erstellt eine IMixerChannel-Instanz, die zum Anwenden von diversen Effekten auf ein einkommendes Audiosignal benutzt werden kann.
        /// </summary>
        /// <param name="name">Der Name des Channels.</param>
        /// <returns>Den MixerChannel.</returns>
        IMixerChannel CreateMixerChannel(string name);

        /// <summary>
        /// Erstellt einen Hall-Effekt.
        /// </summary>
        /// <returns>Den Effekt.</returns>
        IEffectReverb CreateReverb();
    }
}
