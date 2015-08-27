using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Assets
{
    public class StaticMesh : Mesh
    {
        public StaticMesh(AssetManager manager, string name, AssetType type, IVertexBuffer vertexBuffer) : base(manager, name, type, vertexBuffer)
        { }
        public StaticMesh(AssetManager manager, string name, AssetType type, IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer) : base(manager, name, type, vertexBuffer, indexBuffer)
        { }

        public override void Draw(IRenderContext context)
        {
            IVertexBuffer vertexBuffer = VertexBufferHandle;
            IIndexBuffer indexBuffer = IndexBufferHandle;
            if (vertexBuffer == null || (HasIndices && indexBuffer == null)) // Buffer noch nicht geladen, nichts zeichnen
                return;

            context.SetVertexBuffer(vertexBuffer);
            if (HasIndices)
            {
                context.SetIndexBuffer(indexBuffer);
                context.DrawIndexed();
            }
            else
                context.Draw();
        }

        protected override void Load()
        {
        }
    }
}
