using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class Shader : GraphicsObject, IShader
    {
        public string Name { get; private set; }
        public ShaderType Type { get; private set; }

        private byte[] binaryCode;
        public byte[] BinaryCode
        {
            get
            {
                if (!graphicsDevice.Capabilities.SupportsBinaryShaders)
                    throw new NotSupportedException("Binary shaders are not supported.");
                return (byte[])binaryCode.Clone();
            }
        }

        internal ShaderPart VertexShader { get; private set; }
        internal ShaderPart FragmentShader { get; private set; }
        internal int ProgramID { get; private set; }

        private Dictionary<string, int> uniformBindingPoints = new Dictionary<string, int>();
        private Dictionary<string, int> uniformBlockLocations = new Dictionary<string, int>();
        private Dictionary<string, int> uniformLocations = new Dictionary<string, int>();
        private Dictionary<string, int> textureUnits = new Dictionary<string, int>();

        internal Shader(GraphicsDevice device, string vertexShaderCode, string fragmentShaderCode)
            : base(device, new System.Diagnostics.StackTrace())
        {
            //Create Program
            ProgramID = GL.CreateProgram();

            //Comile and attach Vertex shader
            VertexShader = new ShaderPart(graphicsDevice, vertexShaderCode, ShaderType.VertexShader);
            GL.AttachShader(ProgramID, VertexShader.ID);

            //Comile and attach Fragment shader
            FragmentShader = new ShaderPart(graphicsDevice, fragmentShaderCode, ShaderType.FragmentShader);
            GL.AttachShader(ProgramID, FragmentShader.ID);

            //Link program
            GL.LinkProgram(ProgramID);
            int linkStatus;
            GL.ProgramParameter(ProgramID, ProgramParameterName.ProgramBinaryRetrievableHint, 1);
            GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out linkStatus);
            if (linkStatus == 0)
                throw new GraphicsException(GL.GetProgramInfoLog(ProgramID));

            graphicsDevice.CheckGLError();

            if (graphicsDevice.Capabilities.SupportsBinaryShaders)
            {
                int binaryLength;
                GL.GetProgram(ProgramID, (GetProgramParameterName)0x8741, out binaryLength); // GL_PROGRAM_BINARY_LENGTH
                graphicsDevice.CheckGLError();

                BinaryFormat format;
                int length;
                byte[] array = new byte[binaryLength];
                GL.GetProgramBinary(ProgramID, binaryLength, out length, out format, array);
                graphicsDevice.CheckGLError();

                using (MemoryStream stream = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write("OPENGL4");
                    writer.Write((int)format);
                    writer.Write(array.Length);
                    writer.Write(array);
                    binaryCode = stream.GetBuffer();
                }
            }

            FindUniforms();
            FindUniformBlocks();

            // TODO (Robin) Custom Exception
            // TODO (Robin) Im Fall einer Exception Shader freigeben
        }

        internal Shader(GraphicsDevice graphicsDevice, byte[] binaryCode)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
            this.binaryCode = binaryCode;

            ProgramID = GL.CreateProgram();

            using (MemoryStream stream = new MemoryStream(binaryCode))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (reader.ReadString() != "OPENGL4")
                    throw new ArgumentException("Invalid header of binary code.", "binaryCode");
                int format = reader.ReadInt32();
                int codeSize = reader.ReadInt32();
                byte[] code = reader.ReadBytes(codeSize);

                GL.ProgramBinary(ProgramID, (BinaryFormat)format, code, code.Length);

                int linkStatus;
                GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out linkStatus);
                if (linkStatus == 0)
                    throw new GraphicsException(GL.GetProgramInfoLog(ProgramID));
                graphicsDevice.CheckGLError();
            }

            FindUniforms();
            FindUniformBlocks();
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

        private void FindUniformBlocks()
        {
            int count;
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveUniformBlocks, out count);

            for (int i = 0; i < count; i++)
            {
                int length;
                StringBuilder nameBuilder = new StringBuilder(255);
                GL.GetActiveUniformBlockName(ProgramID, i, 255, out length, nameBuilder);
                string name = nameBuilder.ToString(0, length);

                uniformBlockLocations[name] = GL.GetUniformBlockIndex(ProgramID, name);
            }
        }

        public IConstantBuffer CreateConstantBuffer(BufferUsage usage)
        {
            return graphicsDevice.Factory.CreateConstantBuffer(-1, usage);
        }

        public IConstantBuffer CreateConstantBuffer(string name, BufferUsage usage)
        {
            return graphicsDevice.Factory.CreateConstantBuffer(-1, usage);
        }

        /// <summary>
        /// Ruft die Location einer Uniform ab
        /// </summary>
        /// <param name="name">Name der Uniform</param>
        /// <returns></returns>
        internal int GetUniformLocation(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (!uniformLocations.ContainsKey(name))
                throw new ArgumentException(string.Format("Uniform {0} not found.", name), "name");

            return uniformLocations[name];
        }

        /// <summary>
        /// Ruft den BindingPoint des UniformBuffers + UniformBlocks ab oder verbindet diese
        /// </summary>
        /// <param name="name">Name des Uniform Blocks</param>
        /// <returns></returns>
        internal int GetUniformBlockBindingPoint(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            int bindingPoint;
            if (uniformBindingPoints.TryGetValue(name, out bindingPoint))
                return bindingPoint;

            int unifomBlock;
            if (!uniformBlockLocations.TryGetValue(name, out unifomBlock))
                throw new ArgumentException(string.Format("Uniform block {0} not found.", name), "name");

            graphicsDevice.BindManager.Shader = this;

            bindingPoint = uniformBindingPoints.Count;
            GL.UniformBlockBinding(ProgramID, unifomBlock, bindingPoint);

            uniformBindingPoints[name] = bindingPoint;

            return bindingPoint;
        }

        /// <summary>
        /// Ruft die TextureUnit der Texture ab oder weist der Texture eine TextureUnit zu
        /// </summary>
        /// <param name="name">Name der Texture Uniform</param>
        /// <returns></returns>
        internal int GetTextureUnit(string name)
        {
            int textureUnit;
            if (textureUnits.TryGetValue(name, out textureUnit))
                return textureUnit;

            textureUnit = textureUnits.Count;

            if (textureUnit > graphicsDevice.OpenGLCapabilities.TextureUnits - 1)
                throw new PlatformNotSupportedException("No more texture units available");

            if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.None)
            {
                graphicsDevice.BindManager.Shader = this;
                GL.Uniform1(GetUniformLocation(name), textureUnit);
            }
            else if (graphicsDevice.OpenGLCapabilities.DirectStateAccess == DirectStateAccess.Extension)
                OpenTK.Graphics.OpenGL.GL.Ext.ProgramUniform1(ProgramID, GetUniformLocation(name), textureUnit);

            textureUnits[name] = textureUnit;

            return textureUnit;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!GraphicsDevice.IsDisposed)
            {
                if (VertexShader != null)
                {
                    GL.DetachShader(ProgramID, VertexShader.ID);
                    VertexShader.Dispose();
                }

                if (FragmentShader != null)
                {
                    GL.DetachShader(ProgramID, FragmentShader.ID);
                    FragmentShader.Dispose();
                }

                GL.DeleteProgram(ProgramID);
            }
        }
    }
}
