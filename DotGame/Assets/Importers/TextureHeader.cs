using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Assets.Importers
{
    public struct TextureHeader
    {
        public TextureFormat Format;
        public int Width;
        public int Height;
        public int Depth;
        public int MipLevels;
    }
}
