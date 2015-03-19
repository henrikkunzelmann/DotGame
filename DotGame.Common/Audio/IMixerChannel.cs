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
    public interface IMixerChannel : IAudioObject
    {
        float Gain { get; set; }
    }
}
