using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenGL4.Windows
{
    public interface IWindowContainer
    {
        IWindowInfo WindowInfo { get; }
    }
}
