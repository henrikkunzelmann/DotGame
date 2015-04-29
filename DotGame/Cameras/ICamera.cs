using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.Cameras
{
    public interface ICamera
    {
        Matrix4x4 View { get; }
        Matrix4x4 Projection { get; }
    }
}
