using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
