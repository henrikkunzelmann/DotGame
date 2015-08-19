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
    public class DotGamePipeline : PassPipeline
    {
        private Scene scene;
        public DotGamePipeline(Engine engine, Scene scene) : base(engine) { this.scene = scene; }

        public override void Init()
        {
            base.Init();

            this.AddPass(new EntitySystem.Rendering.ForwardPass(Engine, scene));
        }
    }
}
