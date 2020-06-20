using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GISControl.Model.Help
{
    public class BitmapConventer
    {
		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		public static BitmapSource Bitmap2BitmapSource(Bitmap bitmap)
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

		public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
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
    }
}
