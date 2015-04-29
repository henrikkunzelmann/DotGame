using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Cameras;
using System.Numerics;

namespace DotGame.Test
{
    public class TestCamera : EngineObject, ICamera
    {
        public Matrix4x4 View
        {
            get
            {
                return Matrix4x4.CreateLookAt(new Vector3(0, 0, 5f), new Vector3(0, 0, 0), Vector3.UnitY);
            }
        }

        public Matrix4x4 Projection
        {
            get
            {
                return Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.PI / 4f, Engine.GraphicsDevice.DefaultWindow.Width / (float)Engine.GraphicsDevice.DefaultWindow.Height, 0.1f, 100.0f);
            }
        }

        public TestCamera(Engine engine)
            : base(engine)
        {

        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
