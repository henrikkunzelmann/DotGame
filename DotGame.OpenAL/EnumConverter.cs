using DotGame.Audio;
using OpenTK.Audio.OpenAL;
using System;

namespace DotGame.OpenAL
{
    internal static class EnumConverter
    {
        public static ALFormat GetFormat(AudioFormat format, int channels)
        {
            switch (format)
            {
                case AudioFormat.Byte8:
                    if (channels == 1)
                        return ALFormat.Mono8;
                    else if (channels == 2)
                        return ALFormat.Stereo8;
                    break;
                case AudioFormat.Short16:
                    if (channels == 1)
                        return ALFormat.Mono16;
                    else if (channels == 2)
                        return ALFormat.Stereo16;
                    break;
                case AudioFormat.Float32:
                    if (channels == 1)
                        return ALFormat.MonoFloat32Ext;
                    else if (channels == 2)
                        return ALFormat.StereoFloat32Ext;
                    break;
            }

            throw new NotImplementedException("Format {0} with {1} channels is not supported!");
        }
    }
}
