using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    /// <summary>
    /// Stellt ein Shaderprogramm für die Render-Pipeline dar. 
    /// </summary>
    public interface IShader : IGraphicsObject
    {
        /// <summary>
        /// Der Name des Shaders.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Erstellt einen passenden ConstantBuffer für alle globale Variablen.
        /// </summary>
        /// <returns></returns>
        IConstantBuffer CreateConstantBuffer();

        /// <summary>
        /// Erstellt für die Variable name im Shader einen passenden Constant-Buffer.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConstantBuffer CreateConstantBuffer(string name);
    }
}
