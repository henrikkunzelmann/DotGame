using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public struct GraphicsCapabilities
    {
        /// <summary>
        /// Gibt an ob die API binäre Shader (von Bytecode) laden kann.
        /// </summary>
        public bool SupportsBinaryShaders;
    }
}
