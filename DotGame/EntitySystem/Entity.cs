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
        /// <returns>Bei erfolgreicher Suche die Komponente, ansonsten null.</returns>
        public Component GetComponent(Type type)
        {
            lock (components)
                return components.Find(c => c.GetType() == type);
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

        public Component AddComponent(Type type)
        {
            if (type.GetCustomAttributes(typeof(SingleComponentAttribute), true).Length > 0 && GetComponent(type) != null)
                throw new InvalidOperationException(string.Format("Component of type {0} can only be added once.", type.FullName));

            var component = (Component)Activator.CreateInstance(type);

            component.Entity = this;
            lock (components)
                components.Add(component);

            foreach (var attrib in type.GetCustomAttributes(typeof(RequiresComponentAttribute), true))
            {
                var a = (RequiresComponentAttribute)attrib;
                if (GetComponent(a.ComponentType) == null)
                    AddComponent(a.ComponentType);
            }

            component.Invoke("Init", false);

            return component;
        }

        /// <summary>
        /// Fügt eine neue Komponente vom gegebenen Typen hinzu und gibt diese zurück.
        /// </summary>
        /// <typeparam name="T">Der Typ der Komponente.</typeparam>
        /// <returns>Die neue Komponente.</returns>
        public T AddComponent<T>() where T : Component
        {
            return (T)AddComponent(typeof(T));
        }

        /// <summary>
        /// Gibt alle Komponenten des Entities zurück.
        /// </summary>
        /// <param name="deep">Gibt an, ob auch Komponenten von Kind-Entities zurückgegeben werden sollen.</param>
        /// <returns>Die Komponenten.</returns>
        public Component[] GetComponents(bool deep)
        {
            return GetComponents(true, null);
        }

        /// <summary>
        /// Gibt alle Komponenten des Entities zurück, die den gegebenen Typen übereinstimmen.
        /// </summary>
        /// <param name="deep">Gibt an, ob auch Komponenten von Kind-Entities zurückgegeben werden sollen.</param>
        /// <param name="types">Die akzeptierten Komponent-Typen. Wenn null werden alle Komponenten zurückgegeben.</param>
        /// <returns>Die Komponenten.</returns>
        public Component[] GetComponents(bool deep, params Type[] types)
        {
            // TODO (Joex3): Wah.
            if (types == null && !deep)
            {
                lock (components)
                    return components.ToArray();
            }
            else
            {
                if (deep)
                {
                    var list = new List<Component>();
                    if (types == null)
                    {
                        lock (components)
                            list.AddRange(components);
                    }
                    else
                    {
                        lock (components)
                            list.AddRange(components.Where(c => types.Contains(c.GetType())));
                    }
                    foreach (var child in Transform.GetChildren())
                        list.AddRange(child.Entity.GetComponents(false, types));

                    return list.ToArray();
                }
                else
                {
                    lock (components)
                        return components.Where(c => types.Contains(c.GetType())).ToArray();
                }
            }
        }
        #endregion

        #region EventHandler
        protected override void GetChildHandlers(List<EventHandler> handlers)
        {
            lock (components)
                handlers.AddRange(components);
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
