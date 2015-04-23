using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using DotGame.Assets.Importers;

namespace DotGame.Assets
{
    public class Texture : Asset
    {
        private TextureLoadSettings loadSettings;
        private TextureImporterBase importer;

        private TextureHeader header;
        private ITexture2D handle;

        public ITexture2D Handle { get { MarkForUsage(); return handle; } }
        public TextureFormat Format { get { return header.Format; } }
        public int Width { get { return header.Width; } }
        public int Height { get { return header.Height; } }


        internal Texture(AssetManager manager, string name, string file, TextureLoadSettings loadSettings, TextureImporterBase importer)
            : base(manager, name, file)
        {
            if (importer == null)
                throw new ArgumentNullException("importer");

            this.loadSettings = loadSettings;
            this.importer = importer;

            this.header = importer.LoadHeader(file, loadSettings);
        }

        internal Texture(AssetManager manager, string name, ITexture2D handle)
            : base(manager, name, null)
        {
            if (handle == null)
                throw new ArgumentNullException("handle");

            this.handle = handle;
            this.header = new TextureHeader()
            {
                Format = handle.Format,
                Width = handle.Width,
                Height = handle.Height
            };
        }

        protected override void Load()
        {
            if (handle != null)
                throw new InvalidOperationException("Texture is already loaded.");

            handle = Engine.GraphicsDevice.Factory.CreateTexture2D(header.Width, header.Height, header.Format, loadSettings.GenerateMipMaps);
            importer.LoadData(handle, File, loadSettings);
        }

        protected override void Unload()
        {
            if (handle != null)
                handle.Dispose();
        }
    }
}