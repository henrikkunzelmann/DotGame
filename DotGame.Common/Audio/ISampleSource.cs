using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt eine Quelle an Sampledaten dar.
    /// </summary>
    public interface ISampleSource : IAudioObject
    {
        /// <summary>
        /// Ruft die Anzahl an Samples ab.
        /// </summary>
        long TotalSamples { get; }

        /// <summary>
        /// Ruft die aktuelle Position (in Samples) ab, oder legt diese fest.
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// Ruft das native Format der Daten ab.
        /// </summary>
        AudioFormat NativeFormat { get; }

        /// <summary>
        /// Ruft die Anzahl an Channeln ab.
        /// </summary>
        int Channels { get; }

        /// <summary>
        /// Ruft die Samplerate (Anzahl Samples pro Sekunde) ab.
        /// </summary>
        int SampleRate { get; }

        /// <summary>
        /// Ließt eine bestimmte Anzahl an Samples.
        /// Der zurückgelieferte Array kann kleiner sein, als die angegebene Anzahl.
        /// </summary>
        /// <param name="count">Die maximale Anzahl an Samples.</param>
        /// <returns>Die Samples.</returns>
        float[] ReadSamples(int count);

        /// <summary>
        /// Ließt eine bestimmte Anzahl an Samples.
        /// Der übergebene Array muss die angegebene Anzahl an Samples beeinhalten können.
        /// </summary>
        /// <param name="offset">Der Abstand zum Anfang der Samples im übergebenen Buffer.</param>
        /// <param name="count">Die maximale Anzahl an Samples.</param>
        /// <param name="buffer">Der Buffer.</param>
        void ReadSamples(int offset, int count, float[] buffer);

        /// <summary>
        /// Ließt alle restlichen Samples.
        /// Der übergebene Array muss die angegebene Anzahl an Samples beeinhalten können.
        /// </summary>
        /// <param name="buffer">Der Buffer.</param>
        float[] ReadAll();

        /// <summary>
        /// Ließt alle restlichen Samples.
        /// Der übergebene Array muss die Samples beeinhalten können.
        /// </summary>
        /// <param name="offset">Der Abstand zum Anfang der Samples im übergebenen Buffer.</param>
        /// <param name="buffer">Der Buffer.</param>
        void ReadAll(int offset, float[] buffer);
    }
}
