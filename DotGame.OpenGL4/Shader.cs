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

        public IConstantBuffer CreateConstantBuffer(string name)
        {
            ConstantBuffer constantBuffer = new ConstantBuffer((GraphicsDevice)GraphicsDevice);

            return constantBuffer;
        }

        public void SetConstantBuffer(string name, IConstantBuffer buffer)
        {
            // TODO (Robin) Durch GraphicsDevice Methode ersetzen
            graphicsDevice.StateMachine.Shader = this;

            ConstantBuffer internalBuffer = graphicsDevice.Cast<ConstantBuffer>(buffer, "buffer");

            if (!uniformBufferLocations.ContainsKey(name))
            {
                int blockIndex = GL.GetUniformBlockIndex(ProgramID, name);
                int bindingPoint = uniformBufferLocations.Count;
                GL.UniformBlockBinding(ProgramID, blockIndex, bindingPoint);
                uniformBufferLocations[name] = bindingPoint;
            }

            graphicsDevice.StateMachine.ConstantBuffer = buffer;
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, uniformBufferLocations[name], internalBuffer.UniformBufferObjectID);
            OpenGL4.GraphicsDevice.CheckGLError();
        }


        public IConstantBuffer CreateConstantBuffer()
        {
            return new ConstantBuffer((GraphicsDevice)GraphicsDevice);
        }

        public void SetConstantBuffer(IConstantBuffer buffer)
        {
            SetConstantBuffer("global", buffer);
        }


        public void SetTexture(string name, ITexture2D texture)
        {
            GL.ActiveTexture(TextureUnit.Texture0);

            Texture2D internalTexture = graphicsDevice.Cast<Texture2D>(texture, "texture");
            GL.BindTexture(TextureTarget.Texture2D, internalTexture.TextureID);

            // TODO (Robin) Texture setzen, das ist nur ein Test
        }

        public void SetTexture(string name, ITexture2DArray texture)
        {
            throw new NotImplementedException();
        }

        public void SetTexture(string name, ITexture3D texture)
        {
            throw new NotImplementedException();
        }

        public void SetTexture(string name, ITexture3DArray texture)
        {
            throw new NotImplementedException();
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
