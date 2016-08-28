using DotGame.EntitySystem;
using DotGame.EntitySystem.Components;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace DotGame
{
    /// <summary>
    /// Stellt die komplette Welt dar, d.h. SceneNodes und Camera.
    /// </summary>
    public class Scene : GameComponent
    {
        /// <summary>
        /// Gibt die Root-SceneNode an.
        /// </summary>
        public Entity Root {
            get;
            set;
        }

        [JsonIgnore]
        private Camera camera;
        
        public Camera CurrentCamera
        {
            get
            {
                if (camera == null)
                    camera = (Camera)Root.GetComponents(true, typeof(Camera)).First();
                return camera;
            }
            private set
            {
                camera = value;
            }
        }

        public Scene(Engine engine)
            : base(engine)
        {
            Root = new Entity("root", this, engine);
        }

        public void SerializeScene()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            File.WriteAllText("scene.dot",writer.ToString());
        }

        public override void Update(GameTime gameTime)
        {
            Root.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Dispose(bool isDisposing)
        {
            Root.Destroy();
        }

        public override void Unload()
        {
            base.Unload();

            Root.Destroy();
        }
    }
}
