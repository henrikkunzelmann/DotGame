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
        public IVertexBuffer VertexBufferHandle 
        { 
            get 
            { 
                MarkForUsage(); 
                return vertexBuffer; 
            } 
        }

        private IIndexBuffer indexBuffer;
        public IIndexBuffer IndexBufferHandle
        {
            get
            {
                MarkForUsage();
                return indexBuffer;
            }
        }

        public bool HasIndices { get; private set; }
        public int VertexCount { get; private set; }
        public int TriangleCount { get; private set; }
        public int IndexCount { get; private set; }


        internal Mesh(AssetManager manager, string name, IVertexBuffer vertexBuffer)
            : base(manager, AssetType.Wrapper, name, null)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");

            this.vertexBuffer = vertexBuffer;

            this.HasIndices = false;
            this.VertexCount = vertexBuffer.VertexCount;
            this.TriangleCount = vertexBuffer.VertexCount / 3;
            this.IndexCount = 0;
        }

        internal Mesh(AssetManager manager, string name, IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer)
            : base(manager, AssetType.Wrapper, name, null)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");

            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;

            this.HasIndices = true;
            this.VertexCount = vertexBuffer.VertexCount;
            this.TriangleCount = indexBuffer.IndexCount / 3;
            this.IndexCount = indexBuffer.IndexCount;
        }

        public void Draw(IRenderContext context)
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

        protected override void Unload()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();
            if (indexBuffer != null)
                indexBuffer.Dispose();
        }
    }
}
