using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt einen Effekt, der in einem <c>IMixerChannel</c> benutzt werden kann.
    /// </summary>
    public interface IEffect : IAudioObject, IEquatable<IEffect>
    {

    }
}
