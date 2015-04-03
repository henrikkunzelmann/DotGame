using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class ShaderPart : IDisposable
    {
        internal int ID { get; private set; }

        internal ShaderPart(string shaderSource, ShaderType shaderType)
        {
            //Comile and attach Vertex shader
            ID = GL.CreateShader(shaderType);
            GL.ShaderSource(ID, shaderSource);
            GL.CompileShader(ID);            
            int vertexShaderStatus;
            GL.GetShader(ID, ShaderParameter.CompileStatus, out vertexShaderStatus);
            if (vertexShaderStatus == 0)
            {
                throw new Exception(GL.GetShaderInfoLog(ID));
            }
            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public void Dispose()
        {
            GL.DeleteShader(ID);
        }
    }
}
