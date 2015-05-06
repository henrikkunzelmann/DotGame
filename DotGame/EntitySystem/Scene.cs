using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.EntitySystem.Components;
using DotGame.Graphics;

namespace DotGame.EntitySystem
{
    public sealed class Scene : EventHandler
    {
        private List<Entity> rootNodes = new List<Entity>();

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

        protected override EventHandler[] GetChildHandlers()
        {
            return GetRootChildren();
        }
    }
}
