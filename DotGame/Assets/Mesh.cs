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

        private Mesh(Engine engine, string name, IVertexBuffer vertexBuffer)
            : base(engine, name)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");

            this.vertexBuffer = vertexBuffer;
        }

        public static Mesh Create<T>(Engine engine, string name, T[] vertices) where T : struct, IVertexType
        {
            if (engine == null)
                throw new ArgumentNullException("engine");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be not null, empty or white-space.", "name");
            if (vertices == null)
                throw new ArgumentNullException("vertices");
            if (vertices.Length == 0)
                throw new ArgumentException("Vertices must not be empty.", "vertices");

            return new Mesh(engine, name, engine.GraphicsDevice.Factory.CreateVertexBuffer(vertices, vertices[0].VertexDescription, BufferUsage.Static));
        }

        public void Draw(IRenderContext context)
        {
            context.SetVertexBuffer(vertexBuffer);
            context.Draw();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();
        }
    }
}
