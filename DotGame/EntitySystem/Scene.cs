using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.EntitySystem.Components;
using DotGame.Graphics;
using DotGame.Rendering;

namespace DotGame.EntitySystem
{
    public sealed class Scene : EventHandler
    {
        public readonly Engine Engine;

        private List<Entity> rootNodes = new List<Entity>();

        /// <summary>
        /// Ruft die aktuelle Kamera ab, die zum Rendern benutzt wird.
        /// </summary>
        public Camera CurrentCamera { get; internal set; }

        public Scene(Engine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            this.Engine = engine;
        }

        /// <summary>
        /// Erstellt ein neues Entity im Root-Knoten.
        /// </summary>
        /// <param name="name">Der Name des Entities.</param>
        /// <returns>Das Entity.</returns>
        public Entity CreateChild(string name)
        {
            var node = new Entity(this, name);
            lock (rootNodes)
                rootNodes.Add(node);
            return node;
        }

        public T[] GetComponents<T>() where T : Component
        {
            return GetComponents(typeof(T)).Cast<T>().ToArray();
        }

        public Component[] GetComponents(params Type[] types)
        {
            var list = new List<Component>();
            lock (rootNodes)
            {
                foreach (var node in rootNodes)
                    list.AddRange(node.GetComponents(true, types));
            }
            return list.ToArray();
        }

        internal void AddChild(Entity node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            lock (rootNodes)
                rootNodes.Add(node);
        }

        internal bool RemoveChild(Entity node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            lock (rootNodes)
                return rootNodes.Remove(node);
        }

        public Entity[] GetRootChildren()
        {
            lock (rootNodes)
                return rootNodes.ToArray();
        }

        protected override void GetChildHandlers(List<EventHandler> list)
        {
            lock (rootNodes)
                list.AddRange(rootNodes);
        }
    }
}
