using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt einen Sound dar, der beim Erstellen komplett geladen wird.
    /// </summary>
    public interface ISound : IAudioObject
    {
        bool Supports3D { get; }

        ISoundInstance CreateInstance(bool isPaused);
    }
}
