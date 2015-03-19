using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public struct EngineSettings
    {
        public GraphicsAPI GraphicsAPI { get; set; }
        public AudioAPI AudioAPI { get; set; }
        public string AudioDevice { get; set; }
    }
}
