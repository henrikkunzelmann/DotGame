using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering
{
    public struct MaterialDescription
    {
        public bool HasDiffuseTexture { get; set; }
        public bool HasColor { get; set; }
        public bool HasNormalMap { get; set; }
        public bool HasSpecularMap { get; set; }
    }
}
