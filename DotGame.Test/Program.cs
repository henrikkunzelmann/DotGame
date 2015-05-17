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
using DotGame.EntitySystem;
using DotGame.EntitySystem.Components;
using DotGame.Geometry;
using DotGame.Assets;
using DotGame.Graphics;
using DotGame.Audio;

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
            Entity first = null;
            for (int i = 0; i < 200; i++)
            {
                var entityCube = component.Scene.CreateChild("Cube" + i);
                entityCube.AddComponent<MeshRenderer>().Material = material;
                entityCube.GetComponent<MeshInstance>().Mesh = mesh;
                if (i == 0)
                    entityCube.AddComponent<RotateComponent>();
                entityCube.Transform.LocalPosition = new Vector3(1, 0, 0);

                if (last != null)
                    entityCube.Transform.Parent = last;
                last = entityCube.Transform;
                if (first == null)
                    first = entityCube;
            }

            var channel = engine.AudioDevice.Factory.CreateMixerChannel("Reverb");
            var reverb = engine.AudioDevice.Factory.CreateReverb();
            reverb.Diffusion = 1.0f;
            reverb.DecayTime = 4;
            reverb.GainDamp = 0.95f;
            reverb.RoomRolloffFactor = 0;
            reverb.Gain = 0.05f;
            channel.Effect = reverb;
            
            var source = last.Entity.AddComponent<AudioSource>();
            source.Sound = engine.AudioDevice.Factory.CreateSound("test.ogg", SoundFlags.Support3D).CreateInstance();
            source.Sound.IsLooping = true;
            engine.AudioDevice.Listener.Gain = 0.1f;
            source.Sound.Play();

            source.Sound.Route(0, channel);

            var entityCamera = component.Scene.CreateChild("Camera");
            var camera = entityCamera.AddComponent<Camera>();
            camera.IsEnabled = true;
            camera.ClearColor = Color.SkyBlue;
            entityCamera.Transform.LocalPosition = new Vector3(0, 0, -204);
            entityCamera.AddComponent<AudioListener>();

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
