using DotGame.Assets;
using DotGame.EntitySystem;
using DotGame.EntitySystem.Components;
using DotGame.Geometry;
using DotGame.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Test
{
    public class SceneGraphTest : EngineComponent
    {
        Entity cube;

        public SceneGraphTest(Engine engine) : base(engine)
        { }

        public override void Init()
        {
            base.Init();

            cube = new Entity("cube", Engine);
            Material material = new Material(Engine.AssetManager, "TestMaterial");
            material.Texture = Engine.AssetManager.LoadTexture("CubeTexture", "GeneticaMortarlessBlocks.jpg");

            cube.AddComponent(new RotateComponent());

            MeshBuilder builder = new MeshBuilder(Engine);
            builder.PushCube(-Vector3.One, Vector3.One, Vector2.Zero, Vector2.One);

            StaticMesh mesh = builder.BuildMesh("cube");            
            cube.AddComponent(new StaticModel(mesh, material));
            var scene = ((Scene)Engine.Components.First(c => c is Scene));
            scene.Root.AddChild(cube);

            Entity cameraEntity = new Entity("camera", Engine);
            Camera camera = new Camera();
            cameraEntity.Transform.LocalPosition = new Vector3(0, 0, -5);
            camera.IsEnabled = true;
            camera.ClearColor = Graphics.Color.CornflowerBlue;
            cameraEntity.AddComponent(camera);
            scene.Root.AddChild(cameraEntity);

            Entity entity1 = Prefab.FromEntity(cube).CreateInstance();
            entity1.Transform.LocalPosition = new Vector3(-2, -1, 3);
            scene.Root.AddChild(entity1);

            scene.SerializeScene();
        }

        protected override void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }
    }
}
