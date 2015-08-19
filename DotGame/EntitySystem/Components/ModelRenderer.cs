using DotGame.Assets;
using DotGame.EntitySystem.Rendering;
using DotGame.Graphics;
using DotGame.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    public class ModelRenderer : Renderer
    {
        // TODO (Joex3): Material über Json/Bson serialisierbar machen.
        [JsonIgnore]
        public Material Material
        {
            get; private set;
        }

        [JsonIgnore]
        public Mesh Mesh
        {
            get; private set;
        }

        public ModelRenderer(Mesh mesh, Material material)
        {
            Material = material;
            Mesh = mesh;
        }


        public override void Draw(GameTime gameTime, Pass pass, IRenderContext context)
        {
            var transform = Entity.Transform;
            if (Mesh == null || Material == null)
                return;

            pass.Shader.Apply(context, Entity.Scene.CurrentCamera.ViewProjection, Material, Entity.Transform.Matrix);

            if (pass is ForwardPass)
                Mesh.Draw(context);
            else
                throw new InvalidOperationException(string.Format("Pass {0} not supported!", pass.GetType().FullName));
        }
    }
}
