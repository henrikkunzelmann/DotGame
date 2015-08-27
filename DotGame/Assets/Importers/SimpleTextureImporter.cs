using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using DotGame.Graphics;
using Color = DotGame.Graphics.Color;
using System.Runtime.InteropServices;

namespace DotGame.Assets.Importers
{
    public class SimpleTextureImporter : TextureImporterBase
    {
        public SimpleTextureImporter(AssetManager assetManager)
            : base(assetManager)
        {

        }

        public override TextureHeader LoadHeader(string file, TextureLoadSettings settings)
        {
            using(Image image = Image.FromFile(file))
            {
                return new TextureHeader()
                {
                    Width = image.Width,
                    Height = image.Height,
                    Format = TextureFormat.RGBA32_Float,
                    MipLevels = 1
                };
            }
        }


        public override void LoadData(ITexture2D handle, string file, TextureLoadSettings settings)
        {
            unsafe
            {
                using (Bitmap bitmap = new Bitmap(file))
                {
                    BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                    try
                    {
                        Color[] colorsArgb;
                        switch (data.PixelFormat)
                        {
                            case PixelFormat.Format24bppRgb:
                                colorsArgb = new Color[data.Width * data.Height];
                                for (int x = 0; x < data.Width; x++)
                                    for (int y = 0; y < data.Height; y++)
                                    {
                                        byte* b = (byte*)(data.Scan0 + y * data.Stride + x * 3);
                                        byte* g = b + 1;
                                        byte* r = g + 1;
                                        colorsArgb[y * data.Width + x] = Color.FromArgb(255, *r, *g, *b);
                                    }
                                break;
                            case PixelFormat.Format32bppArgb:
                                colorsArgb = new Color[data.Width * data.Height];
                                for (int x = 0; x < data.Width; x++)
                                    for (int y = 0; y < data.Height; y++)
                                    {
                                        byte* b = (byte*)(data.Scan0 + y * data.Stride + x * 4);
                                        byte* g = b + 1;
                                        byte* r = g + 1;
                                        byte* a = r + 1;
                                        colorsArgb[y * data.Width + x] = Color.FromArgb(*a, *r, *g, *b);
                                    }
                                break;
                            case PixelFormat.Format8bppIndexed:
                                colorsArgb = new Color[data.Width * data.Height];
                                for (int x = 0; x < data.Width; x++)
                                    for (int y = 0; y < data.Height; y++)
                                    {
                                        byte* index = (byte*)(data.Scan0 + y * data.Stride + x);

                                        colorsArgb[y * data.Width + x] = Color.FromArgb(bitmap.Palette.Entries[*index].ToArgb());
                                    }
                                break;
                            default:
                                throw new NotImplementedException("PixelFormat " + data.PixelFormat + " not implemented");
                        }
                        GCHandle gcHandle;
                        DataRectangle dataRectangle = DataRectangle.FromArray(colorsArgb, bitmap.Width * Engine.GraphicsDevice.GetSizeOf(TextureFormat.RGBA32_Float), out gcHandle);
                        try
                        {
                            Engine.GraphicsDevice.RenderContext.UpdateContext.Update(handle, 0, dataRectangle);
                        }
                        finally
                        {
                            gcHandle.Free();
                        }
                    }
                    finally
                    {
                        bitmap.UnlockBits(data);
                    }
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            
        }
    }
}
