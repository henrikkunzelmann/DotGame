using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Cameras;

namespace DotGame.Test
{
    public class TestCamera : EngineObject, ICamera
    {
        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(new Vector3(0, 0, 5f), new Vector3(0, 0, 0), Vector3.UnitY);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathHelper.PI / 4f, Engine.GraphicsDevice.DefaultWindow.Width / (float)Engine.GraphicsDevice.DefaultWindow.Height, 0.1f, 100.0f);
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
