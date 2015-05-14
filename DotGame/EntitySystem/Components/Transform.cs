using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    /// <summary>
    /// Stellt die Transformation und die Verkettung untereinander eines Entities dar.
    /// </summary>
    [SingleComponent]
    public class Transform : Component
    {
        /// <summary>
        /// Ruft Eltern-Transformation ab, oder legt diese fest.
        /// </summary>
        [JsonIgnore]
        public Transform Parent { get { return parent; } set { SetParent(value); } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Transform parent;

        /// <summary>
        /// Ruft die World-Matrix dieser Transformation ab (in Anbetracht aller Eltern-Transformationen).
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 Matrix { get { EnsureNotDirty();  return matrix; } }

        // TODO(Joex3): Set globale Position, Scale und Rotation.

        /// <summary>
        /// Ruft die globale Position dieser Transformation ab.
        /// </summary>
        [JsonIgnore]
        public Vector3 Position { get { EnsureNotDirty(); return position; } }

        /// <summary>
        /// Ruft die globale Rotation dieser Transformation ab.
        /// </summary>
        [JsonIgnore]
        public Quaternion Rotation { get { EnsureNotDirty(); return rotation; } }

        /// <summary>
        /// Ruft die globale Skalierung dieser Transformation ab (Kann bei Rotationen verzerrt werden).
        /// </summary>
        [JsonIgnore]
        public Vector3 Scale { get { EnsureNotDirty(); return scale; } }

        /// <summary>
        /// Ruft die lokale Postion dieser Transformation ab, oder legt diese fest.
        /// </summary>
        public Vector3 LocalPosition { get { return localPosition; } set { localPosition = value; MarkDirty(); } }

        /// <summary>
        /// Ruft die lokale Rotation dieser Transformation ab, oder legt diese fest.
        /// </summary>
        public Quaternion LocalRotation { get { return localRotation; } set { localRotation = value; MarkDirty(); } }

        /// <summary>
        /// Ruft die lokale Skalierung dieser Transformation ab, oder legt diese fest.
        /// </summary>
        public Vector3 LocalScale { get { return localScale; } set { localScale = value; MarkDirty(); } }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 localPosition = Vector3.Zero;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Quaternion localRotation = Quaternion.Identity;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 localScale = new Vector3(1);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Matrix4x4 matrix;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 position;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Quaternion rotation;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3 scale;

        private bool isDirty = true;

        [JsonRequired]
        private List<Transform> children = new List<Transform>();

        protected override void Init()
        {
            base.Init();

            lock (children)
            {
                foreach (var child in children)
                {
                    child.Init();
                }
            }
        }

        protected override void AfterDeserialize(Entity entity)
        {
            base.AfterDeserialize(entity);

            foreach (var c in GetChildren(1))
                c.AfterDeserialize(entity);
        }

        protected override void GetChildHandlers(List<EventHandler> handlers)
        {
            lock (children)
            {
                foreach (var child in children)
                    handlers.Add(child.Entity);
            }
        }

        /// <summary>
        /// Gibt zurück, ob die gegebene Transformation ein Kind dieser Transformation ist.
        /// </summary>
        /// <param name="depth">Die maximal zu suchende Tiefe.</param>
        /// <returns>True, wenn die Transformation gefunden wurde, ansonsten false.</returns>
        public bool ContainsChild(Transform transform, int depth = -1)
        {
            if (depth == 0)
                return false;

            lock (children)
            {
                foreach (var child in children)
                {
                    if (child == transform)
                        return true;
                    if (child.ContainsChild(transform, depth - 1))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gibt alle Kind-Transformationen mit der angegebenen maximalen Tiefe zurück.
        /// </summary>
        /// <param name="depth">Die maximale Tiefe eines Kind-Elements.</param>
        /// <returns>Die Liste aller gefundenen Kind-Elemente.</returns>
        public Transform[] GetChildren(int depth = -1)
        {
            if (depth < 0)
                lock (children)
                    return children.ToArray();

            var list = new List<Transform>();
            GetChildren(list, depth);

            return list.ToArray();
        }

        private void GetChildren(List<Transform> list, int depth)
        {
            if (depth == 0)
                return;

            lock (children)
            {
                foreach (var child in children)
                    child.GetChildren(list, depth - 1);
            }
        }

        private void AddChild(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");
            if (transform == this)
                throw new InvalidOperationException("Transforms can not add themselves as a child.");
            if (ContainsChild(transform))
                throw new InvalidOperationException("Adding the given transform as a child would create a loop.");

            lock (children)
                children.Add(transform);
            transform.parent = this;
        }

        private void RemoveChild(Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            lock (children)
                children.Remove(transform);
            transform.parent = null;
        }

        private void SetParent(Transform parent)
        {
            if (this.parent == parent)
                return;

            if (this.parent != null)
                this.parent.RemoveChild(this);

            if (parent != null)
            {
                Entity.Scene.RemoveChild(Entity);
                parent.AddChild(this);
            }
            else
            {
                Entity.Scene.AddChild(Entity);
            }

            isDirty = true;
        }

        protected void MarkDirty()
        {
            isDirty = true;
            lock (children)
            {
                foreach (var child in children)
                    child.MarkDirty();
            }
        }

        private void EnsureNotDirty()
        {
            if (!isDirty)
                return;

            matrix = Matrix4x4.CreateTranslation(localPosition)
                    * Matrix4x4.CreateFromQuaternion(localRotation)
                    * Matrix4x4.CreateScale(localScale);
            if (parent != null)
                matrix = parent.Matrix * matrix;

            Matrix4x4.Decompose(matrix, out scale, out rotation, out position);
            isDirty = false;
        }
    }
}
