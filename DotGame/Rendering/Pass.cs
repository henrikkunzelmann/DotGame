using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Assets;
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

        /// <summary>
        /// Gibt den Standard-Shader für diesen Pass zurück.
        /// </summary>
        public Shader Shader { get; private set; }

        public Pass(Engine engine, Shader shader)
            : base(engine)
        {
            this.Shader = shader;
        }

        /// <summary>
        /// Wendet den Pass an.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Apply(GameTime gameTime);
    }
}
