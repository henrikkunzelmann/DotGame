namespace DotGame.Graphics
{
    public interface IRenderUpdateContext : IGraphicsObject
    {
        IRenderContext RenderContext { get; }

        /// <summary>
        /// Füllt einen ConstantBuffer
        /// </summary>
        /// <param name="constantBuffer">Zu füllender ConstantBuffer</param>
        /// <param name="data">Daten</param>
        void Update(IConstantBuffer constantBuffer, DataArray data);


        /// <summary>
        /// Füllt einen VertexBuffer
        /// </summary>
        /// <param name="vertexBuffer">Zu füllender VertexBuffer</param>
        /// <param name="data">Daten</param>
        void Update(IVertexBuffer vertexBuffer, DataArray data);
        

        /// <summary>
        /// Füllt einen IndexBuffer
        /// </summary>
        /// <param name="indexBuffer">Zu füllender IndexBuffer</param>
        /// <param name="data">Daten</param>
        void Update(IIndexBuffer indexBuffer, DataArray data, int count);
        
        
        /// <summary>
        /// Füllt ein einziges Level der Textur
        /// </summary>
        /// <param name="texture">Zu füllende Textur</param>
        /// <param name="mipLevel">Zu füllendes Mip-Level</param>
        /// <param name="data">Daten</param>
        void Update(ITexture2D texture, int mipLevel, DataRectangle data);
            

        /// <summary>
        /// Füllt ein einziges Level einer Textur eines 2D Texturarrays
        /// </summary>
        /// <param name="textureArray">2D Texturarray</param>
        /// <param name="arrayIndex">Position der zu füllenden Textur</param>
        /// <param name="mipLevel">Zu füllendes Mip-Level</param>
        /// <param name="data">Daten</param>
        void Update(ITexture2DArray textureArray, int arrayIndex, int mipLevel, DataRectangle data);
        
        /// <summary>
        /// Füllt ein Mip-Level einer Textur einer 3D Textur
        /// </summary>
        /// <param name="texture">Zu füllende Textur</param>
        /// <param name="mipLevel">Zu füllendes Mip-Level</param>
        /// <param name="data">Datem</param>
        void Update(ITexture3D texture, int mipLevel, DataBox data);


        /// <summary>
        /// Generiert die Mipmaps einer 2D Textur
        /// </summary>
        /// <param name="texture">Textur</param>
        void GenerateMips(ITexture2D texture);

        /// <summary>
        /// Generiert die Mipmaps eines 2D Texturarrays
        /// </summary>
        /// <param name="textureArray">Texturarray</param>
        void GenerateMips(ITexture2DArray textureArray);

        /// <summary>
        /// Generiert die Mipmaps einer 3D Textur
        /// </summary>
        /// <param name="texture">Textur</param>
        void GenerateMips(ITexture3D texture);
    }
}
