using DotGame.Assets;
using DotGame.Graphics;
using System.Numerics;

namespace DotGame.Rendering
{
    public abstract class SceneShader : Shader
    {
        public SceneShader(Engine engine, string name) : base(engine, name) { }

        public abstract MaterialDescription MaterialDescription
        {
            get;
        }

        public abstract void Apply(IRenderContext context, Matrix4x4 viewProjection, Material material, Matrix4x4 world);
    }
}
