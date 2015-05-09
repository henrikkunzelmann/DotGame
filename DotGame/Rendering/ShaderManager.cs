using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    public class ShaderManager : EngineObject
    {
        public ShaderManager(Engine engine)
            : base(engine)
        {

        }

        public IShader CompileShader(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (Engine.Settings.GraphicsAPI == GraphicsAPI.Direct3D11)
                return Engine.GraphicsDevice.Factory.CompileShader(name, new ShaderCompileInfo(name + ".fx", "VS", "vs_4_0"), new ShaderCompileInfo(name + ".fx", "PS", "ps_4_0"));
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                return Engine.GraphicsDevice.Factory.CompileShader(name, new ShaderCompileInfo(name + ".vertex.glsl", "", "vs_4_0"), new ShaderCompileInfo(name + ".fragment.glsl", "", "ps_4_0"));
            else
                throw new NotImplementedException();
        }

        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
