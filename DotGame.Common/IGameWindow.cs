using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame
{
    public interface IGameWindow
    {
        int Width { get; set; }
        int Height { get; set; }

        void Run();
    }
}
