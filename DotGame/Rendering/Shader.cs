using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using DotGame.Assets;
using System.Numerics;

namespace DotGame.Rendering
{
    public abstract class Shader : EngineObject
    {
        public string Name { get; private set; }

        public Shader(Engine engine, string name)
            : base(engine)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be not null, empty or white-space.", "name");
            
            this.Name = name;

            engine.ShaderManager.RegisterShader(this);
        }

        public abstract void Apply(Pass pass, IRenderContext context, Material material, Matrix4x4 worldViewProj);
    }
}
