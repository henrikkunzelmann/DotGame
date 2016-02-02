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
        public Transform Parent { get { return Entity.Parent?.Transform; }}

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

        public override void Destroy()
        {
            base.Destroy();
        }

        public void MarkDirty()
        {
            isDirty = true;
            if (Entity != null)
            {
                var children = Entity.Children;
                foreach (var child in children)
                    child.Transform.MarkDirty();
            }
        }

        private void EnsureNotDirty()
        {
            if (!isDirty)
                return;

            matrix = Matrix4x4.CreateTranslation(localPosition)
                    * Matrix4x4.CreateFromQuaternion(localRotation)
                    * Matrix4x4.CreateScale(localScale);
            if (Parent != null)
                matrix = matrix * Parent.Matrix;

            Matrix4x4.Decompose(matrix, out scale, out rotation, out position);
            isDirty = false;
        }
    }
}
