using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class Shader : GraphicsObject, IShader
    {
        public string Name { get; private set; }
        public ShaderType Type { get; private set; }

        internal ShaderPart VertexShader { get; private set; }
        internal ShaderPart FragmentShader { get; private set; }
        internal int ProgramID { get; private set; }

        private Dictionary<string, int> uniformBufferLocations = new Dictionary<string, int>();
        private Dictionary<string, int> uniformLocations = new Dictionary<string, int>();

        internal Shader(GraphicsDevice device, string vertexShaderCode, string fragmentShaderCode)
            : base(device, new System.Diagnostics.StackTrace())
        {
            //Create Program
            ProgramID = GL.CreateProgram();

            //Comile and attach Vertex shader
            VertexShader = new ShaderPart(vertexShaderCode, ShaderType.VertexShader);
            GL.AttachShader(ProgramID, VertexShader.ID);

            //Comile and attach Fragment shader
            FragmentShader = new ShaderPart(fragmentShaderCode, ShaderType.FragmentShader);
            GL.AttachShader(ProgramID, FragmentShader.ID);

            //Link program
            GL.LinkProgram(ProgramID);
            int linkStatus;
            GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(ProgramID));
            }
            OpenGL4.GraphicsDevice.CheckGLError();

            //Validate program
            GL.ValidateProgram(ProgramID);
            int validateStatus;
            GL.GetProgram(ProgramID, GetProgramParameterName.ValidateStatus, out validateStatus);
            if (validateStatus == 0)
            {
                throw new Exception(GL.GetProgramInfoLog(ProgramID));
            }
            OpenGL4.GraphicsDevice.CheckGLError();


            FindUniforms();

            // TODO (Robin) Custom Exception
            // TODO (Robin) Im Fall einer Exception Shader freigeben
        }

        private void FindUniforms()
        {
            int count;
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveUniforms, out count);

            for (int i = 0; i < count; i++)
            {
                int length;
                StringBuilder nameBuilder = new StringBuilder(255);
                GL.GetActiveUniformName(ProgramID, i, 255, out length, nameBuilder);
                string name = nameBuilder.ToString(0, length);

                uniformLocations[name] = GL.GetUniformLocation(ProgramID, name);
            }
        }

        public IConstantBuffer CreateConstantBuffer()
        {
            return new ConstantBuffer(graphicsDevice, -1);
        }

        public IConstantBuffer CreateConstantBuffer(string name)
        {
            ConstantBuffer constantBuffer = new ConstantBuffer(graphicsDevice, -1);

            return constantBuffer;
        }

        internal int GetUniform(string name)
        {
            return uniformLocations[name];
        }

        internal int GetUniformBuffer(string name)
        {
            if (!uniformBufferLocations.ContainsKey(name))
            {
                int blockIndex = GL.GetUniformBlockIndex(ProgramID, name);
                int bindingPoint = uniformBufferLocations.Count;
                GL.UniformBlockBinding(ProgramID, blockIndex, bindingPoint);
                uniformBufferLocations[name] = bindingPoint;
                return bindingPoint;
            }
            return uniformBufferLocations[name];
        }

        protected override void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (!GraphicsDevice.IsDisposed)
            {
                GL.DetachShader(ProgramID, VertexShader.ID);
                VertexShader.Dispose();

                GL.DetachShader(ProgramID, FragmentShader.ID);
                FragmentShader.Dispose();

                GL.DeleteProgram(ProgramID);
            }
        }
    }
}
