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
    public class DeferredPipeline : PassPipeline
    {
        private Scene scene;
        public DeferredPipeline(Engine engine, Scene scene) : base(engine) { this.scene = scene; }

        public override void Init()
        {
            base.Init();

            this.AddPass(new Passes.GBufferPass(Engine, scene));
        }
    }
}
