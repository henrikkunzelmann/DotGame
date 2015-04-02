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
        /// Zeichnet nicht indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl der Vertices im gesetzten VertexBuffer.
        /// </summary>
        void Draw();

        /// <summary>
        /// Zeichnet indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl aller Indices im gesetzten IndexBuffer.
        /// </summary>
        void DrawIndexed();
    }
}
