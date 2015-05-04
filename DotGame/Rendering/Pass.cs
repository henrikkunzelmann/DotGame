using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    /// <summary>
    /// Stellt einen Pass dar der Teil der PassPipeline ist. 
    /// </summary>
    public abstract class Pass : EngineObject
    {
        /// <summary>
        /// Gibt das IGraphicsDevice zurück welches die Engine benutzt.
        /// </summary>
        public IGraphicsDevice GraphicsDevice 
        {
            get 
            { 
                return Engine.GraphicsDevice; 
            } 
        }

        public Pass(Engine engine)
            : base(engine)
        {
        }

        /// <summary>
        /// Wendet den Pass an.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Apply(GameTime gameTime);
    }
}
