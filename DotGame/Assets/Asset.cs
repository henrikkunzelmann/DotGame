using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets
{
    public abstract class Asset : EngineObject
    {
        public string Name { get; private set; }

        public Asset(Engine engine, string name)
            : base(engine)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be null, empty or white-space.", "name");

            this.Name = name;
        }
    }
}
