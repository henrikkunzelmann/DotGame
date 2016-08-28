using DotGame.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace DotGame.Rendering
{
    public abstract class ScenePass : Pass
    {
        public ScenePass(Engine engine, Scene scene) : base(engine) { Scene = scene; }
        
        public Scene Scene
        {
            get;
            private set;
        }

        private List<SceneShader> shaders = new List<SceneShader>();

        public void AddShader(SceneShader shader)
        {
            shaders.Add(shader);
        }
        public void RemoveShader(SceneShader shader)
        {
            shaders.Remove(shader);
        }
        public void RemoveShader(VertexDescription vertexDescription, MaterialDescription materialDescription)
        {
            shaders.Remove(shaders.First(s => s.VertexDescription.EqualsIgnoreOrder(vertexDescription) && s.MaterialDescription.Equals(materialDescription)));
        }
        public SceneShader GetShader(VertexDescription vertexDescription, MaterialDescription materialDescription)
        {
            return shaders.First(s => s.VertexDescription.EqualsIgnoreOrder(vertexDescription) && s.MaterialDescription.Equals(materialDescription));
        }



        protected override void Dispose(bool isDisposing)
        {
            foreach (var shader in shaders)
            {
                if (shader != null)
                    shader.Dispose();
            }
        }
    }
}
