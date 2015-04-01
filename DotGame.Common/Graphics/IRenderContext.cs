using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public interface IRenderContext : IGraphicsObject
    {
        void SetShader(IShader shader);
        void SetPrimitiveType(PrimitiveType type);

        void SetState(IRenderState state);
        void SetVertexBuffer(IVertexBuffer vertexBuffer);
        void SetIndexBuffer(IIndexBuffer indexBuffer);
        void Draw();
        void DrawIndexed();
    }
}
