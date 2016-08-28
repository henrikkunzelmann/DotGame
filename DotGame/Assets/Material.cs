using DotGame.Rendering;

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
            : base(manager, AssetType.User, name, null)
        {

        }

        protected override void Load()
        {
        }

        protected override void Unload()
        {
        }

        public MaterialDescription CreateDescription()
        {
            var info = new MaterialDescription();
            info.HasDiffuseTexture = true;
            return info;
        }
    }
}
