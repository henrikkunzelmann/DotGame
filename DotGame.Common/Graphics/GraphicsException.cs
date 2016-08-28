using System;

namespace DotGame.Graphics
{
    public class GraphicsException : Exception
    {
        public GraphicsException()
            : base()
        {

        }

        public GraphicsException(string message)
            : base(message)
        {

        }

        public GraphicsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
