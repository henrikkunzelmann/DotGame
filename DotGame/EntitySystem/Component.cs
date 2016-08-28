using DotGame.EntitySystem.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotGame.EntitySystem
{
    /// <summary>
    /// Stellt eine Basiskomponente dar.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Ruft das Entity, in dem diese Komponente registriert ist, ab.
        /// </summary>
        [JsonIgnore]
        public Entity Entity { get { return entity; } set { if (entity != null) throw new InvalidOperationException("Entity already set."); entity = value; } }
        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Entity entity;

        // 1st: single, 2nd: required components
        private static Dictionary<Type, Tuple<bool, RequiresComponentAttribute[], RequiresGameComponentAttribute[]>> componentCache = new Dictionary<Type, Tuple<bool, RequiresComponentAttribute[], RequiresGameComponentAttribute[]>>();

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

        /// <summary>
        /// Gibt einen Array, der alle RequiresComponentAttribute eines Komponententyps enthält, zurück.
        /// </summary>
        /// <param name="type">Der Komponententyp.</param>
        /// <returns>Den Array.</returns>
        public static RequiresGameComponentAttribute[] GetRequiredGameComponents(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (!typeof(Component).IsAssignableFrom(type))
                throw new ArgumentException("Given type is no component.", "type");

            return GetCache(type).Item3;
        }

        /// <summary>
        /// Gibt einen Array, der alle RequiresComponentAttribute eines Komponententyps enthält, zurück.
        /// </summary>
        /// <typeparam name="T">Der Komponententyp.</typeparam>
        /// <returns>Den Array.</returns>
        public static RequiresGameComponentAttribute[] GetRequiredGameComponents<T>() where T : Component
        {
            return GetCache(typeof(T)).Item3;
        }

        private static Tuple<bool, RequiresComponentAttribute[], RequiresGameComponentAttribute[]> GetCache(Type type)
        {
            Tuple<bool, RequiresComponentAttribute[], RequiresGameComponentAttribute[]> entry;
            if (!componentCache.TryGetValue(type, out entry))
            {
                var single = type.GetCustomAttributes(typeof(SingleComponentAttribute), true).Length > 0;
                var required = type.GetCustomAttributes(typeof(RequiresComponentAttribute), true).Cast<RequiresComponentAttribute>().ToArray();
                var gameRequired = type.GetCustomAttributes(typeof(RequiresGameComponentAttribute), true).Cast<RequiresGameComponentAttribute>().ToArray();
                entry = new Tuple<bool, RequiresComponentAttribute[], RequiresGameComponentAttribute[]>(single, required, gameRequired);
                componentCache[type] = entry;
            }
            return entry;
        }

        /// <summary>
        /// Wird bei der Initialisierung aufegrufen.
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// Wird bei einem Update aufegrufen.
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Wird aufgerufen, wenn das Entity zerstört wird.
        /// </summary>
        public virtual void Destroy() { }

        /// <summary>
        /// Wird nach dem Deserialisieren aufgerufen.
        /// </summary>
        public virtual void AfterDeserialize(Entity entity) { this.Entity = entity; }
    }
}
