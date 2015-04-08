using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Gibt an wie ein Buffer benutzt wird.
    /// </summary>
    public enum BufferUsage
    {
        /// <summary>
        /// Am besten geeignet für Buffer die sich nicht oder nicht oft ändern. 
        /// </summary>
        Static, 

        /// <summary>
        /// Am besten geeignet für Buffer die sich oft ändern.
        /// </summary>
        Dynamic
    }
}
