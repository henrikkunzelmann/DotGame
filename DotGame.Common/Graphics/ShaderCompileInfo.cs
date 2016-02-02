using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Speichert Informationen über das compilieren von Shadern.
    /// </summary>
    public struct ShaderCompileInfo
    {
        /// <summary>
        /// Gibt den Quellcode des Shaders an oder setzt diesen.
        /// </summary>
        public string SouceCode;

        /// <summary>
        /// Gibt die Funktion im Quellcode des Shaders an oder setzt diese.
        /// </summary>
        public string Function;

        /// <summary>
        /// Gibt die Version des Shaders an oder setzt diese. Wenn der Quellcode selbst die Version angibt, dann wird dieser Wert nicht benutzt.
        /// </summary>
        public string Version;

        public ShaderCompileInfo(string sourceCode, string function, string version)
        {
            this.SouceCode = sourceCode;
            this.Function = function;
            this.Version = version;
        }
    }
}
