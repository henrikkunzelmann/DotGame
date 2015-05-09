using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame;
using DotGame.Assets;
using DotGame.Graphics;
using DotGame.Rendering;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt ein Entity dar, welches ein Mesh und ein Material in die Scene bringt.
    /// </summary>
    public class MeshEntity : Entity, IRenderItem
    {
        public Mesh Mesh { get; private set; }
        public Material Material { get; private set; }

        public MeshEntity(Scene scene, string name, Mesh mesh, Material material)
            : base(scene, name)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            if (material == null)
                throw new ArgumentNullException("material");

            this.Mesh = mesh;
            this.Material = material;
        }

        public void Draw(GameTime gameTime, Pass pass, IRenderContext context)
        {
            pass.Shader.Apply(context, Scene.Camera.View * Scene.Camera.Projection, Material, World);
            Mesh.Draw(context);
        }
    }
}
