using DotGame.Assets.Importers;
using DotGame.Graphics;
using System;

namespace DotGame.Assets
{
    public class Texture : Asset
    {
        private TextureLoadSettings loadSettings;
        private TextureImporterBase importer;

        private TextureHeader header;
        private ITexture2D handle;

        public TextureFormat Format { get { return header.Format; } }
        public int Width { get { return header.Width; } }
        public int Height { get { return header.Height; } }

        public ITexture2D Handle
        {
            get
            {
                MarkForUsage();
                return handle;
            }
        }

        internal Texture(AssetManager manager, string name, string file, TextureLoadSettings loadSettings, TextureImporterBase importer)
            : base(manager, AssetType.File, name, file)
        {
            if (importer == null)
                throw new ArgumentNullException("importer");

            this.loadSettings = loadSettings;
            this.importer = importer;

            this.header = importer.LoadHeader(file, loadSettings);
        }

        internal Texture(AssetManager manager, string name, ITexture2D handle)
            : base(manager, AssetType.Wrapper, name, null)
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

            if(header.MipLevels > 0)
                handle = Engine.GraphicsDevice.Factory.CreateTexture2D(header.Width, header.Height, header.Format, header.MipLevels);
            else
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