using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using DotGame.Graphics;

namespace DotGame.Assets.Importers.DDSTextureImporter
{
    public class DDSTextureImporter : TextureImporterBase
    {
        private TextureHeader header;

        private Header headerDDS;
        private HeaderDXT10 headerDXT10;

        private bool IsHeaderLoaded = false;
        private bool hasHeaderDXT10 = false;

        private IntPtr ptr;
        
        public DDSTextureImporter(AssetManager manager)
            :base(manager)
        {
        }

        public override TextureHeader LoadHeader(string file, TextureLoadSettings settings)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            header = new TextureHeader();

            FileStream stream = new FileStream(file, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);
            try
            {
                //Header + Magic Word
                int magicWordSize = 4;
                byte[] magicWordData = reader.ReadBytes(magicWordSize);
                string magicWord = Encoding.ASCII.GetString(magicWordData).Trim();

                if (magicWord == "DDS")
                {
                    int headerSize = 124;
                    byte[] headerData = reader.ReadBytes(headerSize);

                    this.headerDDS = DDSTextureImporter.ByteArrayToStruct<Header>(headerData, 0);
                    header.Width = (int)this.headerDDS.Width;
                    header.Height = (int)this.headerDDS.Height;
                    header.Depth = (int)Depth;
                    header.MipLevels = this.headerDDS.MipMapCount > 0 ? (int)this.headerDDS.MipMapCount : 1;

                    headerDXT10 = new HeaderDXT10();
                    if (this.headerDDS.PixelFormat.Flags == PixelFormatFlags.FourCC && this.headerDDS.PixelFormat.FourCC == "DXT10")
                    {
                        hasHeaderDXT10 = true;
                        byte[] headerDxt10Data = reader.ReadBytes(Marshal.SizeOf(typeof(HeaderDXT10)));
                        headerDXT10 = DDSTextureImporter.ByteArrayToStruct<HeaderDXT10>(headerDxt10Data, 0);
                    }

                    header.Format = GetTextureFormat();
                }
            }
            finally
            {
                reader.Close();
                stream.Close();
            }
            IsHeaderLoaded = true;
            return header;
        }

        public override void LoadData(Graphics.ITexture2D handle, string file, TextureLoadSettings settings)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (handle == null)
                throw new ArgumentNullException("handle");

            if (!IsHeaderLoaded)
            {
                LoadHeader(file, settings);
            }

            FileStream stream = new FileStream(file, FileMode.Open);
            if (stream.Length <= 128)
                throw new Exception("File is too small");

            GCHandle arrayHandle;

            try
            {
                int dataOffset = 4 + 124;

                if (hasHeaderDXT10)
                    dataOffset += Marshal.SizeOf(typeof(HeaderDXT10));

                stream.Position = dataOffset;

                int dataSize = (int)stream.Length - dataOffset;
                byte[] data = new byte[dataSize];
                stream.Read(data, 0, dataSize);

                if (!IsVolumeTexture)
                {
                    int faces = IsCubeMap ? 6 : 1;
                    int levels = header.MipLevels > 0 ? header.MipLevels : 1;
                    DataRectangle[] rectangles = new DataRectangle[levels*faces];


                    if (TextureFormatHelper.IsCompressed(header.Format))
                        arrayHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    else
                        arrayHandle = GCHandle.Alloc(ConvertFormat(data), GCHandleType.Pinned);

                    IntPtr pointer = arrayHandle.AddrOfPinnedObject();

                    for (int face = 0; face < faces; face++)
                    {
                        int width = header.Width;
                        int height = header.Height;
                        for (int mipLevel = 0; mipLevel < levels; mipLevel++)
                        {
                            int size = (int)GetSize(width, height);
                            rectangles[levels * face + mipLevel] = new DataRectangle(pointer, (int)GetPitch(width), size);

                            width /= 2;
                            height /= 2;
                            pointer += size;
                        }
                    }
                    Engine.GraphicsDevice.RenderContext.Update(handle, rectangles);

                }
                else
                {
                    //TextureImporterBase doesn't support 3D Textures
                }
            }
            finally
            {
                //if (arrayHandle.IsAllocated)
                //    arrayHandle.Free();

                stream.Close();
            }
        }


        private TextureFormat GetTextureFormat()
        {
            if (!hasHeaderDXT10 && headerDDS.PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
            {
                //Komprimierte Texturen
                if (headerDDS.PixelFormat.FourCC == "DXT1")
                    return TextureFormat.DXT1;

                if (headerDDS.PixelFormat.FourCC == "DXT2")
                    return TextureFormat.Unknown;

                if (headerDDS.PixelFormat.FourCC == "DXT3")
                    return TextureFormat.DXT3;

                if (headerDDS.PixelFormat.FourCC == "DXT4")
                    return TextureFormat.Unknown;

                if (headerDDS.PixelFormat.FourCC == "DXT5")
                    return TextureFormat.DXT5;
            }
            else if (hasHeaderDXT10)
            {
            }

            //Textur wird konvertiert
            return TextureFormat.RGBA32_Float;
        }

        /// <summary>
        /// Convert to R16G16B16A16_FLOAT
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Color[] ConvertFormat(byte[] data)
        {
            Color[] colors = null;
            if (!hasHeaderDXT10)
            {           
                //Mit DDS PixelFormat konvertieren
                if (!headerDDS.PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
                {
                    colors = headerDDS.PixelFormat.ToRGBA(data, header.Width, header.Height);
                }
            }

            return colors;
        }

        private uint GetSize(int width, int height)
        {
            if (headerDDS.PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
            {
                if (headerDDS.PixelFormat.FourCC == "DXT1" || headerDDS.PixelFormat.FourCC == "DXT2" || headerDDS.PixelFormat.FourCC == "DXT3" || headerDDS.PixelFormat.FourCC == "DXT4" || headerDDS.PixelFormat.FourCC == "DXT5")
                {
                    return (uint)(Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * BlockSize);
                }
            }

            return ((uint)width * headerDDS.PixelFormat.RGBBitCount + 7) / 8 * (uint)height;
        }

        private uint GetPitch(int width)
        {
            if (headerDDS.PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
            {
                if (headerDDS.PixelFormat.FourCC == "DXT1" || headerDDS.PixelFormat.FourCC == "DXT2" || headerDDS.PixelFormat.FourCC == "DXT3" || headerDDS.PixelFormat.FourCC == "DXT4" || headerDDS.PixelFormat.FourCC == "DXT5")
                {
                    return (uint)(Math.Max(1, (width + 3) / 4) * BlockSize);
                }
            }

            return (uint)(width * headerDDS.PixelFormat.RGBBitCount + 7) / 8;
        }

        private uint BlockSize
        {
            get
            {
                if (headerDDS.PixelFormat.Flags == PixelFormatFlags.FourCC)
                {
                    if (headerDDS.PixelFormat.FourCC == "DXT1" || (headerDDS.PixelFormat.FourCC == "DXT10" && (headerDXT10.Format == Format.BC4_SNorm || headerDXT10.Format == Format.BC4_Typeless || headerDXT10.Format == Format.BC4_UNorm)))
                        return 8;
                    else
                        return 16;
                }
                return 1;
            }
        }

        private uint Depth
        {
            get
            {
                uint depth = 1;
                if (headerDDS.Caps2 == Caps2.CubeMap)
                {
                    depth = 0;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapPositiveX))
                        depth += 1;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapPositiveY))
                        depth += 1;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapNegativeX))
                        depth += 1;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapNegativeY))
                        depth += 1;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapPositiveZ))
                        depth += 1;
                    if (headerDDS.Caps2.HasFlag(Caps2.CubeMapNegativeZ))
                        depth += 1;
                                        
                    return depth;
                }
                else if (headerDDS.Flags.HasFlag(Flags.Depth) && headerDDS.Caps2.HasFlag(Caps2.Volume))
                    return headerDDS.Depth;

                return 1;
            }
        }

        private bool IsVolumeTexture
        {
            get {
                //headerDDS.Caps.HasFlag(Caps.Complex); is not required
                return headerDDS.Caps2.HasFlag(Caps2.Volume);
            }
        }
        private bool IsCubeMap
        {
            get {
                //headerDDS.Caps.HasFlag(Caps.Complex); is not required

                //DX11 doesn't support partial cubemaps
                return headerDDS.Caps2.HasFlag(Caps2.CubeMap) && headerDDS.Caps2.HasFlag(Caps2.CubeMapAllFaces);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
        }

        private static T ByteArrayToStruct<T>(byte[] array, int offset) where T : struct
        {
            if (array == null)
                throw new ArgumentNullException("array");

            int size = Marshal.SizeOf(typeof(T));

            if (array.Length < size)
                throw new ArgumentException("array");

            if ((offset < 0) || (offset + size > array.Length))
                throw new ArgumentOutOfRangeException("offset");

            object structure;
            GCHandle arrayHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = arrayHandle.AddrOfPinnedObject();
                ptr += offset;
                structure = Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                arrayHandle.Free();
            }
            return (T)structure;
        }
        private static T ByteArrayToStruct<T>(byte[] array) where T : struct
        {
            return ByteArrayToStruct<T>(array, 0);
        }
    }
}
