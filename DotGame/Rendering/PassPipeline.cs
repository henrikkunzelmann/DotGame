using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Rendering
{
    /// <summary>
    /// Speichert die Passes und deren Reihenfolge die für das Rendering benutzt werden soll.
    /// </summary>
    public class PassPipeline : EngineObject
    {
        private object locker = new object();
        private Pass[] passes;
        private List<Pass> nextPasses = new List<Pass>();
        private bool passesDirty = true;

        public PassPipeline(Engine engine)
            : base(engine)
        {
            
        }

        public void AddPass(Pass pass)
        {
            if (pass == null)
                throw new ArgumentNullException("pass");

            lock (locker)
            {
                if (nextPasses.Contains(pass))
                    throw new InvalidOperationException("Pass already added.");

                nextPasses.Add(pass);
                passesDirty = true;
            }
        }

        public void RemovePass(Pass pass)
        {
            if (pass == null)
                throw new ArgumentNullException("pass");

            lock (locker)
            {
                nextPasses.Remove(pass);
                passesDirty = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            lock (locker)
            {
                if (passesDirty)
                {
                    passes = nextPasses.ToArray();
                    passesDirty = false;
                }
            }

            for (int i = 0; i < passes.Length; i++)
                passes[i].Render(gameTime);
        }

        protected override void Dispose(bool isDisposing)
        {
            lock (locker)
            {
                foreach (Pass pass in nextPasses)
                    pass.Dispose();
            }
        }
    }
}
