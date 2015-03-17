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
        /// Ruft die IGraphicsFactory ab, die zur Erzeugung neuer Grafikobjekte benutzt wird.
        /// </summary>
        IGraphicsFactory Factory { get; }

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
        /// <param name="depth">Der Wert für den Tiefenchannel (standardmäßig 0).</param>
        /// <param name="stencil">Der Wert für den Stencilchannel (standardmäßig 0).</param>
        void Clear(ClearOptions clearOptions, Color color, float depth, int stencil);

        /// <summary>
        /// Tauscht den Backbuffer mit dem Frontbuffer, was zum Anzeigen des Bildes führt.
        /// </summary>
        void SwapBuffers();
    }
}
