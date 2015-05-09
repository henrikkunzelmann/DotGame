using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Assets
{
    /// <summary>
    /// Gibt die verschiedenen Asset-Typen an.
    /// </summary>
    public enum AssetType
    {
        /// <summary>
        /// Das Asset wurde von einer Datei geladen.
        /// </summary>
        File, 
        
        /// <summary>
        /// Das Asset wurde vom Benutzer während der Laufzeit erstellt.
        /// </summary>
        User, 
        
        /// <summary>
        /// Das Asset stellt einen Wrapper für eine interne Ressource dar.
        /// </summary>
        Wrapper
    }
}
