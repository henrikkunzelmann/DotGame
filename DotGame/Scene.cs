using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Utils;
using DotGame.Cameras;
using DotGame.Rendering;

namespace DotGame
{
    public class Scene : EngineObject
    {
        private ICamera camera;

        // TODO (henrik1235) Scenegraph
        public List<SceneNode> Nodes { get; private set; }

        public ICamera Camera
        {
            get { return camera; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                camera = value;
            }
        }

        public Scene(Engine engine)
            : base(engine)
        {
            Nodes = new List<SceneNode>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (SceneNode node in Nodes)
                node.UpdateTree(gameTime);
        }

        public void PrepareForDraw(GameTime gameTime, List<RenderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (SceneNode node in Nodes)
                node.PrepareForDrawTree(gameTime, items);
        }

        protected override void Dispose(bool isDisposing)
        {
            foreach (SceneNode node in Nodes.ToArray())
                node.Dispose();
        }
    }
}
