using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class Sampler : GraphicsObject, ISampler
    {
        internal int SamplerID { get; private set; }

        public SamplerInfo Info { get; set; }

        internal Sampler(GraphicsDevice graphicsDevice, SamplerInfo info)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            Info = info;

            if ((Info.MagFilter == TextureFilter.Anisotropic || Info.MinFilter == TextureFilter.Anisotropic) && (!graphicsDevice.HasAnisotropicFiltering || Info.MaximumAnisotropy > graphicsDevice.MaxAnisotropicFiltering))
                throw new PlatformNotSupportedException("Anisotropic filtering is not supported at that level or at all.");

            if (Info.MaximumAnisotropy == 0)
                throw new Exception("MaximumAnisotropy must not be 0");

            if (Info.MipLodBias > graphicsDevice.MaxTextureLoDBias)
                throw new PlatformNotSupportedException("MipLoDBias is higher than max lod bias.");


            SamplerID = GL.GenSampler();
            graphicsDevice.StateManager.SetSampler(this, 0);

            //AddressMode
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureWrapS, (float)EnumConverter.Convert(Info.AddressU));
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureWrapT, (float)EnumConverter.Convert(Info.AddressV));
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureWrapR, (float)EnumConverter.Convert(Info.AddressW));
            OpenGL4.GraphicsDevice.CheckGLError();

            //Filtering
            Tuple<TextureMinFilter, TextureMagFilter> filter = EnumConverter.Convert(Info.MinFilter, Info.MagFilter, Info.MipFilter);
            TextureMinFilter min = (TextureMinFilter)filter.Item1;
            TextureMagFilter mag = (TextureMagFilter)filter.Item2;
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureMinFilter, (float)min);
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureMagFilter, (float)mag);
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureMaxAnisotropyExt, Info.MaximumAnisotropy);

            //Border color
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureBorderColor, new float[] { Info.BorderColor.R, Info.BorderColor.G, Info.BorderColor.B, Info.BorderColor.A });

            //Compare modes
            if (Info.Type == SamplerType.Comparison)
            {
                GL.SamplerParameter(SamplerID, SamplerParameterName.TextureCompareMode, (float)TextureCompareMode.CompareRefToTexture);
                GL.SamplerParameter(SamplerID, SamplerParameterName.TextureCompareFunc, (float)EnumConverter.Convert(Info.ComparisonFunction));
            }
            else
                GL.SamplerParameter(SamplerID, SamplerParameterName.TextureCompareMode, (float)TextureCompareMode.None);

            //LoD
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureMinLod, Info.MinimumLod);
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureMaxLod, Info.MaximumLod);
            GL.SamplerParameter(SamplerID, SamplerParameterName.TextureLodBias, Info.MipLodBias);

            OpenGL4.GraphicsDevice.CheckGLError();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
                GL.DeleteSampler(SamplerID);
        }
    }
}
