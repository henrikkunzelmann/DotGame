using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    public interface ISoundInstance : IAudioObject, IEquatable<ISoundInstance>
    {
        ISound Sound { get; }

        Vector3 Position { get; set; }
        Vector3 Velocity { get; set; }
        float Gain { get; set; }
        float Pitch { get; set; }
        bool IsLooping { get; set; }

        void Route(int slot, IMixerChannel route);

        void Play();
        void Pause();
        void Stop();
    }
}
