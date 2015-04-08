using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotGame.Graphics
{
    public enum ColorWriteMaskFlags
    {
        Red = 1,
        Green = 2, 
        Blue = 4, 
        Alpha = 8, 
        All = Red | Green | Blue | Alpha
    }
}
