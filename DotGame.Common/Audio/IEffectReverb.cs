using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    public interface IEffectReverb : IEffect
    {
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob der hochfrequentige Anteil des Eingangssignals durch die AirAbsorptionGain-Eigenschaft beeinflusst werden, oder ruft diesen ab. Standardwert: True.
        /// </summary>
        bool EnableAirAbsorption { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie stark der hochfrequentige Anteil des Eingangssignals auf Distanz abgeschwächt werden soll (lineare Abschwächung pro Meter), oder legt diesen Fest. Werte außerhalb [0.892f; 1.0f] sind ungültig. Standardwert: 0.994f.
        /// </summary>
        float AirAbsorptionGain { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, in welchem Verhältnis der hochfrequentigen Anteil zum tieffrequenten Anteil des Eingangssignals ausblendet. Richtwert für die Dauer ist die DecayTime-Eigenschaft. Werte außerhalb [0.1f; 2.0f] sind ungültig. Standardwert: 0.83f.
        /// </summary>
        float DecayFrequencyRatio { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie lange der Hall bestehen soll, oder legt diesen fest. Werte außerhalb [0.1f; 20.0f] sind ungültig. Standardwert: 1.49f.
        /// </summary>
        float DecayTime { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie Dicht der späte Hall sein soll, oder legt diesen fest. Niedrigere Werte sorgen für mehr Wärme. Werte außerhalb [0.0f; 1.0f] sind ungültig. Standardwert: 1.0f.
        /// </summary>
        float Density { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, wie dicht die einzelnen Echos im Hall beeinander liegen, oder legt diesen fest. Niedrigere Werten sorgen für eine stärkere Körnung. Werte außerhalb [0.0f; 1.0f] sind ungültig. Standardwert: 1.0f.
        /// </summary>
        float Diffusion { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der die Verstärkung des Halls angibt (1.0f = 0dB; 0.0f = -100dB), oder legt diesen fest. Werte außerhalb [0.0f; 1.0f] sind ungültig. Standardwert: 0.32f.
        /// </summary>
        float Gain { get; set; }

        /// <summary>
        /// Ruft einen Wert ab, der die Abschwächung des hochfrequenten Anteils des Halles angibt (1.0f = 0dB; 0.0f = -100dB), oder legt diesen fest. Werte außerhalb [0.0f; 1.0f] sind ungültig. Standardwert: 0.89f.
        /// </summary>
        float GainDamp { get; set; }

        float LateReverbDelay { get; set; }

        float LateReverbGain { get; set; }

        float ReflectionsDelay { get; set; }

        float ReflectionsGain { get; set; }

        float RoomRolloffFactor { get; set; }
    }
}
