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
using DotGame.Geometry;
using DotGame.EntitySystem;
using DotGame.Assets;
using DotGame.EntitySystem.Components;
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

            //engine.AddComponent(new GraphTest(engine));
            engine.AddComponent(new MipMapTest(engine));

            Application.Run();
        }
    }
}
