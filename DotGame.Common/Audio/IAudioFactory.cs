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
    public interface IAudioFactory : IDisposable, IEquatable<IAudioFactory>
    {
        /// <summary>
        /// Ruft das IAudioDevice ab, das dieser IAudioFactory zugeordnet ist.
        /// </summary>
        IAudioDevice AudioDevice { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Erstellt einen Sound, der später zum Abspielen instanziiert werden kann.
        /// </summary>
        /// <param name="file">Die Datei, aus der der Sound geladen wird.</param>
        /// <param name="flags">Die SoundFlags, die für diesen Sound benutzt werden sollen</param>
        /// <returns>Den Sound.</returns>
        /// <remarks>Unterstütze Formate: wav, ogg/vorbis</remarks>
        ISound CreateSound(string file, SoundFlags flags);

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
