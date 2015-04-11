using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Assets
{
    public class Texture : Asset
    {
        private ITexture2D handle;

        public Texture(Engine engine, string name, ITexture2D handle)
            : base(engine, name)
        {
            this.handle = handle;
        }

        public ITexture2D GetHandle()
        {
            return handle;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (handle != null)
                handle.Dispose();
        }
    }
}
