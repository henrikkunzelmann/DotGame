using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Verwaltet Befehle an das IGraphicsDevice, wie die Befehle über die Pipeline oder Zeichen-Befehle.
    /// </summary>
    public interface IRenderContext : IGraphicsObject
    {
        void Update<T>(IConstantBuffer buffer, T data) where T : struct;

        /// <summary>
        /// Leert den Inhalt des aktuell gebundenen RenderTargets. Ist kein RenderTarget gebunden, wird der Backbuffer angesprochen.
        /// </summary>
        /// <param name="color">Die Farbe.</param>
        void Clear(Color color);

        /// <summary>
        /// Leert den Inhalt des aktuell gebundenen RenderTargets. Ist kein RenderTarget gebunden, wird der Backbuffer angesprochen.
        /// </summary>
        /// <param name="clearOptions">Die <see cref="ClearOptions"/>-Flags, die die angesprochenen Channel angeben.</param>
        /// <param name="color">Die Farbe für Farbchannel benutzt wird.</param>
        /// <param name="depth">Der Wert für den Tiefenchannel (standardmäßig 1).</param>
        /// <param name="stencil">Der Wert für den Stencilchannel (standardmäßig 0).</param>
        void Clear(ClearOptions clearOptions, Color color, float depth, int stencil);

        void SetRenderTarget(IRenderTarget2D color, IRenderTarget2D depth);
        void SetRenderTargetColor(IRenderTarget2D color);
        void SetRenderTargetDepth(IRenderTarget2D depth);

        /// <summary>
        /// Setzt den Shader in der Pipeline.
        /// </summary>
        /// <param name="shader"></param>
        void SetShader(IShader shader);

        /// <summary>
        /// Setzt den PrimitiveType in der Pipeline.
        /// </summary>
        /// <param name="type"></param>
        void SetPrimitiveType(PrimitiveType type);

        /// <summary>
        /// Setzt die Pipeline auf den neuen Zustand.
        /// </summary>
        /// <param name="state"></param>
        void SetState(IRenderState state);

        /// <summary>
        /// Setzt den VertexBuffer.
        /// </summary>
        /// <param name="vertexBuffer"></param>
        void SetVertexBuffer(IVertexBuffer vertexBuffer);

        /// <summary>
        /// Setzt den IndexBuffer.
        /// </summary>
        /// <param name="indexBuffer"></param>
        void SetIndexBuffer(IIndexBuffer indexBuffer);

        /// <summary>
        /// Setzt den aktuellen ConstantBuffer für alle globale Variablen.
        /// </summary>
        /// <param name="buffer"></param>
        void SetConstantBuffer(IShader shader, IConstantBuffer buffer);

        /// <summary>
        /// Setzt den aktuellen ConstantBuffer für die Variable name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buffer"></param>
        void SetConstantBuffer(IShader shader, string name, IConstantBuffer buffer);

        void SetTexture(IShader shader, string name, ITexture2D texture);
        void SetTexture(IShader shader, string name, ITexture2DArray texture);
        void SetTexture(IShader shader, string name, ITexture3D texture);
        void SetTexture(IShader shader, string name, ITexture3DArray texture);

        void SetSampler(IShader shader, string name, ISampler sampler);

        /// <summary>
        /// Zeichnet nicht indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl der Vertices im gesetzten VertexBuffer.
        /// </summary>
        void Draw();

        /// <summary>
        /// Zeichnet indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl aller Indices im gesetzten IndexBuffer.
        /// </summary>
        void DrawIndexed();
    }
}
