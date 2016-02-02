﻿using DotGame.Assets;
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
        public SceneGraphTest(Engine engine) : base(engine)
        { }

        public override void Init()
        {
            base.Init();

            Entity entity = new Entity("hi", Engine);
            Material material = new Material(Engine, "TestMaterial");
            material.Texture = Engine.AssetManager.LoadTexture("CubeTexture", "CubeTextureDXT1MipMaps.dds");

            MeshBuilder builder = new MeshBuilder(Engine);
            builder.PushCube(-Vector3.One, Vector3.One, Vector2.Zero, Vector2.One);

            StaticMesh mesh = builder.BuildMesh("cube");
            entity.AddComponent(new StaticModel(mesh, material));
            var scene = ((Scene)Engine.Components.First(c => c is Scene));
            scene.Root.AddChild(entity);

            MeshBuilder skyBuilder = new MeshBuilder(Engine);
            skyBuilder.PushVertex(new Vector3(-10, 10, 10), new Vector2(1, 0), new Vector3(0,0, -1));
            skyBuilder.PushVertex(new Vector3(10, 10, 10), new Vector2(0, 0), new Vector3(0, 0, -1));
            skyBuilder.PushVertex(new Vector3(10, -10, 10), new Vector2(0, 1), new Vector3(0, 0, -1));
            skyBuilder.PushVertex(new Vector3(-10, -10, 10), new Vector2(1, 1), new Vector3(0, 0, -1));

            skyBuilder.PushVertex(new Vector3(-10, 10, -10), new Vector2(1, 0), new Vector3(0, 0, -1));
            skyBuilder.PushVertex(new Vector3(10, 10, -10), new Vector2(0, 0), new Vector3(0, 0, -1));
            skyBuilder.PushVertex(new Vector3(10, -10, -10), new Vector2(0, 1), new Vector3(0, 0, -1));
            skyBuilder.PushVertex(new Vector3(-10, -10, -10), new Vector2(1, 1), new Vector3(0, 0, -1));


            skyBuilder.PushIndex(0, 1, 2, 2, 3, 0,  4, 5, 6, 6, 7, 4,  1, 5, 6, 6, 2, 1,  0, 4, 7, 7, 3, 0,  0, 1, 5, 5, 4, 0,  3, 2, 6, 6, 7, 3);

            var skybox = skyBuilder.BuildMesh("skybox");
            var skyboxEntity = new Entity("skyboxEntity", Engine);
            Material sky = new Material(Engine, "SkyMaterial");
            sky.Texture = Engine.AssetManager.LoadTexture("SkyTexture", "skybox.dds");
            skyboxEntity.AddComponent(new StaticModel(skybox, sky));
            scene.Root.AddChild(skyboxEntity);
            

            Entity cameraEntity = new Entity("camera", Engine);
            Camera camera = new Camera();
            cameraEntity.Transform.LocalPosition = new Vector3(0, 0, -5);
            camera.IsEnabled = true;
            camera.ClearColor = Graphics.Color.CornflowerBlue;
            cameraEntity.AddComponent(camera);
            scene.Root.AddChild(cameraEntity);

            /*Entity entity1 = Prefab.FromEntity(entity).CreateInstance();
            entity1.Transform.LocalPosition = new Vector3(-2, -1, 3);
            scene.Root.AddChild(entity1);

            scene.SerializeScene();*/
        }

        protected override void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
        }
    }
}
