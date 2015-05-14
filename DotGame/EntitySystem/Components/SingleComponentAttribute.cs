using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    /// <summary>
    /// Gibt an, dass die Komponente nur einmal in einem Entity vorkommen darf.
    /// </summary>
    public class SingleComponentAttribute : Attribute
    {
    }
}
