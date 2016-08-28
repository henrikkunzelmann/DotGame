using DotGame.Graphics;
using System;

namespace DotGame.Assets
{
    public abstract class Mesh : Asset
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

        internal Mesh(AssetManager manager, string name, AssetType type, IVertexBuffer vertexBuffer)
            : base(manager, type, name, null)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");

            this.vertexBuffer = vertexBuffer;

            this.HasIndices = false;
            this.VertexCount = vertexBuffer.VertexCount;
            this.TriangleCount = vertexBuffer.VertexCount / 3;
            this.IndexCount = 0;
        }

        internal Mesh(AssetManager manager, string name, AssetType type, IVertexBuffer vertexBuffer, IIndexBuffer indexBuffer)
            : base(manager, type, name, null)
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

        public abstract void Draw(IRenderContext context);

        protected override void Unload()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();
            if (indexBuffer != null)
                indexBuffer.Dispose();
        }
    }
}
