using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame
{
    public abstract class GameComponent
    {
        public Engine Engine { get; private set; }
        public IGraphicsDevice GraphicsDevice { get { return Engine.GraphicsDevice; } }
       
        public GameComponent(Engine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            this.Engine = engine;
        }

        public abstract void Init();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public abstract void Unload();
    }
}
