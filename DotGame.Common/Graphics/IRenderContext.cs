namespace DotGame.Graphics
{
    /// <summary>
    /// Verwaltet Befehle an das IGraphicsDevice, wie die Befehle über die Pipeline oder Zeichen-Befehle.
    /// </summary>
    public interface IRenderContext : IGraphicsObject
    {
        IRenderUpdateContext UpdateContext { get; }

        /// <summary>
        /// Leert den Inhalt des aktuell gebundenen RenderTargets. Ist kein RenderTarget gebunden, wird der Backbuffer angesprochen.
        /// </summary>
        /// <param name="color">Die Farbe.</param>
        void Clear(Color color);

        /// <summary>
        /// Leert den Inhalt des aktuell gebundenen RenderTargets. Ist kein RenderTarget gebunden, wird der Backbuffer angesprochen.
        /// </summary>
        /// <param name="clearOptions">Die <see cref="ClearOptions"/>-Flags, die die angesprochenen Channel angeben.</param>
        /// <param name="color">Die Farbe für Farbchannel benutzt wird.</param>
        /// <param name="depth">Der Wert für den Tiefenchannel (standardmäßig 1).</param>
        /// <param name="stencil">Der Wert für den Stencilchannel (standardmäßig 0).</param>
        void Clear(ClearOptions clearOptions, Color color, float depth, byte stencil);

        /// <summary>
        /// Setzt Color-RenderTarget und Depth-RenderTarget auf die Standardwerte.
        /// </summary>
        void SetRenderTargetsBackBuffer();
        void SetRenderTargets(IRenderTarget2D depth, params IRenderTarget2D[] colorTargets);
        void SetRenderTargetsDepth(IRenderTarget2D depth);
        void SetRenderTargetsColor(params IRenderTarget2D[] colorTargets);

        void SetViewport(Viewport viewport);

        void SetScissor(Rectangle rectangle);

        void SetBlendFactor(Color blendFactor);

        void SetStencilReference(byte stencilReference);

        /// <summary>
        /// Setzt die Pipeline auf den neuen Zustand.
        /// </summary>
        /// <param name="state"></param>
        void SetState(IRenderState state);

        /// <summary>
        /// Setzt den VertexBuffer.
        /// </summary>
        /// <param name="vertexBuffer"></param>
        void SetVertexBuffer(IVertexBuffer vertexBuffer);

        /// <summary>
        /// Setzt den IndexBuffer.
        /// </summary>
        /// <param name="indexBuffer"></param>
        void SetIndexBuffer(IIndexBuffer indexBuffer);

        /// <summary>
        /// Setzt den aktuellen ConstantBuffer für alle globale Variablen.
        /// </summary>
        /// <param name="buffer"></param>
        void SetConstantBuffer(IShader shader, IConstantBuffer buffer);

        /// <summary>
        /// Setzt den aktuellen ConstantBuffer für die Variable name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buffer"></param>
        void SetConstantBuffer(IShader shader, string name, IConstantBuffer buffer);

        /// <summary>
        /// Setzt die Texture für einen Shader auf NULL.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        void SetTextureNull(IShader shader, string name);

        /// <summary>
        /// Setzt die Texture für einen Shader.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        void SetTexture(IShader shader, string name, ITexture2D texture);
        /// <summary>
        /// Setzt die Texture für einen Shader.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        void SetTexture(IShader shader, string name, ITexture2DArray texture);
        /// <summary>
        /// Setzt die Texture für einen Shader.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        void SetTexture(IShader shader, string name, ITexture3D texture);
        /// <summary>
        /// Setzt die Texture für einen Shader.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        void SetTexture(IShader shader, string name, ITexture3DArray texture);

        /// <summary>
        /// Setzt den Sampler für einen Shader.
        /// </summary>
        /// <param name="shader"></param>
        /// <param name="name"></param>
        /// <param name="sampler"></param>
        void SetSampler(IShader shader, string name, ISampler sampler);

        /// <summary>
        /// Zeichnet nicht indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl der Vertices im gesetzten VertexBuffer.
        /// </summary>
        void Draw();

        /// <summary>
        /// Zeichnet nicht indexierte Vertices mit dem gesetzten PrimitiveType und den übergebenen Parametern.
        /// </summary>
        void Draw(int vertexCount, int startVertexLocation);

        /// <summary>
        /// Zeichnet indexierte Vertices mit dem gesetzten PrimitiveType und der Anzahl aller Indices im gesetzten IndexBuffer.
        /// </summary>
        void DrawIndexed();

        /// <summary>
        /// Zeichnet indexierte Vertices mit dem gesetzten PrimitiveType und den übergebenen Parametern.
        /// </summary>
        void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation);
    }
}
