using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt eine Kamera da die in eine feste Richtung zeigt.
    /// </summary>
    public class FixedCamera : ICamera
    {
        private bool dirtyView = true;
        private bool dirtyProjection = true;

        private Matrix4x4 view;
        private Matrix4x4 projection;

        private Vector3 upVector = Vector3.UnitY;
        private Vector3 position = Vector3.Zero;
        private Vector3 lookAt = Vector3.UnitX;

        private float fieldOfView = MathHelper.PI / 2f;
        private float aspectRatio = 16f / 9f;
        private float nearPlane = 0.1f;
        private float farPlane = 100f;

        /// <summary>
        /// Gibt den Up-Vektor zurück oder setzt diesen.
        /// </summary>
        public Vector3 UpVector
        {
            get { return upVector; }
            set
            {
                if (value != upVector)
                {
                    upVector = value;
                    dirtyView = true;
                }
            }
        }

        /// <summary>
        /// Gibt die Position der Kamera zurück oder setzt diese.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set
            {
                if (value != position)
                {
                    position = value;
                    dirtyView = true;
                }
            }
        }

        /// <summary>
        /// Gibt den Punkt an in welche Richtung die Kamera zeigt oder setzt diesen.
        /// </summary>
        public Vector3 LookAt
        {
            get { return lookAt; }
            set
            {
                if (value != lookAt)
                {
                    lookAt = value;
                    dirtyView = true;
                }
            }
        }

        public Vector3 Direction
        {
            get
            {
                return Vector3.Normalize(LookAt - Position);
            }
            set
            {
                LookAt = Position + value;
            }
        }

        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                if (value != fieldOfView)
                {
                    fieldOfView = value;
                    dirtyProjection = true;
                }
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                if (value != aspectRatio)
                {
                    aspectRatio = value;
                    dirtyProjection = true;
                }
            }
        }

        public float NearPlane
        {
            get { return nearPlane; }
            set
            {
                if (value != nearPlane)
                {
                    nearPlane = value;
                    dirtyProjection = true;
                }
            }
        }

        public float FarPlane
        {
            get { return farPlane; }
            set
            {
                if (value != farPlane)
                {
                    farPlane = value;
                    dirtyProjection = true;
                }
            }
        }

        public Matrix4x4 View
        {
            get
            {
                if (dirtyView)
                {
                    view = Matrix4x4.CreateLookAt(position, lookAt, upVector);
                    dirtyView = false;
                }
                return view;
            }
        }

        public Matrix4x4 Projection
        {
            get
            {
                if (dirtyProjection)
                {
                    projection = Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlane, farPlane);
                    dirtyProjection = false;
                }
                return projection;
            }
        }
    }
}
