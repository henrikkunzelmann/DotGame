using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotGame.Utils;
using DotGame.Graphics;
using DotGame.Assets.Importers;

namespace DotGame.Assets
{
    /// <summary>
    /// Verwaltet Assets und stellt Funktionen zum Laden von Assets bereit.
    /// </summary>
    public class AssetManager : EngineObject
    {
        private List<Asset> assets = new List<Asset>();
        private Dictionary<string, ImporterBase> importers = new Dictionary<string, ImporterBase>();

        /// <summary>
        /// Gibt die Anzahl der geladenen Assets zurück.
        /// </summary>
        public int AssetCount
        {
            get
            {
                lock (assets)
                    return assets.Count;
            }
        }

        public AssetManager(Engine engine)
            : base(engine)
        {
            RegisterImporter(new SimpleTextureImporter(this), ".bmp", ".jpeg", ".jpg", ".png");
        }

        private void CheckExtension(string extension, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException("Extension must not be null, empty or white-space.", parameterName);
            if (!extension.StartsWith("."))
                throw new ArgumentException("Extension must start with a point: '.'.", parameterName);
        }

        public void RegisterImporter(ImporterBase importer, params string[] fileExtensions)
        {
            if (importer == null)
                throw new ArgumentNullException("importer");

            foreach (string extension in fileExtensions)
            {
                CheckExtension(extension, "fileExtensions");
                if (importers.ContainsKey(extension))
                    throw new InvalidOperationException("A importer is already registered with the extension: " + extension + ".");
                else
                    importers.Add(extension, importer);
            }

            Log.Debug("Registered {0} with extensions: {1}", importer.GetType().FullName, string.Join(", ", fileExtensions));
        }

        public bool IsImporterRegistered(string fileExtension)
        {
            CheckExtension(fileExtension, "fileExtension");
            return importers.ContainsKey(fileExtension);
        }

        public T GetImporter<T>(string fileExtension) where T : ImporterBase
        {
            CheckExtension(fileExtension, "fileExtension");
            ImporterBase importer;
            if (!importers.TryGetValue(fileExtension, out importer))
                throw new ArgumentException("No importer with the fileExtension registered.", "fileExtension");
            T t = importer as T;
            if (t == null)
                throw new InvalidCastException("Importer " + importer.GetType().Name + " does not match with type " + typeof(T).Name + ".");
            return t;
        }

        /// <summary>
        /// Fügt ein Asset zur Liste der Assets hinzu.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset"></param>
        /// <returns></returns>
        internal T Register<T>(T asset) where T : Asset
        {
            lock(assets)
                assets.Add(asset);
            if (asset.File == null)
                Log.Debug("Loaded {0} with name \"{1}\" from runtime source", asset.GetType().Name, asset.Name);
            else
                Log.Debug("Loaded {0} with name \"{1}\" from file \"{2}\"", asset.GetType().Name, asset.Name, asset.File);
            return asset;
        }

        /// <summary>
        /// Entfernt das Asset aus der Liste der Assets.
        /// </summary>
        /// <param name="asset"></param>
        internal void Remove(Asset asset)
        {
            lock (assets)
                assets.Remove(asset);

            if (asset.File == null)
                Log.Debug("Removed {0} with name \"{1}\" from runtime source", asset.GetType().Name, asset.Name);
            else
                Log.Debug("Removed {0} with name \"{1}\" from file \"{2}\"", asset.GetType().Name, asset.Name, asset.File);
        }

        public Mesh LoadMesh<TVertex>(string name, TVertex[] vertices) where TVertex : struct, IVertexType
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");
            if (vertices.Length == 0)
                throw new ArgumentException("Vertices is empty.", "vertices");
            return LoadMesh(name, vertices, vertices[0].VertexDescription);
        }

        public Mesh LoadMesh<TVertex>(string name, TVertex[] vertices, VertexDescription vertexDescription) where TVertex : struct
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");
            if (vertices.Length == 0)
                throw new ArgumentException("Vertices is empty.", "vertices");

            IVertexBuffer vertexBuffer = Engine.GraphicsDevice.Factory.CreateVertexBuffer(vertices, vertexDescription, BufferUsage.Static);
            return new Mesh(this, name, AssetType.User, vertexBuffer);
        }


        public Mesh LoadMesh<TVertex, TIndex>(string name, TVertex[] vertices, VertexDescription vertexDescription, TIndex[] indices, IndexFormat indexFormat) 
            where TVertex : struct 
            where TIndex : struct
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");
            if (vertices.Length == 0)
                throw new ArgumentException("Vertices is empty.", "vertices");
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (indices.Length == 0)
                throw new ArgumentException("Indices is empty.", "indices");

            IVertexBuffer vertexBuffer = Engine.GraphicsDevice.Factory.CreateVertexBuffer(vertices, vertexDescription, BufferUsage.Static);
            IIndexBuffer indexBuffer = Engine.GraphicsDevice.Factory.CreateIndexBuffer(indices, indexFormat, BufferUsage.Static);
            return new Mesh(this, name, AssetType.User, vertexBuffer, indexBuffer);
        }

        /// <summary>
        /// Lädt eine Texture mit Standard Einstellungen.
        /// </summary>
        /// <param name="file">Der Pfad zur Texture.</param>
        /// <returns></returns>
        public Texture LoadTexture(string name, string file)
        {
            return LoadTexture(name, file, new TextureLoadSettings());
        }

        /// <summary>
        /// Lädt eine Texture.
        /// </summary>
        /// <param name="file">Der Pfad zur Texture.</param>
        /// <param name="settings">Die Einstellungen, die für das Laden der Texture benutzt werden.</param>
        /// <returns></returns>
        public Texture LoadTexture(string name, string file, TextureLoadSettings settings)
        {
            return new Texture(this, name, file, settings, GetImporter<TextureImporterBase>(Path.GetExtension(file)));
        }

        protected override void Dispose(bool isDisposing)
        {
            lock(assets)
            {
                Asset[] assetsToDispose = assets.ToArray();
                for (int i = 0; i < assetsToDispose.Length; i++)
                    assetsToDispose[i].Dispose();
            }
        }
    }
}
