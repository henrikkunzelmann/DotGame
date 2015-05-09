using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt die komplette Welt dar, d.h. SceneNodes und Camera.
    /// </summary>
    public class Scene : EngineObject
    {
        /// <summary>
        /// Gibt die Root-SceneNode an.
        /// </summary>
        public SceneNode Root { get; private set; }

        public ICamera Camera { get; set; }

        public Scene(Engine engine)
            : base(engine)
        {
            Camera = new FixedCamera();
            Root = new SceneNode(this, "root");
        }

        protected override void Dispose(bool isDisposing)
        {
            Root.Dispose();
        }

        public void PrepareDraw(GameTime gameTime, List<IRenderItem> renderList)
        {
            Root.PrepareDraw(gameTime, renderList);
        }
    }
}
