using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public struct ShaderCompileInfo
    {
        public string File;
        public string Function;
        public string Version;

        public ShaderCompileInfo(string file, string function, string version)
        {
            this.File = file;
            this.Function = function;
            this.Version = version;
        }
    }
}
