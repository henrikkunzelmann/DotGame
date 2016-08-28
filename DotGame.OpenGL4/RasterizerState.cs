using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace DotGame.OpenGL4
{
    internal class RasterizerState : GraphicsObject, IRasterizerState
    {
        public RasterizerStateInfo Info { get; private set; }

        internal RasterizerState(GraphicsDevice graphicsdevice, RasterizerStateInfo info)
            :base(graphicsdevice, new StackTrace(1))
        {
            this.Info = info;
        }

        internal void Apply()
        {
            Apply(null);
        }

        internal void Apply(IRasterizerState compareTo)
        {
            if (compareTo != null)
            {
                if (compareTo.Info.FillMode != Info.FillMode)
                    GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(Info.FillMode));

                if (compareTo.Info.CullMode != Info.CullMode)
                {
                    if (Info.CullMode != CullMode.None)
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(EnumConverter.Convert(Info.CullMode));
                    }
                    else
                        GL.Disable(EnableCap.CullFace);
                }

                if (compareTo.Info.IsFrontCounterClockwise != Info.IsFrontCounterClockwise)
                    GL.FrontFace(Info.IsFrontCounterClockwise ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw);

                if (compareTo.Info.IsMultisampleEnabled != Info.IsMultisampleEnabled)
                {
                    if (Info.IsMultisampleEnabled)
                    {
                        GL.Enable(EnableCap.Multisample);
                        GL.Enable(EnableCap.PolygonSmooth);
                    }
                    else
                    {
                        GL.Disable(EnableCap.Multisample);
                        GL.Disable(EnableCap.PolygonSmooth);
                    }
                }

                if (compareTo.Info.IsScissorEnabled != Info.IsScissorEnabled)
                {
                    if (Info.IsScissorEnabled)
                        GL.Enable(EnableCap.ScissorTest);
                    else
                        GL.Disable(EnableCap.ScissorTest);
                }

                if (compareTo.Info.IsAntialiasedLineEnabled != Info.IsAntialiasedLineEnabled)
                {
                    if (Info.IsAntialiasedLineEnabled)
                        GL.Enable(EnableCap.LineSmooth);
                    else
                        GL.Disable(EnableCap.LineSmooth);
                }

                if (compareTo.Info.DepthBias != Info.DepthBias || compareTo.Info.SlopeScaledDepthBias != Info.SlopeScaledDepthBias)
                {
                    GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(Info.FillMode));
                    GL.PolygonOffset(Info.DepthBias, Info.SlopeScaledDepthBias);
                }
            }
            else
            {
                //Nothing to compare to
                //Set all states
                GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(Info.FillMode));

                if (Info.CullMode != CullMode.None)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(EnumConverter.Convert(Info.CullMode));
                }
                else
                    GL.Disable(EnableCap.CullFace);

                GL.FrontFace(Info.IsFrontCounterClockwise ? FrontFaceDirection.Ccw : FrontFaceDirection.Cw);

                if (Info.IsMultisampleEnabled)
                {
                    GL.Enable(EnableCap.Multisample);
                    GL.Enable(EnableCap.PolygonSmooth);
                }
                else
                {
                    GL.Disable(EnableCap.Multisample);
                    GL.Disable(EnableCap.PolygonSmooth);
                }

                if (Info.IsScissorEnabled)
                    GL.Enable(EnableCap.ScissorTest);
                else
                    GL.Disable(EnableCap.ScissorTest);

                if (Info.IsAntialiasedLineEnabled)
                    GL.Enable(EnableCap.LineSmooth);
                else
                    GL.Disable(EnableCap.LineSmooth);

                GL.PolygonMode(MaterialFace.FrontAndBack, EnumConverter.Convert(Info.FillMode));

                GL.PolygonOffset(Info.DepthBias, Info.SlopeScaledDepthBias);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            //Nothing to dispose
        }
    }
}
