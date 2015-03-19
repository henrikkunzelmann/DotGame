using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt das AudioDevice dar.
    /// </summary>
    public interface IAudioDevice : IDisposable
    {
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Ruft die IAudioFactory ab, die zur Erzeugung neuer AudioObjekte benutzt wird.
        /// </summary>
        IAudioFactory Factory { get; }

        Version ApiVersion { get; }

        /// <summary>
        /// Ruft den IMixerChannel ab, in den alle ISoundInstance-Instanzen standardmäßig geroutet werden.
        /// </summary>
        IMixerChannel MasterChannel { get; }

        int MaxRoutes { get; }
    }
}
