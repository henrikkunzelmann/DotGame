namespace DotGame.Graphics
{
    /// <summary>
    /// Ein bestimmer Bereich in dem die Engine zeichnen kann, z.B. ein normales Fenster oder ein Teil davon.
    /// </summary>
    public interface IGameWindow
    {
        /// <summary>
        /// Stellt die Breite des Fensters dar.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Stellt die Höhe des Fensters dar.
        /// </summary>
        int Height { get; set; }

        bool IsFullScreen { get; set; }

        /// <summary>
        /// Erstellt ein GraphicsDevice für diese GameWindow.
        /// </summary>
        /// <returns></returns>
        IGraphicsDevice CreateDevice(DeviceCreationFlags flags);
    }
}
