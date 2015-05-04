using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.Graphics;
using System.Numerics;

namespace DotGame.Assets
{
    public class Material : Asset
    {
        private bool dirty = true;
        private Texture texture;


        public Texture Texture
        {
            get { return texture; }
            set
            {
                if (value != texture)
                {
                    texture = value;
                    dirty = true;
                }
            }
        }

        public Material(AssetManager manager, string name)
            : base(manager, name, null)
        {

        }

        protected override void Load()
        {
        }

        protected override void Unload()
        {
        }
    }
}
