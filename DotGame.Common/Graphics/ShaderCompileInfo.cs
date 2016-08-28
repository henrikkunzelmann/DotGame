namespace DotGame.Graphics
{
    /// <summary>
    /// Speichert Informationen über das compilieren von Shadern.
    /// </summary>
    public struct ShaderCompileInfo
    {
        /// <summary>
        /// Gibt die Quellcode-Datei des Shaders an oder setzt diese.
        /// </summary>
        public string File;

        /// <summary>
        /// Gibt die Funktion im Quellcode des Shaders an oder setzt diese.
        /// </summary>
        public string Function;

        /// <summary>
        /// Gibt die Version des Shaders an oder setzt diese. Wenn der Quellcode selbst die Version angibt, dann wird dieser Wert nicht benutzt.
        /// </summary>
        public string Version;

        public ShaderCompileInfo(string file, string function, string version)
        {
            this.File = file;
            this.Function = function;
            this.Version = version;
        }
    }
}
