using OpenTK.Graphics.OpenGL4;
using DotGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.OpenGL4
{
    /// <summary>
    /// Changes OpenGL states and prevents redundant changes.
    /// </summary>
    public class StateManager
    {
        private GraphicsDevice graphicsDevice;

        private IVertexBuffer currentVertexBuffer;
        private IIndexBuffer currentIndexBuffer;
        private IConstantBuffer currentConstantBuffer;
        private IShader currentShader;

        internal IVertexBuffer VertexBuffer 
        {
            get { return currentVertexBuffer; }
            set
            {
                if (value != currentVertexBuffer ) 
                {
                    currentVertexBuffer = value;
                    VertexBuffer internalVertexBuffer = graphicsDevice.Cast<VertexBuffer>(value, "value");

                    GL.BindVertexArray(internalVertexBuffer.VertexArrayObjectID);
                }
            } 
        }

        internal IIndexBuffer IndexBuffer 
        { 
            get { return currentIndexBuffer; } 
            set 
            {
                if (value != currentIndexBuffer)
                {
                    currentIndexBuffer = value;
                    IndexBuffer internalVertexBuffer = graphicsDevice.Cast<IndexBuffer>(value, "value");

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, internalVertexBuffer.IndexBufferID);
                }
                    
            } 
        }

        internal IConstantBuffer ConstantBuffer
        {
            get { return currentConstantBuffer; }
            set
            {
                if (value != currentConstantBuffer)
                {
                    currentConstantBuffer = value;
                    ConstantBuffer internalConstantBuffer = graphicsDevice.Cast<ConstantBuffer>(value, "value");

                    GL.BindBuffer(BufferTarget.UniformBuffer, internalConstantBuffer.UniformBufferObjectID);
                }

            }
        }

        internal IShader Shader 
        { 
            get { return currentShader; }
            set 
            {
                if (value != currentShader)
                {
                    currentShader = value;
                    Shader internalShader = graphicsDevice.Cast<Shader>(value, "value");

                    GL.UseProgram(internalShader.ProgramID);
                }
            }
        }
        
        public StateManager(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }
    }
}
