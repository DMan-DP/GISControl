using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GISControl.Model.Help
{
    public static class BitmapConventer
    {
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		public static BitmapSource ToBitmapSource(Bitmap bitmap)
		{
			IntPtr hBitmap = bitmap.GetHbitmap();
			BitmapSource retval;

			try
			{
				retval = Imaging.CreateBitmapSourceFromHBitmap(
							 hBitmap,
							 IntPtr.Zero,
							 Int32Rect.Empty,
							 BitmapSizeOptions.FromEmptyOptions());
			}
			finally
			{
				DeleteObject(hBitmap);
			}

			return retval;
		}

		public static Bitmap ToBitmap(BitmapImage bitmapImage)
		{
			using (MemoryStream outStream = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

				return new Bitmap(bitmap);
			}
		}

        public static BitmapImage ToBitmapImage(Bitmap bitmap, ImageFormat format = null)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
				if (format == null) format = ImageFormat.Png;
                bitmap.Save(stream, format);
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public static WriteableBitmap ToWriteableBitmap(BitmapImage bitmapImage)
        {
			return new WriteableBitmap(bitmapImage);
        }
    }
}
