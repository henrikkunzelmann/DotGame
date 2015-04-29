using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt den Zuhörer im dreidimensionalen Raum dar.
    /// </summary>
    public interface IAudioListener : IEquatable<IAudioListener>
    {
        /// <summary>
        /// Ruft das IAudioDevice ab, das dieser IAudioFactory zugeordnet ist.
        /// </summary>
        IAudioDevice AudioDevice { get; }

        /// <summary>
        /// Ruft die globale Lautstärke ab, oder legt diese fest.
        /// </summary>
        float Gain { get; set; }

        /// <summary>
        /// Ruft die Position im Raum ab, oder legt diese fest.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        /// Ruft die Geschwindigkeit ab, oder legt diese fest.
        /// Dieser Wert wird u.a. für den Dopplereffekt genutzt.
        /// </summary>
        Vector3 Velocity { get; set; }

        /// <summary>
        /// Ruft den Up-Vektor ab, oder legt diesen fest.
        /// </summary>
        Vector3 Up { get; set; }

        /// <summary>
        /// Ruft den At-Vektor ab, oder legt diesen fest.
        /// </summary>
        Vector3 At { get; set; }

        /// <summary>
        /// Setzt den Up- und At-Vektor.
        /// </summary>
        /// <param name="Up">Der Up-Vektor.</param>
        /// <param name="At">Der At-Vektor.</param>
        void Orientation(Vector3 Up, Vector3 At);
    }
}
