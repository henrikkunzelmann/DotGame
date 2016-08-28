using DotGame.Graphics;
using Newtonsoft.Json;

namespace DotGame
{
    public abstract class EngineComponent : EngineObject
    {
        [JsonIgnore]
        public IGraphicsDevice GraphicsDevice { get { return Engine.GraphicsDevice; } }
       
        public EngineComponent(Engine engine) : base(engine)
        {
        }

        public virtual void Init() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void Unload()
        {
            Dispose(true);
        }
    }
}
