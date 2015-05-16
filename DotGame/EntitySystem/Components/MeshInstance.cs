using DotGame.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    /// <summary>
    /// Stellt eine Meshinstanz dar, die in einer MeshRenderer Komponente gerendert werden kann.
    /// </summary>
    public class MeshInstance : Component
    {
        /// <summary>
        /// Das Mesh, das dieser MeshInstance zugeordnet ist.
        /// </summary>
        // TODO (Joex3): Mesh über Json/Bson serialisierbar machen.
        [JsonIgnore]
        public Mesh Mesh;
    }
}
