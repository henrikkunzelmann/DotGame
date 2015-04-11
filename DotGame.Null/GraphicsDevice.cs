using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Null
{
    /// <summary>
    /// GraphicsDevice welches keinerlei realle Wirkung hat, nützlich um z.B. nur mit dem Asset System zu arbeiten.
    /// </summary>
    public class GraphicsDevice : IGraphicsDevice
    {
        public bool IsDisposed { get; private set; }

        public IGraphicsFactory Factory
        {
            get { throw new NotSupportedException(); }
        }

        public IGameWindow DefaultWindow
        {
            get { throw new NotSupportedException(); }
        }

        public bool VSync { get; set; }

        public int GetSizeOf(TextureFormat format)
        {
            return 1;
        }

        public int GetSizeOf(VertexElementType type)
        {
            return 1;
        }

        public int GetSizeOf(VertexDescription description)
        {
            int size = 0;
            VertexElement[] elements = description.GetElements();
            for (int i = 0; i < elements.Length; i++)
                size += GetSizeOf(elements[i].Type);

            return size;
        }


        public int GetSizeOf(IndexFormat format)
        {
            return 1;
        }
  

        public void SwapBuffers()
        {
        }

        public void MakeCurrent() { }
        public void DetachCurrent() { }

        public IRenderContext RenderContext
        {
            get { throw new NotSupportedException(); }
        }


        public GraphicsCapabilities Capabilities
        {
            get { throw new NotSupportedException(); }
        }

        public void Dispose()
        {
            // wir haben nichts wirklich zu disposen
            IsDisposed = true;
        }
    }
}
