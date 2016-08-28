using System;
using System.Collections.Generic;
using System.Linq;

namespace DotGame.Rendering
{
    /// <summary>
    /// Speichert die Passes und deren Reihenfolge die für das Rendering benutzt werden soll.
    /// </summary>
    public abstract class PassPipeline : EngineComponent
    {
        private object locker = new object();
        private Pass[] passes;
        private List<Pass> nextPasses = new List<Pass>();
        private bool passesDirty = true;

        public IReadOnlyList<Pass> Passes
        {
            get
            {
                lock(locker)
                {
                    return nextPasses.AsReadOnly();
                }
            }
        }

        /// <summary>
        /// Gibt die Anzahl der Passes in dieser Pipeline an.
        /// </summary>
        public int PassCount
        {
            get
            {
                lock(locker)
                {
                    return nextPasses.Count;
                }
            }
        }

        public PassPipeline(Engine engine)
            : base(engine)
        {
        }

        public PassPipeline(Engine engine, params Pass[] passes)
            : this(engine)
        {
            if (passes == null)
                throw new ArgumentNullException("passes");

            this.nextPasses = passes.ToList();
        }

        /// <summary>
        /// Erstellt eine Standard PassPipeline.
        /// </summary>
        /// <returns></returns>
        public static PassPipeline CreateDefault(Engine engine, Scene scene)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            //TODO (Robin) Je nach Feature Level & API passende Pipeline erstellen (Forward for mobile, Deferred for PC,...)

            return new DeferredPipeline(engine, scene);
        }

        /// <summary>
        /// Fügt einen Pass zu der Pipeline hinzu.
        /// </summary>
        /// <param name="pass"></param>
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

        /// <summary>
        /// Entfernt einen Pass der Pipeline.
        /// </summary>
        /// <param name="pass"></param>
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

        public override void Draw(GameTime gameTime)
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
