using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotGame.Graphics
{
    public struct OpenGLGraphicsCapabilities
    {
        internal Version OpenGLVersion;
        internal Version GLSLVersion;

        /// <summary>
        /// Gibt an ob die API Texturauflösungen kann, welche keine Zweierpotenz sind.
        /// </summary>
        public bool SupportsNonPowerOf2Textures;

        /// <summary>
        /// Gibt die maximale Texturgröße an.
        /// </summary>
        public int MaxTextureSize;

        /// <summary>
        /// Gibt an ob die Grafikkarte anisotropisches Filtern unterstützt. (EXT_texture_filter_anisotropic)
        /// </summary>
        internal bool SupportsAnisotropicFiltering;
        
        /// <summary>
        /// Gibt an ob die S3 Texturekompression unterstützt. (EXT_texture_compression_s3tc)
        /// </summary>
        internal bool SupportsS3TextureCompression;

        /// <summary>
        /// Gibt an ob Immutable Textures unterstützt werden (ARB_texture_storage, core ab 4.2)
        /// </summary>
        internal bool SupportsTextureStorage;

        /// <summary>
        /// Gibt an ob Immutable Buffer unterstützt werden (ARB_buffer_storage, core ab 4.4)
        /// </summary>
        internal bool SupportsBufferStorage;

        /// <summary>
        /// Gibt an ob die DebugOutput Extension unterstützt wird (ARB_debug_output, core ab 4.1)
        /// </summary>
        internal bool SupportsDebugOutput;

        /// <summary>
        /// Gibt die Anzahl der verfügbaren Texture Units an
        /// </summary>
        internal int TextureUnits;

        /// <summary>
        /// Gibt die maximale Stufe anisotropischer Filterung an (Für Anisotrope Filterung) 
        /// </summary>
        internal int MaxAnisotropicFiltering;

        /// <summary>
        /// Gibt den maximalen TextureLoDBias an
        /// </summary>
        internal int MaxTextureLoDBias;

        /// <summary>
        /// Gibt an, ob und wie DirectStateAccess unterstützt wird
        /// </summary>
        internal DirectStateAccess DirectStateAccess;

        /// <summary>
        /// Gibt die maximale Anzahl der Vertex Attribute an
        /// </summary>
        internal int MaxVertexAttribs;

        /// <summary>
        /// Gibt an, ob VertexAttribBinding unterstützt wird
        /// </summary>
        internal bool VertexAttribBinding;

        /// <summary>
        /// Gibt die maximale Anzahl VertexBuffer Bindings an (Für VertexAttribBinding)
        /// </summary>
        internal int MaxVertexAttribBindings;

        /// <summary>
        /// Gibt das VertexBuffer Binding Offset an (For VertexAttribBinding)
        /// </summary>
        internal int MaxVertexAttribBindingOffset;

        /// <summary>
        /// Gibt die maximale Größe eines Vertex an
        /// </summary>
        internal int MaxVertexAttribStride;

        /// <summary>
        /// Gibt an, ob der Treiber das Validieren von Texturen, Buffern und FrameBuffern beherrscht. (ARB_invalidate_subdata, core ab 4.3)
        /// </summary>
        internal bool SupportsResourceValidation;
    }

    internal enum DirectStateAccess
    {
        None,

        /// <summary>
        /// GL_EXT_direct_state_access
        /// </summary>
        Extension,

        /// <summary>
        /// GL_ARB_direct_state_access, core ab 4.5
        /// </summary>
        Core
    }
}
