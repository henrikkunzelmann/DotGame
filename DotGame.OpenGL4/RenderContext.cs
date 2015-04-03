using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotGame.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace DotGame.OpenGL4
{
    internal class RenderContext : GraphicsObject, IRenderContext
    {
        private RenderStateInfo currentState = new RenderStateInfo();
        private VertexBuffer currentVertexBuffer;
        private IndexBuffer currentIndexBuffer;

        private bool stateDirty;
        private bool vertexBufferDirty;
        private bool indexBufferDirty;

        public RenderContext(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new System.Diagnostics.StackTrace(1))
        {
 
        }

        public void SetShader(IShader shader)
        {
            if (shader == null)
                throw new ArgumentNullException("shader");
            graphicsDevice.Cast<IShader>(shader, "shader"); // Shader überprüfen

            currentState.Shader = shader;
            stateDirty = true;
        }

        public void SetPrimitiveType(PrimitiveType type)
        {
            EnumConverter.Convert(type); // Type überprüfen (ob supported ist)

            currentState.PrimitiveType = type;
            stateDirty = true;
        }

        public void SetCullMode(CullMode cullMode)
        {
            EnumConverter.Convert(cullMode); // Type überprüfen (ob supported ist)

            currentState.CullMode = cullMode;
            stateDirty = true;
        }

        public void SetState(IRenderState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            //graphicsDevice.Cast<RenderState>(state, "state"); // State überprüfen

            if (!state.Info.Equals(currentState))
            {
                stateDirty = true;
                currentState = state.Info;
            }
        }

        public void SetVertexBuffer(IVertexBuffer vertexBuffer)
        {
            if (vertexBuffer == null)
                throw new ArgumentNullException("vertexBuffer");
            if (vertexBuffer.IsDisposed)
                throw new ArgumentException("VertexBuffer is disposed.", "vertexBuffer");

            if (currentVertexBuffer != vertexBuffer)
            {
                currentVertexBuffer = graphicsDevice.Cast<VertexBuffer>(vertexBuffer, "vertexBuffer");
                vertexBufferDirty = true;
            }
        }

        public void SetIndexBuffer(IIndexBuffer indexBuffer)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            if (indexBuffer.IsDisposed)
                throw new ArgumentException("IndexBuffer is disposed.", "indexBuffer");

            if (currentIndexBuffer != indexBuffer)
            {
                currentIndexBuffer = graphicsDevice.Cast<IndexBuffer>(indexBuffer, "indexBuffer");
                indexBufferDirty = true;
            }
        }

        private void ApplyState()
        {
            var shader = graphicsDevice.Cast<Shader>(currentState.Shader, "currentState.Shader");
            if (stateDirty)
            {
                graphicsDevice.StateMachine.Shader = shader;

                if (currentState.CullMode != CullMode.None)
                {
                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(EnumConverter.Convert(currentState.CullMode));
                }
                else
                    GL.Disable(EnableCap.CullFace);
            }
            if (vertexBufferDirty)
            {
                graphicsDevice.StateMachine.VertexBuffer = currentVertexBuffer;
            }
            if (indexBufferDirty)
                graphicsDevice.StateMachine.IndexBuffer = currentIndexBuffer;

            //TEST
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            OpenGL4.GraphicsDevice.CheckGLError();
        }

        public void Draw()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");

            ApplyState();
            GL.DrawArrays(EnumConverter.Convert(currentState.PrimitiveType), 0, currentVertexBuffer.VertexCount);
        }

        public void DrawIndexed()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");
            if (currentIndexBuffer == null)
                throw new InvalidOperationException("Tried to draw without indexbuffer set.");

            ApplyState();
            GL.DrawElements((BeginMode)EnumConverter.Convert(currentState.PrimitiveType), currentIndexBuffer.IndexCount, EnumConverter.Convert(currentIndexBuffer.Format), 0);
        }

        protected override void Dispose(bool isDisposing)
        {
            
        }
    }
}
