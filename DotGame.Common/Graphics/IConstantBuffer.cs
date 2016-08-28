namespace DotGame.Graphics
{
    public interface IConstantBuffer : IGraphicsObject
    {
        /// <summary>
        /// Die Größe des ConstantBuffers in Bytes.
        /// </summary>
        int SizeBytes { get; }

        /// <summary>
        /// Gibt an wie der ConstantBuffer benutzt wird.
        /// </summary>
        ResourceUsage Usage { get; }
    }
}
