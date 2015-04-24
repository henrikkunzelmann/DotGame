using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Audio
{
    /// <summary>
    /// Kapselt Informationen über die verfügbaren Funktionen der AudioApi.
    /// </summary>
    public struct AudioCapabilities
    {
        /// <summary>
        /// Gibt an, ob die API die Efx-Extension unterstützt.
        /// </summary>
        public bool SupportsEfx;
    }
}
