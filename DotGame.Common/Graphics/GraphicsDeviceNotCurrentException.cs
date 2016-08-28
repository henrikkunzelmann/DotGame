using System;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt eine Ausnahme dar, die ausgelöst wird, wenn das GraphicsDevice nicht im aktuellen Thread verfügbar ist.
    /// </summary>
    public class GraphicsDeviceNotCurrentException : Exception
    {
        /// <summary>
        /// Das GraphicsDevice, welches die Ausnahme ausgelöst hat.
        /// </summary>
        public readonly IGraphicsDevice GraphicsDevice;

        /// <summary>
        /// Erstellt eine neue Instanz der GraphicsDeviceNotCurrentException-Klasse.
        /// </summary>
        /// <param name="graphicsDevice">Das GraphicsDevice, welches die Ausnahme ausgelöst hat.</param>
        public GraphicsDeviceNotCurrentException(IGraphicsDevice graphicsDevice) : base(string.Format("GraphicsDevice {0} is not current on calling thread.", graphicsDevice))
        {
            this.GraphicsDevice = graphicsDevice;
        }
    }
}
