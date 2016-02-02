using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame;
using DotGame.Utils;
using DotGame.Rendering;
using DotGame.Graphics;
using DotGame.EntitySystem;
using DotGame.EntitySystem.Components;
using Newtonsoft.Json;
using System.IO;

namespace DotGame
{
    /// <summary>
    /// Stellt die komplette Welt dar, d.h. SceneNodes und Camera.
    /// </summary>
    public class Scene : EngineComponent
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
        
        public Camera Camera
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

        public override void Init()
        {
            base.Init();

            Root.Init();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Root.Update(gameTime);
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
