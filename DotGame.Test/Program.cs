using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using DotGame;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.SceneGraph;
using DotGame.EntitySystem;
using DotGame.EntitySystem.Components;
using DotGame.Geometry;
using DotGame.Assets;
using DotGame.Graphics;

namespace DotGame.Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var engine = new Engine(new EngineSettings()
            {
                GraphicsAPI = GraphicsAPI.Direct3D11,
                Width = 1280,
                Height = 720,
                AudioAPI = AudioAPI.OpenAL
            });

            var component = new EntitySystemComponent(engine);
            engine.AddComponent(component);

            var mesh = engine.AssetManager.LoadMesh("Cube", new VertexPositionTexture[] 
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

            var material = new Material(engine.AssetManager, "TestMaterial");
            material.Texture = engine.AssetManager.LoadTexture("CubeTexture", "GeneticaMortarlessBlocks.jpg");

            Transform last = null;
            for (int i = 0; i < 333; i++)
            {
                var entityCube = component.Scene.CreateChild("Cube" + i);
                entityCube.AddComponent<MeshRenderer>().Material = material;
                entityCube.GetComponent<MeshInstance>().Mesh = mesh;
                entityCube.AddComponent<RotateComponent>();
                entityCube.Transform.LocalPosition = new Vector3(1, 0, 0);

                if (last != null)
                    entityCube.Transform.Parent = last;
                last = entityCube.Transform;
            }

            var entityCamera = component.Scene.CreateChild("Camera");
            var camera = entityCamera.AddComponent<Camera>();
            camera.IsEnabled = true;
            camera.ClearColor = Color.SkyBlue;
            entityCamera.Transform.LocalPosition = new Vector3(0, 0, -32);
            //entityCamera.Transform.Parent = entityCube.Transform;

            float deg = 60;
            float rad = deg / 180f * MathHelper.PI;

            Console.WriteLine("deg:" + deg);
            Console.WriteLine("rad: " + rad);
            Console.WriteLine("rad2: " + MathHelper.DegressToRadians(deg));
            Console.WriteLine("volvo pls fix?");

            Application.Run();
        }
    }
}
