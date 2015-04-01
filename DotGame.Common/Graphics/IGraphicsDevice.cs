﻿using System;
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
        /// Gibt die Größe in Bytes der VertexDescription zurück.
        /// </summary>
        /// <param name="description">Die VertexDescription.</param>
        /// <returns></returns>
        int GetSizeOf(VertexDescription description);

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

        /// <summary>
        /// Tauscht den Backbuffer mit dem Frontbuffer, was zum Anzeigen des Bildes führt.
        /// </summary>
        void SwapBuffers();
    }
}
