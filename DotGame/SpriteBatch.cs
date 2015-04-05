using DotGame.Geometry;
using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public enum SpriteSortMode
    {
        FrontToBack,
        BackToFront,
        Texture,
        Depth
    }

    public class SpriteBatch
    {
        private class BatchGroup
        {
            public ITexture2D Texture;
            public int IndexStart;
            public int IndexCount;

            public BatchGroup(ITexture2D Texture)
            {
                this.Texture = Texture;
            }
        }

        private struct Quad
        {
            public ITexture2D Texture;
            public IVertexType A, B, C, D;

            public Quad(ITexture2D Texture, IVertexType A, IVertexType B, IVertexType C, IVertexType D)
            {
                this.Texture = Texture;
                this.A = A;
                this.B = B;
                this.C = C;
                this.D = D;
            }
        }

        public IGraphicsDevice GraphicsDevice { get; private set; }
        
        private IVertexBuffer vertexBuffer;
        private IIndexBuffer indexBuffer;

        private List<BatchGroup> batches;
        private List<Quad> quads;

        private List<IVertexType> vertices;
        private List<int> indices;

        private IShader shader;
        private Matrix mvp;
        private SpriteSortMode sortMode;

        public SpriteBatch(IGraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;

            this.batches = new List<BatchGroup>();
            this.quads = new List<Quad>();
            this.vertices = new List<IVertexType>();
            this.indices = new List<int>();
        }

        public void Begin(IShader shader, Matrix world, Matrix projection, SpriteSortMode sortMode = SpriteSortMode.BackToFront)
        {
            this.shader = shader;
            this.mvp = world * projection;
            this.sortMode = sortMode;
        }

        public void End()
        {
            // Batches erstellen und Sortieren
            for (int i = 0; i < batches.Count; i++)
            { 
                //TODO: (tice) Batches erstellen und Sortieren
            }

            // Update Buffers
            //if (vertexBuffer == null)
            //    vertexBuffer = GraphicsDevice.Factory.CreateVertexBuffer(vertices.ToArray(), VertexPositionTextureColor.VertexDescription);
            //TODO: (tice) Vertex- und IndexBuffer neu befüllen

            // Setup State
            GraphicsDevice.RenderContext.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.RenderContext.SetIndexBuffer(indexBuffer);

            GraphicsDevice.RenderContext.SetShader(shader);
            //TODO: (tice) shader variable setzen: MVP

            // Draw Batches
            foreach (var batch in batches)
            {
                //TODO: (tice) shader variable setzen: Texture
                //TODO: (tice) GraphicsDevice.RenderContext.DrawIndexed(batch.IndexStart, batch.IndexCount);
            }

            // Clear
            quads.Clear();
            vertices.Clear();
            indices.Clear();
            batches.Clear();
        }

        private void AddQuad(Quad Quad)
        {
            quads.Add(Quad);
        }

        #region Draw
        public void Draw(ITexture2D Texture, Rectangle Rectangle, Color Color)
        {
            Draw(Texture, Rectangle, new Rectangle(0, 0, Texture.Width, Texture.Height), Color);
        }

        public void Draw(ITexture2D Texture, Rectangle Rectangle, Rectangle RectangleSource, Color Color)
        {
            Vector2 uvMin = new Vector2(RectangleSource.Left / Texture.Width, RectangleSource.Top / Texture.Height);
            Vector2 uvMax = new Vector2(RectangleSource.Right / Texture.Width, RectangleSource.Bottom / Texture.Height);

            var a = new VertexPositionTextureColor(new Vector3(Rectangle.X, Rectangle.Y, 0), uvMin, Color);
            var b = new VertexPositionTextureColor(new Vector3(Rectangle.Right, Rectangle.Y, 0), new Vector2(uvMax.X, uvMin.Y), Color);
            var c = new VertexPositionTextureColor(new Vector3(Rectangle.Right, Rectangle.Bottom, 0), uvMax, Color);
            var d = new VertexPositionTextureColor(new Vector3(Rectangle.X, Rectangle.Bottom, 0), new Vector2(uvMin.X, uvMax.Y), Color);

            AddQuad(new Quad(Texture, a, b, c, d));
        }
        //TODO: (tice) mehr Draw() overloads
        #endregion
    }
}
