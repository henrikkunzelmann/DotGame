namespace DotGame.Assets.Importers
{
    public abstract class ImporterBase : EngineObject
    {
        public AssetManager AssetManager { get; private set; }

        public ImporterBase(AssetManager assetManager)
            : base(assetManager.Engine)
        {
            this.AssetManager = assetManager;
        }
    }
}
