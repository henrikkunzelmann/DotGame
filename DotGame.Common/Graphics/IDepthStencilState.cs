namespace DotGame.Graphics
{
    public interface IDepthStencilState : IGraphicsObject
    {
        DepthStencilStateInfo Info { get; }
    }
}
