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
        private Dictionary<string,TextureHeader> headers = new Dictionary<string, TextureHeader>();
        private Dictionary<string,Header> ddsHeaders = new Dictionary<string, Header>();
        private Dictionary<string, HeaderDXT10> dxt10Headers = new Dictionary<string, HeaderDXT10>();
                
        public DDSTextureImporter(AssetManager manager)
            :base(manager)
        {
        }

        public override TextureHeader LoadHeader(string file, TextureLoadSettings settings)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            var header = new TextureHeader();

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

                    var headerDDS = DDSTextureImporter.ByteArrayToStruct<Header>(headerData, 0);
                    ddsHeaders.Add(file, headerDDS);
                    header.Width = (int)headerDDS.Width;
                    header.Height = (int)headerDDS.Height;
                    header.Depth = (int)GetDepth(file);
                    header.MipLevels = headerDDS.MipMapCount > 0 ? (int)headerDDS.MipMapCount : 1;
                    
                    if (headerDDS.PixelFormat.Flags == PixelFormatFlags.FourCC && headerDDS.PixelFormat.FourCC == "DXT10")
                    {
                        byte[] headerDxt10Data = reader.ReadBytes(Marshal.SizeOf(typeof(HeaderDXT10)));
                        var headerDXT10 = DDSTextureImporter.ByteArrayToStruct<HeaderDXT10>(headerDxt10Data, 0);
                        dxt10Headers.Add(file, headerDXT10);
                    }

                    header.Format = GetTextureFormat(file);

                    headers.Add(file, header);
                }
            }
            finally
            {
                reader.Close();
                stream.Close();
            }
            return header;
        }

        public override void LoadData(Graphics.ITexture2D handle, string file, TextureLoadSettings settings)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (handle == null)
                throw new ArgumentNullException("handle");

            if (!ddsHeaders.ContainsKey(file) || !headers.ContainsKey(file))
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

                if (dxt10Headers.ContainsKey(file))
                    dataOffset += Marshal.SizeOf(typeof(HeaderDXT10));

                stream.Position = dataOffset;

                int dataSize = (int)stream.Length - dataOffset;
                byte[] data = new byte[dataSize];
                stream.Read(data, 0, dataSize);

                if (!GetIsVolumeTexture(file))
                {
                    int faces = GetIsCubeMap(file) ? 6 : 1;
                    int levels = headers[file].MipLevels > 0 ? headers[file].MipLevels : 1;
                    DataRectangle[] rectangles = new DataRectangle[levels*faces];
                                        
                    if (TextureFormatHelper.IsCompressed(headers[file].Format))
                        arrayHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    else
                        arrayHandle = GCHandle.Alloc(ConvertFormat(file, data), GCHandleType.Pinned);

                    try
                    {
                        IntPtr pointer = arrayHandle.AddrOfPinnedObject();

                        for (int face = 0; face < faces; face++)
                        {
                            int width = headers[file].Width;
                            int height = headers[file].Height;
                            for (int mipLevel = 0; mipLevel < levels; mipLevel++)
                            {
                                int size;
                                if (TextureFormatHelper.IsCompressed(headers[file].Format))
                                    size = (int)CalculateSize(file, width, height);
                                else
                                    size = width * height * Engine.GraphicsDevice.GetSizeOf(headers[file].Format);

                                rectangles[levels * face + mipLevel] = new DataRectangle(pointer, (int)CalculatePitch(file, width), size);

                                width /= 2;
                                height /= 2;
                                pointer += size;
                            }
                        }
                        for (int i = 0; i < rectangles.Length; i++)
                            Engine.GraphicsDevice.RenderContext.UpdateContext.Update(handle, i, rectangles[i]);
                    }
                    finally
                    {
                        arrayHandle.Free();
                    }

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

                headers.Remove(file);
                dxt10Headers.Remove(file);
                ddsHeaders.Remove(file);
            }
        }


        private TextureFormat GetTextureFormat(string file)
        {
            if (!dxt10Headers.ContainsKey(file))
            {
                if (ddsHeaders.ContainsKey(file) && ddsHeaders[file].PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
                {
                    //Komprimierte Texturen
                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT1")
                        return TextureFormat.DXT1;

                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT2")
                        return TextureFormat.Unknown;

                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT3")
                        return TextureFormat.DXT3;

                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT4")
                        return TextureFormat.Unknown;

                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT5")
                        return TextureFormat.DXT5;
                }
            }
            else
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
        private Color[] ConvertFormat(string file, byte[] data)
        {
            Color[] colors = null;
            if (!dxt10Headers.ContainsKey(file) && ddsHeaders.ContainsKey(file) && headers.ContainsKey(file))
            {           
                //Mit DDS PixelFormat konvertieren
                if (!ddsHeaders[file].PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
                {
                    colors = ddsHeaders[file].PixelFormat.ToRGBA(data, headers[file].Width, headers[file].Height);
                }
            }

            return colors;
        }

        private uint CalculateSize(string file, int width, int height)
        {
            if (ddsHeaders.ContainsKey(file) && ddsHeaders[file].PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
            {
                if (ddsHeaders[file].PixelFormat.FourCC == "DXT1" || ddsHeaders[file].PixelFormat.FourCC == "DXT2" || ddsHeaders[file].PixelFormat.FourCC == "DXT3" || ddsHeaders[file].PixelFormat.FourCC == "DXT4" || ddsHeaders[file].PixelFormat.FourCC == "DXT5")
                {
                    return (uint)(Math.Max(1, (width + 3) / 4) * Math.Max(1, (height + 3) / 4) * GetBlockSize(file));
                }
            }

            return ((uint)width * ddsHeaders[file].PixelFormat.RGBBitCount + 7) / 8 * (uint)height;
        }

        private uint CalculatePitch(string file, int width)
        {
            if (ddsHeaders.ContainsKey(file) && ddsHeaders[file].PixelFormat.Flags.HasFlag(PixelFormatFlags.FourCC))
            {
                if (ddsHeaders[file].PixelFormat.FourCC == "DXT1" || ddsHeaders[file].PixelFormat.FourCC == "DXT2" || ddsHeaders[file].PixelFormat.FourCC == "DXT3" || ddsHeaders[file].PixelFormat.FourCC == "DXT4" || ddsHeaders[file].PixelFormat.FourCC == "DXT5")
                {
                    return (uint)(Math.Max(1, (width + 3) / 4) * GetBlockSize(file));
                }
            }

            return (uint)(width * ddsHeaders[file].PixelFormat.RGBBitCount + 7) / 8;
        }

        private uint GetBlockSize(string file)
        {
            if (ddsHeaders.ContainsKey(file) && headers.ContainsKey(file) && ddsHeaders[file].PixelFormat.Flags == PixelFormatFlags.FourCC)
                {
                    if (ddsHeaders[file].PixelFormat.FourCC == "DXT1" || (dxt10Headers.ContainsKey(file) && ddsHeaders[file].PixelFormat.FourCC == "DXT10" && (dxt10Headers[file].Format == Format.BC4_SNorm || dxt10Headers[file].Format == Format.BC4_Typeless || dxt10Headers[file].Format == Format.BC4_UNorm)))
                        return 8;
                    else
                        return 16;
                }
                return 1;
        }

        private uint GetDepth(string file)
        {
            uint depth = 1;
            if (ddsHeaders.ContainsKey(file))
            {
                if (ddsHeaders[file].Caps2 == Caps2.CubeMap)
                {
                    depth = 0;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapPositiveX))
                        depth += 1;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapPositiveY))
                        depth += 1;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapNegativeX))
                        depth += 1;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapNegativeY))
                        depth += 1;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapPositiveZ))
                        depth += 1;
                    if (ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapNegativeZ))
                        depth += 1;

                    return depth;
                }
                else if (ddsHeaders[file].Flags.HasFlag(Flags.Depth) && ddsHeaders[file].Caps2.HasFlag(Caps2.Volume))
                    return ddsHeaders[file].Depth;
            }

            return 1;
        }

        private bool GetIsVolumeTexture(string file)
        {
            //headerDDS.Caps.HasFlag(Caps.Complex); is not required
            return ddsHeaders.ContainsKey(file) && ddsHeaders[file].Caps2.HasFlag(Caps2.Volume);
        }
        private bool GetIsCubeMap(string file)
        {
            //headerDDS.Caps.HasFlag(Caps.Complex); is not required

            //DX11 doesn't support partial cubemaps
            return ddsHeaders.ContainsKey(file) && ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMap) && ddsHeaders[file].Caps2.HasFlag(Caps2.CubeMapAllFaces);
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
