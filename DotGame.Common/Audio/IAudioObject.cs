using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Stellt ein Audioobjekt dar.
    /// </summary>
    public interface IAudioObject : IDisposable
    {
        /// <summary>
        /// Ruft das IAudioDevice ab, das diesem IAudioObject zugeordnet ist.
        /// </summary>
        IAudioDevice AudioDevice { get; }

        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Tritt auf, wenn "Dispose" aufgerufen oder das Objekt finalisiert und vom Garbage Collector der Microsoft .NET Common Language Runtime bereinigt wird.
        /// </summary>
        EventHandler<EventArgs> Disposing { get; set; }

        /// <summary>
        /// Ruft die Ressourcen-Tags für diese Ressource ab.
        /// </summary>
        object Tag { get; set; }
    }
}
