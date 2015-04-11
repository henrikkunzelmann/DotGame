using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;

namespace DotGame.Rendering
{
    public class ShaderManager : EngineObject, IDisposable
    {
        private Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

        public ShaderManager(Engine engine)
            : base(engine)
        {

        }

        public void RegisterShader(Shader shader)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");

            shaders.Add(shader.Name, shader);
        }

        public IShader CompileShader(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (Engine.Settings.GraphicsAPI == GraphicsAPI.DirectX11)
                return Engine.GraphicsDevice.Factory.CompileShader(name, new ShaderCompileInfo(name + ".fx", "VS", "vs_4_0"), new ShaderCompileInfo(name + ".fx", "PS", "ps_4_0"));
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
                return Engine.GraphicsDevice.Factory.CompileShader(name, new ShaderCompileInfo(name + ".vertex.glsl", "", "vs_4_0"), new ShaderCompileInfo(name + ".fragment.glsl", "", "ps_4_0"));
            else
                throw new NotImplementedException();
        }

        protected override void Dispose(bool isDisposing)
        {
            foreach (KeyValuePair<string, Shader> shader in shaders)
                shader.Value.Dispose();
        }
    }
}
