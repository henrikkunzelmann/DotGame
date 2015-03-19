using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt Methoden zum Erstellen von Ressourcen bereit.
    /// </summary>
    public interface IAudioFactory
    {
        ISound CreateSound(string file);
        ISampleSource CreateSampleSource(string file);
        IMixerChannel CreateMixerChannel(string name);
    }
}
