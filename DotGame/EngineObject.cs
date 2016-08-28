using Newtonsoft.Json;
using System;

namespace DotGame
{
    public abstract class EngineObject : IDisposable
    {
        [JsonIgnore]
        public Engine Engine { get; private set; }
        [JsonIgnore]
        public object Tag { get; set; }

        public EngineObject(Engine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            this.Engine = engine;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected abstract void Dispose(bool isDisposing);
    }
}
