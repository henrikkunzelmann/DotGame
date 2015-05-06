using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotGame.EntitySystem.Components;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DotGame.EntitySystem
{
    /// <summary>
    /// Stellt ein Objekt in der Szene dar, welches Komponenten kapselt.
    /// </summary>
    public sealed class Entity : EventHandler, IEquatable<Entity>
    {
        /// <summary>
        /// Ruft die Szene des Entities ab.
        /// </summary>
        [JsonIgnore]
        public Scene Scene { get { return scene; } set { SetScene(value); } }
        [JsonIgnore]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Scene scene;

        /// <summary>
        /// Die Guid des Entities.
        /// </summary>
        public readonly Guid Guid = Guid.NewGuid();

        /// <summary>
        /// Ruft den Namen des Entities ab, oder legt diesen fest.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or whitespace.");
                name = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;

        /// <summary>
        /// Gibt die Transformations-Komponente des Entites zurück.
        /// </summary>
        [JsonIgnore]
        public Transform Transform { get { return GetComponent<Transform>(); } }

        [JsonRequired]
        private List<Component> components = new List<Component>();

        #region Operatoren
        public static bool operator ==(Entity a, Entity b)
        {
            if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
                return true;
            if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))
                return false;

            return a.Guid == b.Guid;
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
        #endregion

        #region Konstruktoren
        [JsonConstructor]
        private Entity()
        {
        }

        internal Entity(Scene scene, string name)
        {
            this.Scene = scene;
            Name = name;
            AddComponent<Transform>();
        }
        #endregion

        #region Komponenten
        /// <summary>
        /// Ruft eine Komponente vom gegebenen Typen ab.
        /// Wenn mehrere Komponenten des Typs existieren, wird der erste Treffer zurückgegeben.
        /// </summary>
        /// <param name="type">Der Typ der Komponente.</param>
        /// <param name="component">Bei erfolgreicher Suche die Komponente, ansonsten null.</param>
        /// <returns>True, wenn eine Komponente gefunden wurde, ansonsten false.</returns>
        public bool TryGetComponent(Type type, out Component component)
        {
            lock (components)
            {
                component = components.Find(c => c.GetType() == type);
                return component != null;
            }
        }

        /// <summary>
        /// Ruft eine Komponente vom gegebenen Typen ab.
        /// Wenn mehrere Komponenten des Typs existieren, wird der erste Treffer zurückgegeben.
        /// </summary>
        /// <typeparam name="T">Der Typ der Komponente.</typeparam>
        /// <param name="component">Bei erfolgreicher Suche die Komponente, ansonsten null.</param>
        /// <returns>True, wenn eine Komponente gefunden wurde, ansonsten false.</returns>
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            var type = typeof(T);
            lock (components)
            {
                component = (T)components.Find(c => c.GetType() == type);
                return component != null;
            }
        }

        /// <summary>
        /// Ruft eine Komponente vom gegebenen Typen ab.
        /// Wenn mehrere Komponenten des Typs existieren, wird der erste Treffer zurückgegeben.
        /// </summary>
        /// <param name="type">Der Typ der Komponente.</param>
        /// <returns>Bei erfolgreicher Suche die Komponente, ansonsten null.</returns>
        public Component GetComponent(Type type)
        {
            Component c;
            if (TryGetComponent(type, out c))
                return c;

            return null;
        }
        /// <summary>
        /// Ruft eine Komponente vom gegebenen Typen ab.
        /// Wenn mehrere Komponenten des Typs existieren, wird der erste Treffer zurückgegeben.
        /// </summary>
        /// <typeparam name="T">Der Typ der Komponente.</typeparam>
        /// <returns>Bei erfolgreicher Suche die Komponente, ansonsten null.</returns>
        public T GetComponent<T>() where T : Component
        {
            lock (components)
                return (T)GetComponent(typeof(T));
        }

        /// <summary>
        /// Fügt eine neue Komponente vom gegebenen Typen hinzu und gibt diese zurück.
        /// </summary>
        /// <typeparam name="T">Der Typ der Komponente.</typeparam>
        /// <returns>Die neue Komponente.</returns>
        public T AddComponent<T>() where T : Component
        {
            var type = typeof(T);
            var component = (T)Activator.CreateInstance(type);
            component.Entity = this;
            component.Invoke("Init", false);
            lock (components)
                components.Add(component);

            return component;
        }

        /// <summary>
        /// Gibt alle Komponenten des Entities zurück.
        /// Die Referenzen der Komponenten werden in einem neuen Array gespeichert, weshalb es ratsam wäre, den zurückgegebenen Array zu cachen.
        /// </summary>
        /// <returns>Die Komponenten.</returns>
        public Component[] GetComponents()
        {
            lock (components)
                return components.ToArray();
        }

        /// <summary>
        /// Gibt alle Komponenten des Entities zurück, die den gegebenen Typen übereinstimmen.
        /// Die Referenzen der Komponenten werden in einem neuen Array gespeichert, weshalb es ratsam wäre, den zurückgegebenen Array zu cachen.
        /// </summary>
        /// <param name="types">Die akzeptierten Komponent-Typen. Wenn null werden alle Komponenten zurückgegeben.</param>
        /// <returns>Die Komponenten.</returns>
        public Component[] GetComponents(params Type[] types)
        {
            if (types == null)
            {
                lock (components)
                    return components.ToArray();
            }
            else
            {
                lock (components)
                    return components.Where(c => types.Contains(c.GetType())).ToArray();
            }
        }
        #endregion

        #region EventHandler
        protected override EventHandler[] GetChildHandlers()
        {
            return GetComponents();
        }
        #endregion

        internal void AfterDeserialize(Transform transform)
        {
            lock (components)
            {
                // Die Referenz zur Transform-Komponente geht beim Serialisieren verloren.
                components.Insert(0, transform);

                foreach (var c in components)
                    c._AfterDeserialize(this);
            }
        }

        private void SetScene(Scene scene)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");
            if (this.scene != null)
                throw new InvalidOperationException("Entity already has a scene.");

            this.scene = scene;
        }

        /// <inheritdoc/>
        public bool Equals(Entity other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj != null && obj is Entity && ((Entity)obj).Guid == Guid;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}
