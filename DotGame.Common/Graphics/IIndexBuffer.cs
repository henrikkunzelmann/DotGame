namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt einen IndexBuffer dar.
    /// </summary>
    public interface IIndexBuffer : IGraphicsObject
    {
        /// <summary>
        /// Gibt das Format der Indices in diesem IndexBuffer an.
        /// </summary>
        IndexFormat Format { get; }

        /// <summary>
        /// Gibt die Anzahl der Indices in diesem IndexBuffer an.
        /// </summary>
        int IndexCount { get; }

        /// <summary>
        /// Gibt die Größe des IndexBuffers in Bytes an.
        /// </summary>
        int SizeBytes { get; }

        /// <summary>
        /// Gibt an wie der IndexBuffer benutzt wird.
        /// </summary>
        ResourceUsage Usage { get; }
    }
}
