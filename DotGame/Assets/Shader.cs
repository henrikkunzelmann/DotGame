using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotGame.Graphics;

namespace DotGame.Assets
{
    public class Shader : Asset
    {
        private IShader handle;
        public IShader Handle
        {
            get
            {
                MarkForUsage();
                return handle;
            }
        }

        private string vertexFunction;
        private string pixelFunction;
        private Version shaderModel;

        public VertexDescription VertexDescription
        {
            get
            {
                return handle?.VertexDescription;
            }
        }

        internal Shader(Engine engine, string name, string file, string vertexFunction, string pixelFunction, Version shaderModel) : base(engine, AssetType.File, name, file)
        {
            this.vertexFunction = vertexFunction;
            this.pixelFunction = pixelFunction;
            this.shaderModel = shaderModel;
        }

        private string GetShaderModelString(string shaderType, Version shaderModel)
        {
            return shaderType + "_" + shaderModel.Major.ToString() + "_" + shaderModel.Minor;
        }

        protected override void Load()
        {
            string binary = string.Empty;
            string vertexShaderPath = string.Empty;
            string pixelShaderPath = string.Empty;
            
            if (Engine.Settings.GraphicsAPI == GraphicsAPI.Direct3D11)
            {
                binary = Path.GetFileNameWithoutExtension(File) + ".opengl4.bin";
                vertexShaderPath = Path.GetFileNameWithoutExtension(File) + ".fx";
                pixelShaderPath = Path.GetFileNameWithoutExtension(File) + ".fx";
            }
            else if (Engine.Settings.GraphicsAPI == GraphicsAPI.OpenGL4)
            {
                binary = Path.GetFileNameWithoutExtension(File) + ".directx11.bin";
                vertexShaderPath = Path.GetFileNameWithoutExtension(File) + ".vertex.glsl";
                pixelShaderPath = Path.GetFileNameWithoutExtension(File) + ".fragment.glsl";
            }
            if (System.IO.File.Exists(binary))
            {
                byte[] binaryData = System.IO.File.ReadAllBytes(binary);
                handle = Engine.GraphicsDevice.Factory.CreateShader(Name, binaryData);
            }
            else
            {
                if (!System.IO.File.Exists(vertexShaderPath) || !System.IO.File.Exists(pixelShaderPath))
                    throw new Exception("Shader file could not be not found.");

                string vertexShader = System.IO.File.ReadAllText(vertexShaderPath);

                string pixelShader;
                if (pixelShaderPath == vertexShaderPath)
                    pixelShader = vertexShader;
                else
                    pixelShader = System.IO.File.ReadAllText(pixelShaderPath);

                handle = Engine.GraphicsDevice.Factory.CompileShader(File, new ShaderCompileInfo(vertexShader, "VS", GetShaderModelString("vs", shaderModel)), new ShaderCompileInfo(pixelShader, "PS", GetShaderModelString("ps", shaderModel)));
                if(handle.BinaryCode!= null)
                    System.IO.File.WriteAllBytes(binary, handle.BinaryCode);
            }

            if (handle == null)
                throw new Exception("Unable to load shader");
        }

        protected override void Unload()
        {
            handle?.Dispose();
        }
    }
}
