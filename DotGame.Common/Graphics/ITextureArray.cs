namespace DotGame.Graphics
{
    public interface ITextureArray : IGraphicsObject
    {
        /// <summary>
        /// Die Anzahl der Texturen in diesem TextureArray.
        /// </summary>
        int ArraySize { get; }
    }
}
