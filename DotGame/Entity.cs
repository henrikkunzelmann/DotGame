using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public class Entity : SceneNode
    {
        private bool propertiesDirty = true;
        private Vector3 position = Vector3.Zero;
        private Vector3 rotationOrigin = Vector3.Zero;
        private Quaternion rotation;
        private Vector3 scale = Vector3.One;

        private Matrix matrix;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    propertiesDirty = true;
                }
            }
        }

        public Vector3 RotationOrigin
        {
            get { return rotationOrigin; }
            set
            {
                if (rotationOrigin != value)
                {
                    rotationOrigin = value;
                    propertiesDirty = true;
                }
            }
        }

        public Quaternion Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation != value)
                {
                    rotation = value;
                    propertiesDirty = true;
                }
            }
        }

        public Vector3 Scale
        {
            get { return scale; }
            set
            {
                if (scale != value)
                {
                    scale = value;
                    propertiesDirty = true;
                }
            }
        }

        public Entity(Engine engine, string name)
            : base(engine, name)
        {

        }

        public Matrix GetMatrix()
        {
            if (propertiesDirty)
            {
                // TODO (henrik1235) Andere Transformationen unterstützen
                matrix = Matrix.CreateTranslation(position);
                propertiesDirty = false;
            }
            return matrix;
        }
    }
}
