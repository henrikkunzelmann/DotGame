using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GraphicsAPI graphicsApi = GraphicsAPI.OpenGL4;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                graphicsApi = GraphicsAPI.DirectX11;

            var engine = new Engine(new EngineSettings()
            {
                GraphicsAPI = graphicsApi,
                AudioAPI = AudioAPI.OpenAL,
                Width = 800,
                Height = 450,
            }, null);

            Application.Run(new Form1(engine));
        }
    }
}
