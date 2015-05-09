using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt ein Entity dar.
    /// </summary>
    public class Entity : SceneNode
    {
        private bool worldDirty = true;

        private Matrix4x4 world;

        private Vector3 position;
        private Vector3 scale = Vector3.One;
        private Quaternion rotation = Quaternion.Identity;

        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    worldDirty = true;
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
                    worldDirty = true;
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
                    worldDirty = true;
                }
            }
        }

        public override Matrix4x4 LocalWorld
        {
            get
            {
                if (worldDirty)
                {
                    world = Matrix4x4.CreateFromQuaternion(rotation) * Matrix4x4.CreateScale(scale) * Matrix4x4.CreateTranslation(position);
                    worldDirty = true;
                }
                return world;
            }
        }

        public Entity(Scene scene, string name)
            : base(scene, name)
        {

        }

        public Entity(SceneNode parent, string name)
            : base(parent, name)
        {

        }
    }
}