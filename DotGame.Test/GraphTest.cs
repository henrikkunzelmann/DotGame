using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DotGame;
using DotGame.Utils;
using DotGame.SceneGraph;
using DotGame.Rendering;
using DotGame.Assets;
using DotGame.Geometry;


namespace DotGame.Test
{
    public class GraphTest : GameComponent
    {
        public Scene Scene { get; private set; }
        public PassPipeline Pipeline { get; private set; }

        public MeshEntity Mesh { get; private set; }

        public GraphTest(Engine engine)
            : base(engine)
        {

        }

        public override void Init()
        {
            Scene = new Scene(Engine);
            Pipeline = new PassPipeline(Engine, new TestPass(Engine, Scene));

            FixedCamera cam = new FixedCamera();
            cam.Position = new Vector3(0, 0, 5);
            cam.LookAt = Vector3.Zero;

            Scene.Camera = cam;

            Material material = new Material(Engine.AssetManager, "TestMaterial");
            material.Texture = Engine.AssetManager.LoadTexture("CubeTexture", "GeneticaMortarlessBlocks.jpg");

            Mesh mesh = Engine.AssetManager.LoadMesh("cube", new VertexPositionTexture[] 
            {
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,  1.0f, -1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, -1.0f, -1.0f),   new Vector2(1.0f, 1.0f)),

                new VertexPositionTexture(new Vector3(-1.0f, -1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,  1.0f,  1.0f),   new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, -1.0f,  1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),

                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f,  1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, 1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, 1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, 1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, 1.0f, -1.0f),   new Vector2(1.0f, 1.0f)),

                new VertexPositionTexture(new Vector3(-1.0f,-1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,-1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,-1.0f,  1.0f),   new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,-1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,-1.0f, -1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,-1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),

                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f,  1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,  1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f, -1.0f, -1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,  1.0f,  1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-1.0f,  1.0f, -1.0f),   new Vector2(1.0f, 1.0f)),

                new VertexPositionTexture(new Vector3( 1.0f, -1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, -1.0f,  1.0f),   new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3( 1.0f, -1.0f, -1.0f),   new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f, -1.0f),   new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3( 1.0f,  1.0f,  1.0f),   new Vector2(0.0f, 1.0f)),
            });

            Mesh = new MeshEntity(Scene, "cube", mesh, material);
            Scene.Root.AddChild(Mesh);
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalTime.TotalMilliseconds / 1000f;
            Mesh.Rotation = Quaternion.CreateFromYawPitchRoll(t * .1f, t * .3f, t * .4f);
        }

        public override void Draw(GameTime gameTime)
        {
            Pipeline.Draw(gameTime);
        }

        public override void Unload()
        {
        }
    }
}
