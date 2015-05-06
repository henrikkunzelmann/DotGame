using DotGame.Cameras;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    /// <summary>
    /// Stellt eine Kamera dar.
    /// </summary>
    public class Camera : Component, ICamera
    {
        private static Vector3 lookat = new Vector3(0, 0, 1);

        /// <summary>
        /// Ruft die View-Matrix dieser Kamera ab.
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 View
        {
            get
            {
                var mat = Matrix4x4.CreateFromQuaternion(Entity.Transform.Rotation);

                return Matrix4x4.CreateLookAt(Entity.Transform.Position, Entity.Transform.Position
                    + Vector3.TransformNormal(lookat, mat),
                    Vector3.TransformNormal(Up, mat));
            }
        }

        /// <summary>
        /// Ruft die Projektions-Matrix dieser Kamera ab.
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 Projection { get { return Matrix4x4.CreatePerspectiveFieldOfView(Fov, AspectRatio, ZNear, ZFar); } }

        /// <summary>
        /// Der Up-Vektor dieser Kamera.
        /// </summary>
        public Vector3 Up = Vector3.UnitY;

        /// <summary>
        /// Das Sichtfeld der Kamera im Bogenmaß.
        /// </summary>
        public float Fov = (float)Math.PI / 2;

        /// <summary>
        /// Das Sichtfeld der Kamera in Grad.
        /// </summary>
        [JsonIgnore]
        public float FovDegrees { get { return Fov * MathHelper.RADIANS_TO_DEGREES; } set { Fov = value * MathHelper.DEGREES_TO_RADIANS; } }

        /// <summary>
        /// Das Seitenverhältnis der Kamera (Breite / Höhe).
        /// </summary>
        public float AspectRatio = 16.0f / 9.0f;
        
        /// <summary>
        /// Der Abstand zur Near-Plane. 
        /// </summary>
        public float ZNear = 0.1f;

        /// <summary>
        /// Der Abstand zur Far-Plane. 
        /// </summary>
        public float ZFar = 100.0f;
    }
}
