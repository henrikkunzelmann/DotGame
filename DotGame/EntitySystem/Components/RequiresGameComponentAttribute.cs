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
    public class RequiresGameComponentAttribute : Attribute
    {
        /// <summary>
        /// Der Typ, der von der Komponente voraussgesetzt wird.
        /// </summary>
        public readonly Type GameComponentType;

        /// <summary>
        /// Gibt an, dass die Komponente eine andere voraussetzt.
        /// <param name="componentType">Der Typ, der von der Komponente voraussgesetzt wird.</param>
        /// </summary>
        public RequiresGameComponentAttribute(Type gameComponentType)
        {
            if (gameComponentType == null)
                throw new ArgumentNullException("componentType");
            if (!gameComponentType.IsSubclassOf(typeof(EngineComponent)))
                throw new ArgumentException("Given type must be a subtype of Component.", "componentType");

            this.GameComponentType = gameComponentType;
        }
    }
}
