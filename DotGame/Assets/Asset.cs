using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets
{
    public abstract class Asset : EngineObject
    {
        public AssetManager Manager { get; private set; }

        /// <summary>
        /// Gibt den AssetType an.
        /// </summary>
        public AssetType Type { get; private set; }

        /// <summary>
        /// Gibt den Namen an.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gibt den Dateinamen an, von dem die Datei geladen wurde. Ist null, wenn Asset nicht von einer Datei geladen wurde.
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// Gibt an, ob das Asset dynamisch geladen und entladen werden kann. 
        /// </summary>
        public bool AllowStreaming { get; private set; }

        /// <summary>
        /// Gibt an, ob das Asset zur Benutzung markiert ist.
        /// </summary>
        public bool IsMarkedForUsage { get; private set; }

        /// <summary>
        /// Gibt an, wann das Asset zuletzt zur Benutzung markiert wurde.
        /// </summary>
        public GameTime LastMarkedForUsage { get; private set; }

        /// <summary>
        /// Gibt an, ob das Asset geladen ist und zur Benutzung bereit ist.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gibt an, wann das Asset zuletzt geladen wurde.
        /// </summary>
        public GameTime LastLoaded { get; private set; }

        /// <summary>
        /// Gibt an, wann das Asset zuletzt entladen wurde.
        /// </summary>
        public GameTime LastUnloaded { get; private set; }

        internal Asset(AssetManager manager, AssetType type, string name, string file)
            : base(manager.Engine)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must not be null, empty or white-space.", "name");

            this.Name = name;
            this.Type = type;
            this.File = file;
        }

        /// <summary>
        /// Markiert ein Asset zur Nutzung, damit wird das Asset dann geladen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void MarkForUsage()
        {
            IsMarkedForUsage = true;
            LastMarkedForUsage = Engine.GameTime;

            if (!AllowStreaming)
                ForceLoad();
        }

        protected sealed override void Dispose(bool isDisposing)
        {
            ForceUnload();

            Manager.Remove(this);
        }

        /// <summary>
        /// Sorgt dafür, dass das Asset im Hintegrrund geladen wird. 
        /// </summary>
        public void ForceLoadAsync()
        {
            if (IsLoaded)
                return;

            // TODO (henrik1235) Task erstellen und ForceLoad async aufrufen
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorgt dafür, dass das Asset geladen wird.
        /// </summary>
        public void ForceLoad()
        {
            if (IsLoaded)
                return;

            IsMarkedForUsage = false;
            Load();

            IsLoaded = true;
            LastLoaded = Engine.GameTime;
        }

        /// <summary>
        /// Sorgt dafür, dass das Asset entladen wird.
        /// </summary>
        public void ForceUnload()
        {
            if (!IsLoaded)
                return;

            IsMarkedForUsage = false;
            Unload();

            IsLoaded = false;
            LastUnloaded = Engine.GameTime;
        }

        /// <summary>
        /// Lädt das Asset, so, dass es benutzbar ist.
        /// </summary>
        protected abstract void Load();

        /// <summary>
        /// Entlädt das Asset, das Asset wird beim nächsten Benutzen erneut geladen. 
        /// </summary>
        protected abstract void Unload();
    }
}
