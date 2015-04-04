using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt einen Sound dar, der beim Erstellen komplett geladen wird.
    /// </summary>
    public interface ISound : IAudioObject, IEquatable<ISound>
    {
        /// <summary>
        /// Ruft den Dateinamen ab, aus der der Sound geladen wird.
        /// </summary>
        string File { get; }

        /// <summary>
        /// Ruft diverse Einstellungen für den Sound ab.
        /// </summary>
        SoundFlags Flags { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob der Sound für eine 3D-Wiedergabe verfügbar ist.
        /// </summary>
        /// <remarks>Dieser Wert wurde aus den SoundFlags in der Flags-Eigenschaft genommen.</remarks>
        bool Supports3D { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob der Sound aus der Datei gestreamed wird.
        /// </summary>
        /// <remarks>Dieser Wert wurde aus den SoundFlags in der Flags-Eigenschaft genommen.</remarks>
        bool IsStreamed { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob die Samples des Sounds zum Auslesen verfügbar sind.
        /// </summary>
        /// <remarks>Dieser Wert wurde aus den SoundFlags in der Flags-Eigenschaft genommen.</remarks>
        bool AllowRead { get; }

        /// <summary>
        /// Erstellt eine Instanz des Sounds, die zum Abspielen genutzt wird.
        /// </summary>
        /// <returns>Die Instanz.</returns>
        ISoundInstance CreateInstance();
    }
}
