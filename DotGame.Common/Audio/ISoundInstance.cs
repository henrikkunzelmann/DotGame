using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt eine Instanz eines bestimmten Sounds dar.
    /// </summary>
    public interface ISoundInstance : IAudioObject, IEquatable<ISoundInstance>
    {
        /// <summary>
        /// Der Sound, der als Basis für diese Instanz dient.
        /// </summary>
        ISound Sound { get; }

        /// <summary>
        /// Ruft die Peak-Amplitude der Instanz an der aktuellen Position ab.
        /// Diese Eigenschaft ist nur verfügbar, wenn SoundFlags.AllowRead in der Flag-Eigenschaft gesetzt ist.
        /// </summary>
        float Peak { get; }

        /// <summary>
        /// Ruft die Position der Instanz im Raum ab, oder legt diese fest.
        /// Diese Eigenschaft ist nur verfügbar, wenn SoundFlags.Support3D in der Flag-Eigenschaft gesetzt ist.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Ruft die Geschwindigkeit der Instanz im Raum ab, oder legt diese fest.
        /// Dieser Wert wird u.a. für den Dopplereffekt genutzt.
        /// Diese Eigenschaft ist nur verfügbar, wenn SoundFlags.Support3D in der Flag-Eigenschaft gesetzt ist.
        /// </summary>
        Vector3 Velocity { get; set; }

        /// <summary>
        /// Ruft die Lautstärke der Instanz ab, oder legt diese fest.
        /// </summary>
        float Gain { get; set; }

        /// <summary>
        /// Ruft die Abspielgeschwindigkeit der Instanz ab, oder legt diese fest.
        /// Ein Wert von 1.0f bedeuted die normale Geschwindigkeit.
        /// </summary>
        float Pitch { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob die Instanz sich beim Abspielen wiederholt, oder legt diesen fest.
        /// </summary>
        bool IsLooping { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie viele Buffer zum Streamen genutzt werden.
        /// Diese Eigenschaft ist nur verfügbar, wenn SoundFlags.Streamed in der Flag-Eigenschaft gesetzt ist.
        /// </summary>
        int StreamBufferCount { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie viele Buffer im Stream verarbeitet wurden.
        /// Diese Eigenschaft ist nur verfügbar, wenn SoundFlags.Streamed in der Flag-Eigenschaft gesetzt ist.
        /// </summary>
        int StreamBuffersProcessed { get; }

        /// <summary>
        /// Leitet das Signal der Instanz über den angegebenen Slot in den angegebenen MixerChannel um.
        /// </summary>
        /// <param name="slot">Der slot. Die maximale Anzahl an slots kann über AudioDevice.MaxRoutes abgefragt werden.</param>
        /// <param name="route">Das Ziel.</param>
        void Route(int slot, IMixerChannel route);

        /// <summary>
        /// Spielt den Sound ab.
        /// </summary>
        void Play();

        /// <summary>
        /// Pausiert den Sound. Er wird beim nächsten Play()-Aufruf an der aktuellen Position fortgesetzt.
        /// </summary>
        void Pause();

        /// <summary>
        /// Pausiert den Sound und setzt die Position auf 0 zurück.
        /// </summary>
        void Stop();
    }
}
