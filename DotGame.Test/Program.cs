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
            

            engine.AddComponent(new GraphTest(engine));

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
