using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.Assets
{
    public class Material : Asset
    {
        private bool dirty = true;
        private Shader shader;
        private Texture texture;

        public Shader Shader
        {
            get { return shader; }
            set
            {
                if (value != shader)
                {
                    shader = value;
                    dirty = true;
                }
            }
        }


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

        public void Apply(Pass pass, IRenderContext context, Matrix world)
        {
            if (shader == null)
                pass.DefaultShader.Apply(pass, context, this, world);
            else
                shader.Apply(pass, context, this, world);
        }

        protected override void Load()
        {
        }

        protected override void Unload()
        {
        }
    }
}
