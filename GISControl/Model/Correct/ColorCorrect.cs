using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GISControl.Model.Help;

namespace GISControl.Model.Correct
{
    public abstract class ColorCorrect
    {
        protected virtual byte GetCorrectPixel(byte pixelColor, double value)
        {
            return 0;
        }

        public BitmapImage Correct(BitmapImage image, double value)
        {
            Bitmap correctImage = BitmapConventer.ToBitmap(image);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            int imageWidth = correctImage.Width;
            int imageHeight = correctImage.Height;

            Rectangle rect = new Rectangle(0, 0, imageWidth, imageHeight);
            BitmapData bmpData = correctImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * imageHeight;
            byte[] pixelBytes = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixelBytes, 0, bytes);


            Parallel.For(0, bytes,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                i =>
                {
                    pixelBytes[i] = GetCorrectPixel(pixelBytes[i], value);
                });

            System.Runtime.InteropServices.Marshal.Copy(pixelBytes, 0, ptr, bytes);
            correctImage.UnlockBits(bmpData);
            return BitmapConventer.ToBitmapImage(correctImage);
        }
    }
}
