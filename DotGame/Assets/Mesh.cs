using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Assets
{
    public class Mesh : Asset
    {
        private IVertexBuffer vertexBuffer;

        internal Mesh(AssetManager manager, string name, IVertexBuffer vertexBuffer)
            : base(manager, name, null)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");

            this.vertexBuffer = vertexBuffer;
        }

        internal IVertexBuffer GetVertexHandle()
        {
            MarkForUsage();
            return vertexBuffer;
        }

        public void Draw(IRenderContext context)
        {
            IVertexBuffer vertexBuffer = GetVertexHandle();
            if (vertexBuffer == null) // VertexBuffer noch nicht geladen, nichts zeichnen
                return;

            context.SetVertexBuffer(vertexBuffer);
            context.Draw();
        }

        protected override void Load()
        {
        }

        protected override void Unload()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();
        }
    }
}
