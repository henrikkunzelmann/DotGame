using DotGame.Assets;
using Newtonsoft.Json;

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
