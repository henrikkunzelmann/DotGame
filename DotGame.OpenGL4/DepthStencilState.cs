using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    public class DepthStencilState : GraphicsObject, IDepthStencilState
    {
        public DepthStencilStateInfo Info { get; private set; }
        private int depthStencilReference;

        public DepthStencilState(GraphicsDevice graphicsDevice, DepthStencilStateInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Info = info;
        }

        internal void Apply(int depthStencilReference)
        {
            Apply(null, depthStencilReference);
        }

        internal void Apply(IDepthStencilState compareTo, int depthStencilReference)
        {
            if (compareTo != null || this.depthStencilReference != depthStencilReference)
            {
                //Depth
                if (compareTo.Info.IsDepthEnabled != Info.IsDepthEnabled)
                {
                    if (Info.IsDepthEnabled)
                        GL.Enable(EnableCap.DepthTest);
                    else
                        GL.Disable(EnableCap.DepthTest);
                }

                if (compareTo.Info.DepthWriteMask != Info.DepthWriteMask)
                    GL.DepthMask(Info.DepthWriteMask == DepthWriteMask.All);


                if (compareTo.Info.DepthComparsion != Info.DepthComparsion)
                    GL.DepthFunc(EnumConverter.Convert(Info.DepthComparsion));

                //Stencil
                if (compareTo.Info.IsStencilEnabled != Info.IsStencilEnabled)
                {
                    if (Info.IsStencilEnabled)
                        GL.Enable(EnableCap.StencilTest);
                    else
                        GL.Disable(EnableCap.StencilTest);
                }

                if (compareTo.Info.StencilWriteMask != Info.StencilWriteMask)
                    GL.StencilMask(Info.StencilWriteMask);


                if (compareTo.Info.FrontFace.Comparsion != Info.FrontFace.Comparsion)
                {
                    GL.StencilFuncSeparate(StencilFace.Front, (StencilFunction)EnumConverter.Convert(Info.FrontFace.Comparsion), depthStencilReference, Info.StencilWriteMask);
                }
                if (compareTo.Info.FrontFace.DepthFailOperation != Info.FrontFace.DepthFailOperation || compareTo.Info.FrontFace.FailOperation != Info.FrontFace.FailOperation || compareTo.Info.FrontFace.PassOperation != Info.FrontFace.PassOperation)
                {
                    GL.StencilOpSeparate(StencilFace.Front, EnumConverter.Convert(Info.FrontFace.FailOperation), EnumConverter.Convert(Info.FrontFace.DepthFailOperation), EnumConverter.Convert(Info.FrontFace.PassOperation));
                }

                if (compareTo.Info.BackFace.Comparsion != Info.BackFace.Comparsion)
                {
                    GL.StencilFuncSeparate(StencilFace.Back, (StencilFunction)EnumConverter.Convert(Info.BackFace.Comparsion), depthStencilReference, Info.StencilWriteMask);
                }
                if (compareTo.Info.BackFace.DepthFailOperation != Info.BackFace.DepthFailOperation || compareTo.Info.BackFace.FailOperation != Info.BackFace.FailOperation || compareTo.Info.BackFace.PassOperation != Info.BackFace.PassOperation)
                {
                    GL.StencilOpSeparate(StencilFace.Back, EnumConverter.Convert(Info.BackFace.FailOperation), EnumConverter.Convert(Info.BackFace.DepthFailOperation), EnumConverter.Convert(Info.BackFace.PassOperation));
                }
            }
            else
            {
                //Depth
                if (Info.IsDepthEnabled)
                    GL.Enable(EnableCap.DepthTest);
                else
                    GL.Disable(EnableCap.DepthTest);

                GL.DepthMask(Info.DepthWriteMask == DepthWriteMask.All);

                GL.DepthFunc(EnumConverter.Convert(Info.DepthComparsion));

                //Stencil
                if (Info.IsStencilEnabled)
                    GL.Enable(EnableCap.StencilTest);
                else
                    GL.Disable(EnableCap.StencilTest);

                GL.StencilMask(Info.StencilWriteMask);

                GL.StencilFuncSeparate(StencilFace.Front, (StencilFunction)EnumConverter.Convert(Info.FrontFace.Comparsion), depthStencilReference, Info.StencilWriteMask);
                GL.StencilOpSeparate(StencilFace.Front, EnumConverter.Convert(Info.FrontFace.FailOperation), EnumConverter.Convert(Info.FrontFace.DepthFailOperation), EnumConverter.Convert(Info.FrontFace.PassOperation));
                GL.StencilFuncSeparate(StencilFace.Back, (StencilFunction)EnumConverter.Convert(Info.BackFace.Comparsion), depthStencilReference, Info.StencilWriteMask);
                GL.StencilOpSeparate(StencilFace.Back, EnumConverter.Convert(Info.BackFace.FailOperation), EnumConverter.Convert(Info.BackFace.DepthFailOperation), EnumConverter.Convert(Info.BackFace.PassOperation));
            }

            this.depthStencilReference = depthStencilReference;
        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
