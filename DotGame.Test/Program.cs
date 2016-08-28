using DotGame.Rendering;
using System;
using System.Windows.Forms;

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
                AudioAPI = AudioAPI.OpenAL,
                Debug = true
            });

            var scene = new Scene(engine);
            engine.AddComponent(scene);

            engine.AddComponent(new SceneGraphTest(engine));


            var renderer = PassPipeline.CreateDefault(engine, scene);
            engine.AddComponent(renderer);

            //engine.AddComponent(new GraphTest(engine));
            //engine.AddComponent(new MipMapTest(engine));

            Application.Run();
        }
    }
}
