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
    public sealed class Entity : IEquatable<Entity>
    {
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
        public Transform Transform { get { if (transform == null) transform = GetComponent<Transform>(); return transform; } }
        [JsonIgnore]
        private Transform transform;

        [JsonIgnore]
        private Scene scene;

        [JsonIgnore]
        public Scene Scene {
            get
            {
                return scene;
            }
            private set
            {
                if (IsRootNode && scene != null)
                {
                    scene.Root = null;
                }

                scene = value;

                lock(children)
                    children.ForEach(child => child.Scene = Scene);
            }
        }
        
        [JsonRequired]
        private List<Component> components = new List<Component>();

        /// <summary>
        /// Gibt zurück ob diese Entity disposed wurde.
        /// </summary>
        [JsonIgnore]
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gibt die Scene zurück, zu welcher diese Node gehört.
        /// </summary>
        [JsonIgnore]
        private Entity parent;

        /// <summary>
        /// Gibt die Parent-Node zurück oder setzt diese.
        /// </summary>
        [JsonIgnore]
        public Entity Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent == value)
                    return;

                if (parent != null && parent.Scene != Scene)
                    throw new ArgumentException("Parent is from a different scene.");

                // Als Child von alter Parent-Node entfernen
                if (parent != null)
                {
                    lock (parent.children)
                    {
                        parent.children.Remove(this);
                    }
                }

                parent = value;
                Scene = parent.Scene;

                Transform.MarkDirty();

                if (parent != null)
                {
                    parent.children.Add(this);
                }
            }
        }

        [JsonIgnore]
        public Engine Engine
        {
            get;
            private set;
        }

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

        public Entity(string name, Engine engine)
        {
            Name = name;
            Engine = engine;
            AddComponent(new Transform());
        }
        internal Entity(string name, Entity parent, Engine engine)
        {
            Name = name;
            Engine = engine;
            AddComponent(new Transform());
            parent.AddChild(this);
        }
        internal Entity(string name, Scene scene, Engine engine)
        {
            Name = name;
            Engine = engine;
            AddComponent(new Transform());
            Scene = scene;
            Scene.Root = this;
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
                return components.Find(c => type.IsAssignableFrom(c.GetType()));
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

        public void AddComponent(Component component)
        {
            Type type = component.GetType();

            if (Component.IsSingle(type) && GetComponent(type) != null)
                throw new InvalidOperationException(string.Format("Component of type {0} can only be added once.", type.FullName));
            
            component.Entity = this;
            lock (components)
                components.Add(component);

            
            foreach (var attrib in Component.GetRequiredGameComponents(type))
            {
                if (!this.Scene.Engine.Components.Any(t => t.GetType() == attrib.GameComponentType))
                    throw new Exception("Component requires attached GameComponent " + type.ToString());
            }

            component.Init();
        }

        public void RemoveComponent<T>() where T : Component
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            lock(components)
                components.Remove(components.First(c => type.IsAssignableFrom(c.GetType())));
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
                            list.AddRange(components.Where(c => types.Any(t => t.IsAssignableFrom(c.GetType()))));
                    }
                    foreach (var child in children)
                        list.AddRange(child.GetComponents(true, types));

                    return list.ToArray();
                }
                else
                {
                    lock (components)
                        return components.Where(c => types.Any(t => t.IsAssignableFrom(c.GetType()))).ToArray();
                }
            }
        }

        public T[] GetComponents<T>(bool deep) where T : Component
        {
            Component[] components = GetComponents(deep, typeof(T));

            if (components == null)
                return null;

            T[] tComponents = new T[components.Length];
            for (int i = 0; i < components.Length; i++)
                tComponents[i] = (T)components[i];

            return tComponents;
        }
        #endregion

            #region Entity

            /// <summary>
            /// Gibt zurück ob diese Node eine Root-Node ist, d.h. keine Parent-Node hat.
            /// </summary>
        [JsonIgnore]
        public bool IsRootNode
        {
            get { return Parent == null; }
        }

        private List<Entity> children = new List<Entity>();

        /// <summary>
        /// Gibt alle Children dieser Node zurück.
        /// </summary>
        public IReadOnlyCollection<Entity> Children
        {
            get
            {
                lock (children)
                {
                    List<Entity> entities = new List<Entity>();
                    children.ForEach(l => entities.Add(l));
                    return entities.AsReadOnly();
                }
            }
        }
        
        /// <summary>
        /// Fügt eine Entity als Child zu dieser Node hinzu.
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.Parent = this;
            children.Add(entity);
        }

        /// <summary>
        /// Entfernt eine Entity als Child von dieser Node.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (entity.Parent != this)
                throw new ArgumentException("Entity is not child of this node.", "entity");

            entity.Parent = null;
            this.children.Remove(entity);
        }

        /// <summary>
        /// Gibt zurück, ob eine Node als Child in dieser Node vorhanden ist.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsChild(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return entity.Parent == this;
        }
        #endregion

        public void Init()
        {
            components.ForEach(component => component.Init());
        }

        public void Update(GameTime gameTime)
        {
            components.ForEach(component => component.Update(gameTime));
            children.ForEach(entity => entity.Update(gameTime));
        }

        public void AfterDeserialize()
        {
            children.ForEach(entity => entity.Parent = this);
            components.ForEach(component => component.AfterDeserialize(this));
        }

        /// <summary>
        /// Zerstört das Entity.
        /// </summary>
        public void Destroy()
        {
            components.ForEach(component => component.Destroy());
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
