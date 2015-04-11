using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering
{
    public class PassPipeline : EngineObject
    {
        private List<Pass> passes = new List<Pass>();
        private List<Pass> nextPasses = new List<Pass>();

        public PassPipeline(Engine engine)
            : base(engine)
        {
            
        }

        public void AddPass(Pass pass)
        {
            if (pass == null)
                throw new ArgumentNullException("pass");
            if (nextPasses.Contains(pass))
                throw new InvalidOperationException("Pass already added.");

            nextPasses.Add(pass);
        }

        public void RemovePass(Pass pass)
        {
            if (pass == null)
                throw new ArgumentNullException("pass");

            nextPasses.Remove(pass);
        }

        public void Draw(GameTime gameTime)
        {
            passes = nextPasses.ToList();
      
            foreach (Pass pass in passes)
                pass.Render(gameTime);
        }

        protected override void Dispose(bool isDisposing)
        {
            foreach (Pass pass in nextPasses)
                pass.Dispose();
        }
    }
}
