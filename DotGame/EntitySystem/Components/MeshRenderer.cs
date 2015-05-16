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
    [RequiresComponent(typeof(MeshInstance))]
    public class MeshRenderer : Component, IRenderItem
    {
        [JsonIgnore]
        // TODO (Joex3): Material über Json/Bson serialisierbar machen.
        public Material Material;

        public void Draw(GameTime gameTime, Pass pass, IRenderContext context)
        {
            var transform = Entity.Transform;
            var instance = Entity.GetComponent<MeshInstance>();
            if (instance.Mesh == null || Material == null)
                return;

            pass.Shader.Apply(context, Entity.Scene.CurrentCamera.ViewProjection, Material, Entity.Transform.Matrix);

            if (pass is ForwardPass)
                instance.Mesh.Draw(context);
            else
                throw new InvalidOperationException(string.Format("Pass {0} not supported!", pass.GetType().FullName));
        }

        protected override void PrepareDraw(GameTime gameTime, List<IRenderItem> renderList)
        {
            renderList.Add(this);
        }
    }
}
