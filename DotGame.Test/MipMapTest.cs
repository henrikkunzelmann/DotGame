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
    public class MipMapTest : GameComponent
    {
        public Scene Scene { get; private set; }
        public PassPipeline Pipeline { get; private set; }

        public MeshEntity Mesh { get; private set; }

        public MipMapTest(Engine engine)
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
            material.Texture = Engine.AssetManager.LoadTexture("CubeTexture", "GeneticaMortarlessBlocksMipped.dds");
            material.Texture = Engine.AssetManager.LoadTexture("CubeTexture", "CubeTextureDXT1MipMaps.dds");

            MeshBuilder builder = new MeshBuilder(Engine);
            builder.PushCube(-Vector3.One, Vector3.One, Vector2.Zero, Vector2.One);

            Mesh mesh = builder.BuildMesh("cube");

            Mesh = new MeshEntity(Scene, "cube", mesh, material);
            Scene.Root.AddChild(Mesh);
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalTime.TotalMilliseconds / 1000f;
            Mesh.Position = new Vector3(Mesh.Position.X, Mesh.Position.Y, (float)Math.Sin(gameTime.TotalTime.TotalMilliseconds * 0.001f) * 10 - 7);
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
