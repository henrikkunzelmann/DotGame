using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.EntitySystem.Components
{
    /// <summary>
    /// Gibt an, dass die Komponente eine andere voraussetzt.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresComponentAttribute : Attribute
    {
        /// <summary>
        /// Der Typ, der von der Komponente voraussgesetzt wird.
        /// </summary>
        public readonly Type ComponentType;

        /// <summary>
        /// Gibt an, dass die Komponente eine andere voraussetzt.
        /// <param name="componentType">Der Typ, der von der Komponente voraussgesetzt wird.</param>
        /// </summary>
        public RequiresComponentAttribute(Type componentType)
        {
            if (componentType == null)
                throw new ArgumentNullException("componentType");
            if (!componentType.IsSubclassOf(typeof(Component)))
                throw new ArgumentException("Given type must be a subtype of Component.", "componentType");

            this.ComponentType = componentType;
        }
    }
}
