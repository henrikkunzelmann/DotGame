using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    public interface ISampleSource : IAudioObject
    {
        long TotalSamples { get; }
        long Position { get; set; }
        AudioFormat NativeFormat { get; }
        int Channels { get; }
        int SampleRate { get; }

        float[] ReadSamples(int count);
        void ReadSamples(float[] buffer, int offset, int count);
        float[] ReadAll();
        void ReadAll(float[] buffer, int offset);
    }
}
