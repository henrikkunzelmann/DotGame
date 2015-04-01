﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DotGame.Graphics;
using SharpDX.Direct3D11;

namespace DotGame.DirectX11
{
    public class RenderContext : GraphicsObject, IRenderContext
    {
        private RenderStateInfo currentState = new RenderStateInfo();
        private VertexBuffer currentVertexBuffer;
        private IndexBuffer currentIndexBuffer;

        private bool stateDirty;
        private bool vertexBufferDirty;
        private bool indexBufferDirty;

        public RenderContext(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, new StackTrace(1))
        {

        }

        protected override void Dispose(bool isDisposing)
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

        public void SetState(IRenderState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            graphicsDevice.Cast<RenderState>(state, "state"); // State überprüfen

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
                graphicsDevice.Context.VertexShader.Set(shader.VertexShaderHandle);
                graphicsDevice.Context.PixelShader.Set(shader.PixelShaderHandle);
                graphicsDevice.Context.InputAssembler.PrimitiveTopology = EnumConverter.Convert(currentState.PrimitiveType);
            }
            if (vertexBufferDirty)
            {
                graphicsDevice.Context.InputAssembler.InputLayout = graphicsDevice.GetLayout(currentVertexBuffer.Description, shader);
                graphicsDevice.Context.InputAssembler.SetVertexBuffers(0, currentVertexBuffer.Binding);
            }
            if (indexBufferDirty)
                graphicsDevice.Context.InputAssembler.SetIndexBuffer(currentIndexBuffer.Buffer, currentIndexBuffer.Format, 0);
        }

        public void Draw()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");

            ApplyState();
            graphicsDevice.Context.Draw(currentVertexBuffer.VertexCount, 0);
        }

        public void DrawIndexed()
        {
            if (currentVertexBuffer == null)
                throw new InvalidOperationException("Tried to draw without a vertexbuffer set.");
            if (currentIndexBuffer == null)
                throw new InvalidOperationException("Tried to draw without indexbuffer set.");

            ApplyState();
            graphicsDevice.Context.DrawIndexed(currentIndexBuffer.IndexCount, 0, 0);
        }
    }
}
