using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt einen Effekt, der in einem <c>IMixerChannel</c> benutzt werden kann.
    /// </summary>
    public interface IEffect : IAudioObject, IEquatable<IEffect>
    {

    }
}
