using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    public interface IAudioListener
    {
        IAudioDevice AudioDevice { get; }

        float Gain { get; set; }
        Vector3 Position { get; set; }
        Vector3 Velocity { get; set; }
        Vector3 Up { get; set; }
        Vector3 At { get; set; }

        void Orientation(Vector3 Up, Vector3 At);
    }
}
