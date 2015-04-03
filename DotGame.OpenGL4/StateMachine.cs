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
    public class StateMachine
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
                VertexBuffer internalVertexBuffer = graphicsDevice.Cast<VertexBuffer>(value, "value");
                if (value != currentVertexBuffer || (currentVertexBuffer != null && internalVertexBuffer.Shader != Shader)) 
                {
                    currentVertexBuffer = value;

                    GL.BindVertexArray(internalVertexBuffer.VertexArrayObjectID);

                    if (Shader == null)
                        throw new Exception("No Shader set!");

                    //VAO speichert VertexAttributePointer für einen bestimmten Shader
                    //Falls dieser Shader gerade nicht aktiv ist, müssen neue VertexAttributePointer gesetzt werden
                    if (internalVertexBuffer.Shader != Shader)
                    {
                        Shader internalShader = graphicsDevice.Cast<Shader>(Shader, "CurrentShader");

                        GL.BindBuffer(BufferTarget.ArrayBuffer, internalVertexBuffer.VertexBufferObjectID);

                        int offset = 0;
                        VertexElement[] elements = currentVertexBuffer.Description.GetElements();
                        for (int i = 0; i < currentVertexBuffer.Description.ElementCount; i++)
                        {
                            GL.EnableVertexAttribArray(i);
                            GL.BindAttribLocation(internalShader.ProgramID, i, EnumConverter.Convert(elements[i].Usage));

                            GL.VertexAttribPointer(i, graphicsDevice.GetComponentsOf(elements[i].Type), VertexAttribPointerType.Float, false, graphicsDevice.GetSizeOf(internalVertexBuffer.Description), offset);
                            offset += graphicsDevice.GetSizeOf(elements[i].Type);
                        }
                        internalVertexBuffer.Shader = internalShader;
                    }
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
        
        public StateMachine(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }
    }
}
