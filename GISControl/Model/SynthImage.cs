using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using GISControl.Model.ColorPalette;
using GISControl.Model.Help;

namespace GISControl.Model
{
    public abstract class SynthImage
    {
        protected double coeff;
        protected ColorPalette.ColorPalette colorPalette;

        protected virtual Color GetIndex(Color[] pixel)
        {
            return Color.Black;
        }

        public BitmapImage GetIndexImage(Layer[] map, Palette palette = Palette.Green)
        {
            try
            {
                colorPalette.SetPalette(palette);
                int imageWidth = map[0].image.PixelWidth;
                int imageHeight = map[0].image.PixelWidth;

                // Минимальный размер изображение
                for (int i = 1; i < map.Length; i++)
                {
                    if (imageHeight > map[i].image.PixelHeight)
                        imageHeight = map[i].image.PixelHeight;
                    if (imageWidth > map[i].image.PixelWidth)
                        imageWidth = map[i].image.PixelWidth;
                }

                Bitmap indexMap = new Bitmap(imageWidth, imageHeight);
                Bitmap[] mapImage = new Bitmap[map.Length];
                Color[] mapImagePixel = new Color[map.Length];
                for (int i = 0; i < map.Length; i++)
                {
                    mapImage[i] = new Bitmap(BitmapConventer.ToBitmap(map[i].image), imageWidth, imageHeight);
                }

                for (int i = 0; i < imageWidth; i++)
                {
                    for (int j = 0; j < imageHeight; j++)
                    {
                        for (int k = 0; k < map.Length; k++)
                        {
                            mapImagePixel[k] = mapImage[k].GetPixel(i, j);
                        }
                        indexMap.SetPixel(i, j, GetIndex(mapImagePixel));
                    }
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return BitmapConventer.ToBitmapImage(indexMap);
            }
            catch
            {
                MessageBox.Show("Произошла внуренняя ошибка расчета", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return null;
            }
        }

        public void SetCoeff(double value)
        {
            coeff = value;
        }
    }
}
