namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt den aktuellen Zustand der Pipeline dar. Die States können über den IRenderContext ausgewechselt werden.
    /// </summary>
    public interface IRenderState : IGraphicsObject
    {
        /// <summary>
        /// Gibt die Informationen welche dieser Zustand enthält an. 
        /// </summary>
        RenderStateInfo Info { get; }
    }
}
