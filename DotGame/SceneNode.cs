using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame
{
    public class SceneNode : RenderItem
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name can not be null or white space.");
                name = value;
            }
        }

        public SceneNode(Engine engine, string name)
            : base(engine)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name can not be null or white space.", "name");
        }

        public void UpdateTree(GameTime gameTime)
        {
            Update(gameTime);
        }

        public void DrawTree(GameTime gameTime, RenderItemCollection items)
        {
            items.AddItem(this);
        }

        public void DisposeTree()
        {
            Dispose();
        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, Pass pass, IRenderContext context)
        {

        }

        protected override void Dispose(bool isDisposing)
        {
            
        }
    }
}
