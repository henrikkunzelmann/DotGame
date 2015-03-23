using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Ruft den Mischungsanteil zwischen dem rohen Signal (dry) und dem, vom Effekt bearbeiteten, Signal (wet) ab, oder legt diesen fest.
        /// </summary>
        float WetGain { get; set; }
    }
}
