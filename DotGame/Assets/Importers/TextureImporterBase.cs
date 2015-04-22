using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Assets.Importers
{
    public abstract class TextureImporterBase : ImporterBase
    {
        public TextureImporterBase(AssetManager assetManager)
            : base(assetManager)
        {

        }

        public abstract TextureHeader LoadHeader(string file, TextureLoadSettings settings);
        public abstract void LoadData(ITexture2D handle, string file, TextureLoadSettings settings);
    }
}
