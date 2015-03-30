using System;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt ein Grafikobjekt dar.
    /// </summary>
    public interface IGraphicsObject : IDisposable
    {
        /// <summary>
        /// Ruft das IGraphicsDevice ab, das diesem IGraphicsObject zugeordnet ist.
        /// </summary>
        IGraphicsDevice GraphicsDevice { get; }
        
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Tritt auf, wenn "Dispose" aufgerufen oder das Objekt finalisiert und vom Garbage Collector der Microsoft .NET Common Language Runtime bereinigt wird.
        /// </summary>
        event EventHandler<EventArgs> OnDisposing;

        /// <summary>
        /// Tritt auf, wenn "Dispose" aufgerufen wurde.
        /// </summary>
        event EventHandler<EventArgs> OnDisposed;

        /// <summary>
        /// Ruft die Ressourcen-Tags für diese Ressource ab.
        /// </summary>
        object Tag { get; set; }
    }
}
