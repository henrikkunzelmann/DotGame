using DotGame.Graphics;
using DotGame.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem
{
    /// <summary>
    /// Stellt eine Basiskomponente dar.
    /// </summary>
    public abstract class Component : EventHandler
    {
        /// <summary>
        /// Ruft das Entity, in dem diese Komponente registriert ist, ab.
        /// </summary>
        public Entity Entity { get { return entity; } set { if (entity != null) throw new InvalidOperationException("Entity already set."); entity = value; } }
        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Entity entity;

        /// <summary>
        /// Wird bei der Initialisierung aufegrufen.
        /// </summary>
        [Event]
        protected virtual void Init() { }

        /// <summary>
        /// Wird bei einem Update aufegrufen.
        /// </summary>
        [Event]
        protected virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Wird beim Zeichnen aufegrufen.
        /// </summary>
        [Event]
        protected virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Wird aufgerufen, wenn ein Pass sich auf das Rendern vorbereitet.
        /// </summary>
        [Event]
        protected virtual void PrepareDraw(GameTime gameTime, List<IRenderItem> renderList) { }

        /// <summary>
        /// Wird nach dem Deserialisieren aufgerufen.
        /// </summary>
        protected virtual void AfterDeserialize(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (this.Entity == null)
                this.Entity = entity;
        }

        internal void _AfterDeserialize(Entity entity)
        {
            AfterDeserialize(entity);
        }
    }
}
