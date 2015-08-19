using DotGame.EntitySystem.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem
{
    /// <summary>
    /// Kapselt ein Entity, das später instanziiert werden kann.
    /// </summary>
    public sealed class Prefab
    {
        private byte[] data;
        // Nur für Testzwecke.
        private string jsonData;

        private static JsonSerializer serializer;

        static Prefab()
        {
            serializer = new JsonSerializer();
            serializer.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        }

        /// <summary>
        /// Erstellt ein neues Prefab aus dem gegebenen Entity.
        /// </summary>
        /// <param name="entity">Das Entity.</param>
        /// <returns>Das Prefab.</returns>
        public static Prefab FromEntity(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var prefab = new Prefab();
            
            using (var stream = new MemoryStream())
            {
                using (var writer = new BsonWriter(stream))
                    serializer.Serialize(writer, entity);

                prefab.data = stream.GetBuffer();
            }

            return prefab;
        }

        private Prefab()
        {
        }

        
        /// <summary>
        /// Instanziiert das zugrundeliegende Entity in der gegebenen Szene.
        /// </summary>
        /// <param name="scene">Die Szene.</param>
        /// <returns>Das Entity.</returns>
        public Entity CreateInstance()
        {
            Entity instance;
            using (var stream = new MemoryStream(data))
            using (var reader = new BsonReader(stream))
                instance = serializer.Deserialize(reader) as Entity;

            if (instance == null)
                throw new InvalidOperationException("Given prefab could not be deserialized.");
            instance.Name += " Clone";

            instance.AfterDeserialize();

            return instance;
        }
    }
}
