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
    [SingleComponent]
    public class StaticModel : Component
    {
        // TODO (Joex3): Material über Json/Bson serialisierbar machen.
        [JsonIgnore]
        public Material Material
        {
            get; private set;
        }

        [JsonIgnore]
        public StaticMesh Mesh
        {
            get; private set;
        }

        public StaticModel(StaticMesh mesh, Material material)
        {
            Material = material;
            Mesh = mesh;
        }
    }
}
