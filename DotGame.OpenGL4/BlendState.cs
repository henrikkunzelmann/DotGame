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
    public class BlendState : GraphicsObject, IBlendState
    {
        public BlendStateInfo Info { get; private set; }

        public BlendState(GraphicsDevice graphicsDevice, BlendStateInfo info)
            : base(graphicsDevice, new StackTrace(1))
        {
            this.Info = info;
        }

        internal void Apply()
        {
            Apply(null);
        }

        internal void Apply(IBlendState compareTo)
        {
            //AlphaToCoverage nicht unterstützt
            //IndependantBlend nicht unterstützt

            if (compareTo != null)
            {
                if (Info.RenderTargets != null)
                {
                    //States jedes einzelnen RenderTargets setzen
                    for (int i = 0; i < Info.RenderTargets.Length; i++)
                    {
                        bool compareRenderTargetExists = compareTo.Info.RenderTargets != null && compareTo.Info.RenderTargets.Length > i;

                        if (!compareRenderTargetExists || Info.RenderTargets[i].IsBlendEnabled != compareTo.Info.RenderTargets[i].IsBlendEnabled)
                        {
                            if (Info.RenderTargets[i].IsBlendEnabled)
                                GL.Enable(IndexedEnableCap.Blend, i);
                            else
                                GL.Disable(IndexedEnableCap.Blend, i);
                        }

                        if (graphicsDevice.GLSLVersionMajor >= 4)
                        {
                            //BlendOperation und Blend für jedes RT einzeln setzen

                            if ((!compareRenderTargetExists || (compareTo.Info.RenderTargets[i].SrcBlend != Info.RenderTargets[i].SrcBlend || compareTo.Info.RenderTargets[i].DestBlend != Info.RenderTargets[i].DestBlend || compareTo.Info.RenderTargets[i].SrcBlendAlpha != Info.RenderTargets[i].SrcBlendAlpha || compareTo.Info.RenderTargets[i].DestBlendAlpha != Info.RenderTargets[i].DestBlendAlpha)))
                                GL.BlendFuncSeparate(i, EnumConverter.Convert(Info.RenderTargets[i].SrcBlend), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[i].DestBlend), EnumConverter.Convert(Info.RenderTargets[i].SrcBlendAlpha), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[i].DestBlendAlpha));

                            if (graphicsDevice.GLSLVersionMajor >= 4 && (!compareRenderTargetExists || (Info.RenderTargets[i].BlendOp != compareTo.Info.RenderTargets[i].BlendOp || Info.RenderTargets[i].BlendOpAlpha != compareTo.Info.RenderTargets[i].BlendOpAlpha)))
                                GL.BlendEquationSeparate(i, EnumConverter.Convert(Info.RenderTargets[i].BlendOp), EnumConverter.Convert(Info.RenderTargets[i].BlendOpAlpha));
                        }
                        else
                        {
                            //BlendOperation und Blend setzen

                            compareRenderTargetExists = compareTo.Info.RenderTargets != null && compareTo.Info.RenderTargets.Length != 0;
                            if (compareRenderTargetExists || (compareTo.Info.RenderTargets[0].SrcBlend != Info.RenderTargets[0].SrcBlend && compareTo.Info.RenderTargets[0].DestBlend != Info.RenderTargets[0].DestBlend && compareTo.Info.RenderTargets[0].SrcBlendAlpha != Info.RenderTargets[0].SrcBlendAlpha && compareTo.Info.RenderTargets[0].DestBlendAlpha != Info.RenderTargets[0].DestBlendAlpha))
                                GL.BlendFuncSeparate(EnumConverter.Convert(Info.RenderTargets[0].SrcBlend), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[0].DestBlend), EnumConverter.Convert(Info.RenderTargets[0].SrcBlendAlpha), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[0].DestBlendAlpha));


                            if (!compareRenderTargetExists || (Info.RenderTargets[i].BlendOp != compareTo.Info.RenderTargets[0].BlendOp || Info.RenderTargets[0].BlendOpAlpha != compareTo.Info.RenderTargets[0].BlendOpAlpha))
                                GL.BlendEquationSeparate(EnumConverter.Convert(Info.RenderTargets[0].BlendOp), EnumConverter.Convert(Info.RenderTargets[0].BlendOpAlpha));
                        }

                        if (!compareRenderTargetExists || compareTo.Info.RenderTargets[i].RenderTargetWriteMask != Info.RenderTargets[i].RenderTargetWriteMask)
                            GL.ColorMask(i, Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Red), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Blue), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Green), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Alpha));
                    }
                }
            }
            else
            {
                if (Info.RenderTargets != null)
                {
                    //States jedes einzelnen RenderTargets setzen
                    for (int i = 0; i < Info.RenderTargets.Length; i++)
                    {
                        if (Info.RenderTargets[i].IsBlendEnabled)
                            GL.Enable(IndexedEnableCap.Blend, i);
                        else
                            GL.Disable(IndexedEnableCap.Blend, i);

                        if (graphicsDevice.GLSLVersionMajor >= 4)
                        {
                            //BlendOperation und Blend für jedes RT einzeln setzen

                            GL.BlendFuncSeparate(i, EnumConverter.Convert(Info.RenderTargets[i].SrcBlend), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[i].DestBlend), EnumConverter.Convert(Info.RenderTargets[i].SrcBlendAlpha), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[i].DestBlendAlpha));
                            GL.BlendEquationSeparate(i, EnumConverter.Convert(Info.RenderTargets[i].BlendOp), EnumConverter.Convert(Info.RenderTargets[i].BlendOpAlpha));
                        }

                        GL.ColorMask(i, Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Red), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Blue), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Green), Info.RenderTargets[i].RenderTargetWriteMask.HasFlag(ColorWriteMaskFlags.Alpha));
                    }
                    if (graphicsDevice.GLSLVersionMajor < 4 && Info.RenderTargets != null && Info.RenderTargets.Length > 0)
                    {
                        //BlendOperation und Blend setzen

                        GL.BlendFuncSeparate(EnumConverter.Convert(Info.RenderTargets[0].SrcBlend), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[0].DestBlend), EnumConverter.Convert(Info.RenderTargets[0].SrcBlendAlpha), (BlendingFactorDest)EnumConverter.Convert(Info.RenderTargets[0].DestBlendAlpha));
                        GL.BlendEquationSeparate(EnumConverter.Convert(Info.RenderTargets[0].BlendOp), EnumConverter.Convert(Info.RenderTargets[0].BlendOpAlpha));
                    }
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
