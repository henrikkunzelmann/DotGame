using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt das GraphicsDevice dar, üblicherweise die GPU.
    /// </summary>
    public interface IGraphicsDevice : IDisposable
    {
        /// <summary>
        /// Ruft einen Wert ab, der angibt, ob das Objekt verworfen wurde.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Ruft die GraphicsCapabilities ab, welche Informationen über unterstützte Funktionen geben.
        /// </summary>
        GraphicsCapabilities Capabilities { get; }

        /// <summary>
        /// Ruft die IGraphicsFactory ab, die zur Erzeugung neuer Grafikobjekte benutzt wird.
        /// </summary>
        IGraphicsFactory Factory { get; }

        /// <summary>
        /// Ruft den IRenderContext ab, der zu für das Senden von Befehlen an das IGraphicsDevice zuständig ist.
        /// </summary>
        IRenderContext RenderContext { get; }

        /// <summary>
        /// Das Fenster welches das GraphicsDevice nutzt.
        /// </summary>
        IGameWindow DefaultWindow { get; }

        /// <summary>
        /// Ruft die vertikale Synchronisation ab oder legt diese fest.
        /// </summary>
        bool VSync { get; set; }

        /// <summary>
        /// Setzt das GraphicsDevice auf den aktuellen Thread.
        /// </summary>
        void MakeCurrent();

        /// <summary>
        /// Trennt das GraphicsDevice vom aktuellen Thread.
        /// </summary>
        void DetachCurrent();

        /// <summary>
        /// Gibt die Größe in Bytes des TextureFormats zurück.
        /// </summary>
        /// <param name="format">Das TextureFormat.</param>
        /// <returns></returns>
        int GetSizeOf(TextureFormat format);

        /// <summary>
        /// Gibt die Größe in Bytes des VertexElementTypes zurück.
        /// </summary>
        /// <param name="format">Das VertexElementType.</param>
        /// <returns></returns>
        int GetSizeOf(VertexElementType format);

        /// <summary>
        /// Gibt die Größe in Bytes des IndexFormates zurück.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        int GetSizeOf(IndexFormat format);

        /// <summary>
        /// Gibt die Größe in Bytes der VertexDescription zurück.
        /// </summary>
        /// <param name="description">Die VertexDescription.</param>
        /// <returns></returns>
        int GetSizeOf(VertexDescription description);

        /// <summary>
        /// Tauscht den Backbuffer mit dem Frontbuffer, was zum Anzeigen des Bildes führt.
        /// </summary>
        void SwapBuffers();
    }
}
