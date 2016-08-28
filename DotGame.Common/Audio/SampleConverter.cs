using System;

namespace DotGame.Audio
{
    /// <summary>
    /// Helferklasse zum Konvertieren von Sampledaten.
    /// </summary>
    public static class SampleConverter
    {
        /// <summary>
        /// Konvertiert die angegebenen 32bit Samples in 16bit Samples.
        /// </summary>
        /// <param name="samples">Die Quelle.</param>
        /// <param name="newSamples">Das Ziel.</param>
        /// <param name="channel">Der Channel, aus dem die gemultiplexten Samples geladen werden sollen.</param>
        /// <param name="channels">Die Anzahl an gemultiplexten Channeln.</param>
        public static void To16Bit(float[] samples, short[] newSamples, int channel, int channels)
        {
            if (samples == null)
                throw new ArgumentNullException("samples");
            if (newSamples == null)
                throw new ArgumentNullException("newSamples");
            if (channels <= 0)
                throw new ArgumentOutOfRangeException("channels", "channels must be > 0");
            if (newSamples.Length < samples.Length / channels)
                throw new ArgumentException("newSamples.Length must be >= samples.", "newSamples");
            if (channel < 0 || channel >= channels)
                throw new ArgumentOutOfRangeException("channel", "channel must be >= 0 and < channels.");

            for (int i = 0; i < samples.Length / channels; i++)
            {
                
                newSamples[i] = (short)(samples[channel + i * channels] * short.MaxValue);
            }
        }
    }
}
