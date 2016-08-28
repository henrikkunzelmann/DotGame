using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Kapselt diverse Einstellungen, die zum Erstellen und benutzen eines Sounds notwendig sind.
    /// </summary>
    [Flags]
    public enum SoundFlags
    {
        /// <summary>
        /// Entspricht keinen gesetzten flags.
        /// </summary>
        None = 0,

        /// <summary>
        /// Gibt an, dass der Sound für lokalisierbares 3D konfiguriert werden soll. Wenn gesetzt, wird je ein Buffer für jeden Channel erzeugt.
        /// </summary>
        Support3D = 1,

        /// <summary>
        /// Gibt an, dass der Sound aus der Datei dynamisch gestreamed werden soll. Wenn gesetzt, wird der Speicherverbrauch reduziert.
        /// </summary>
        Streamed = 2,

        /// <summary>
        /// Gibt an, dass die Samples des Sounds zum Auslesen verfügbar sein sollen. Wenn gesetzt, werden diverse Eigenschaften verfügbar (z.B. ISoundInstance.Peak).
        /// </summary>
        AllowRead = 4,
    }
}
