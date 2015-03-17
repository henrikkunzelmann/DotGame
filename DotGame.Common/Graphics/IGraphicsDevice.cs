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

        void SwapBuffers();
    }
}
