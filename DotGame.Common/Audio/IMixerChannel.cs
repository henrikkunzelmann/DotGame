using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt einen Channel dar, in dem Effekte auf ein eingehendes Audiosignal angewendet werden können.
    /// </summary>
    public interface IMixerChannel : IAudioObject, IEquatable<IMixerChannel>
    {
        /// <summary>
        /// Der Effekt, der diesem Channel zugeordnet wird.
        /// </summary>
        IEffect Effect { get; set; }

        /// <summary>
        /// Ruft die Lautstärke des bearbeiteten Signals (wet) ab, oder legt diese fest.
        /// </summary>
        float WetGain { get; set; }
    }
}
