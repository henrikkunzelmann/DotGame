using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace DotGame.SceneGraph
{
    /// <summary>
    /// Stellt eine Kamera dar.
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Gibt die Position der Kamera zurück oder setzt diese.
        /// </summary>
        Vector3 Position { get; set; }
        
        /// <summary>
        /// Gibt die View-Matrix der Kamera zurück.
        /// </summary>
        Matrix4x4 View { get; }

        /// <summary>
        /// Gibt die Projection-Matrix der Kamera zurück.
        /// </summary>
        Matrix4x4 Projection { get; }
    }
}
