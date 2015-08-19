using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public struct EngineSettings
    {
        public GraphicsAPI GraphicsAPI;
        public AudioAPI AudioAPI;
        public string AudioDevice;
        public int Width;
        public int Height;
        public bool Debug;
    }
}
