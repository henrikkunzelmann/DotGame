namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt einen VertexBuffer dar.
    /// </summary>
    public interface IVertexBuffer : IGraphicsObject
    {
        VertexDescription Description { get; }

        /// <summary>
        /// Die Anzahl aller Vertices in diesem VertexBuffer.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// Die Größe des VertexBuffers in Bytes.
        /// </summary>
        int SizeBytes { get; }

        /// <summary>
        /// Gibt an wie der VertexBuffer benutzt wird.
        /// </summary>
        ResourceUsage Usage { get; }
    }
}
