using DotGame.EntitySystem.Components;
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

        // 1st: single, 2nd: required components
        private static Dictionary<Type, Tuple<bool, RequiresComponentAttribute[]>> componentCache = new Dictionary<Type, Tuple<bool, RequiresComponentAttribute[]>>();

        /// <summary>
        /// Gibt zurück, ob der angegebene Komponententyp ein SingleComponentAttribute enthält.
        /// </summary>
        /// <param name="type">Der Komponententyp.</param>
        /// <returns>True, wenn ein SingleCOmponentAttribute gefunden wurde, ansonsten false.</returns>
        public static bool IsSingle(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (!typeof(Component).IsAssignableFrom(type))
                throw new ArgumentException("Given type is no component.", "type");

            return GetCache(type).Item1;
        }

        /// <summary>
        /// Gibt zurück, ob der angegebene Komponententyp ein SingleComponentAttribute enthält.
        /// </summary>
        /// <typeparam name="T">Der Komponententyp.</typeparam>
        /// <returns>True, wenn ein SingleCOmponentAttribute gefunden wurde, ansonsten false.</returns>
        public static bool IsSingle<T>() where T : Component
        {
            return GetCache(typeof(T)).Item1;
        }

        /// <summary>
        /// Gibt einen Array, der alle RequiresComponentAttribute eines Komponententyps enthält, zurück.
        /// </summary>
        /// <param name="type">Der Komponententyp.</param>
        /// <returns>Den Array.</returns>
        public static RequiresComponentAttribute[] GetRequired(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (!typeof(Component).IsAssignableFrom(type))
                throw new ArgumentException("Given type is no component.", "type");

            return GetCache(type).Item2;
        }

        /// <summary>
        /// Gibt einen Array, der alle RequiresComponentAttribute eines Komponententyps enthält, zurück.
        /// </summary>
        /// <typeparam name="T">Der Komponententyp.</typeparam>
        /// <returns>Den Array.</returns>
        public static RequiresComponentAttribute[] GetRequired<T>() where T : Component
        {
            return GetCache(typeof(T)).Item2;
        }

        private static Tuple<bool, RequiresComponentAttribute[]> GetCache(Type type)
        {
            Tuple<bool, RequiresComponentAttribute[]> entry;
            if (!componentCache.TryGetValue(type, out entry))
            {
                var single = type.GetCustomAttributes(typeof(SingleComponentAttribute), true).Length > 0;
                var required = type.GetCustomAttributes(typeof(RequiresComponentAttribute), true).Cast<RequiresComponentAttribute>().ToArray();
                entry = new Tuple<bool, RequiresComponentAttribute[]>(single, required);
                componentCache[type] = entry;
            }
            return entry;
        }

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
        /// Wird aufgerufen, wenn das Entity zerstört wird.
        /// </summary>
        [Event]
        protected virtual void Destroy() { }

        /// <summary>
        /// Wird nach dem Deserialisieren aufgerufen.
        /// </summary>
        [Event]
        protected virtual void AfterDeserialize() { }
    }
}
