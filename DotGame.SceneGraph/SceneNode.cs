using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DotGame.Rendering;
using DotGame.Graphics;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt eine Node im Scene-Tree dar.
    /// </summary>
    public class SceneNode : IDisposable
    {
        /// <summary>
        /// Gibt zurück ob diese SceneNode disposed wurde.
        /// </summary>
        public bool IsDisposed { get; private set; }

        public Engine Engine
        {
            get { return Scene.Engine; }
        }

        /// <summary>
        /// Gibt die Scene zurück, zu welcher diese Node gehört.
        /// </summary>
        public Scene Scene { get; private set; }

        /// <summary>
        /// Gibt den Namen der Scene zurück.
        /// </summary>
        public string Name { get; private set; }

        private SceneNode parent;

        /// <summary>
        /// Gibt die Parent-Node zurück oder setzt diese.
        /// </summary>
        public SceneNode Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (value.Scene != Scene)
                    throw new ArgumentException("Parent is from a different scene.");

                // Node ist schon Child von Parent, nichts tun
                if (value == parent)
                    return;

                // Als Child von alter Parent-Node entfernen
                if (parent != null)
                {
                    lock (parent.children)
                    {
                        parent.children.Remove(this);
                    }
                }

                // Parent neu setzen
                parent = value;

                // Als Child bei neuer Parent-Node hinzufügen
                if (parent != null)
                {
                    lock(parent.children)
                    {
                        parent.children.Add(this);
                    }
                }
            }
        }

        /// <summary>
        /// Gibt zurück ob diese Node eine Root-Node ist, d.h. keine Parent-Node hat.
        /// </summary>
        public bool IsRootNode
        {
            get { return Parent == null; }
        }

        private List<SceneNode> children = new List<SceneNode>();

        /// <summary>
        /// Gibt alle Children dieser Node zurück.
        /// </summary>
        public IReadOnlyCollection<SceneNode> Children
        {
            get 
            {
                lock (children)
                {
                    return children.AsReadOnly();
                }
            }
        }

        /// <summary>
        /// Gibt die Matrix zurück, welche die lokale Transformation dieser Node angibt.
        /// </summary>
        public virtual Matrix4x4 LocalWorld { get; protected set; }

        /// <summary>
        /// Gibt die Matrix zurück, welche die globale Transformation dieser Node angibt.
        /// </summary>
        public Matrix4x4 World
        {
            get
            {
                if (parent != null)
                    return LocalWorld * parent.World;
                return LocalWorld;
            }
        }

        public SceneNode(Scene scene, string name)
        {
            if (scene == null)
                throw new ArgumentNullException("scene");
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be not empty or white-space.", "name");

            this.Scene = scene;
            this.Name = name;

            LocalWorld = Matrix4x4.Identity;
        }

        public SceneNode(SceneNode parent, string name)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be not empty or white-space.", "name");

            this.Scene = parent.Scene;
            this.Parent = parent;
            this.Name = name;

            LocalWorld = Matrix4x4.Identity;
        }

        /// <summary>
        /// Fügt eine SceneNode als Child zu dieser Node hinzu.
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(SceneNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Parent = this;
        }

        /// <summary>
        /// Entfernt eine SceneNode als Child von dieser Node.
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(SceneNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (node.Parent != this)
                throw new ArgumentException("Node is not child of this node.", "node");

            node.Parent = null;
        }

        /// <summary>
        /// Gibt zurück, ob eine Node als Child in dieser Node vorhanden ist.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsChild(SceneNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            return node.Parent == this;
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            Dispose(true);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool isDisposing)
        {

        }

        public void PrepareDraw(GameTime gameTime, List<IRenderItem> renderList)
        {
            IRenderItem item = this as IRenderItem;
            if (item != null)
                renderList.Add(item);

            lock(children)
            {
                for (int i = 0; i < children.Count; i++)
                    children[i].PrepareDraw(gameTime, renderList);
            }
        }
    }
}
