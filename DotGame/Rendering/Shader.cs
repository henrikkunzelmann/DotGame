using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Assets;
using DotGame.Graphics;
using System.Numerics;
using DotGame.EntitySystem.Components;

namespace DotGame.Rendering
{
    public abstract class Shader : EngineObject
    {
        public string Name { get; private set; }

        protected IShader shader { get; private set; }

        public Shader(Engine engine, string name)
            : base(engine)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be not null, empty or white-space.", "name");

            this.Name = name;
            this.shader = engine.ShaderManager.CompileShader(name);
        }

        public VertexDescription VertexDescription
        {
            get
            {
                return shader?.VertexDescription;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            shader.Dispose();
        }
    }
}
