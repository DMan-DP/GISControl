using System;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FreeImageAPI;

namespace GISControl.Model
{
    public class Layer
    {
        //Mask file XXXXXX_YYYYMMDDTHHMMSS_CCC_RRm
        private static string nameMask = "_";
        public string name { get; private set; }
        public string data { get; private set; }
        public string resolution { get; private set; }
        public BitmapImage image { get; private set; }
        public BitmapImage preview { get; private set; }

        public Layer(string fileName, string filePath)
        {
            string[] subNames = Regex.Split(fileName, nameMask);
            name = fileName;
            data = "NaN";
            resolution = "NaN";

            image = new BitmapImage();
            image.BeginInit();

            preview = new BitmapImage();
            preview.BeginInit();
            preview.DecodePixelWidth = 64;
            image.CacheOption = BitmapCacheOption.OnLoad;

            var ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".jp2" || ext == ".jpg2" || ext == ".j2k")
            {
                FIBITMAP dib = FreeImage.LoadEx(filePath);
                using (var memory = new MemoryStream())
                {
                    FreeImage.GetBitmap(dib).Save(memory, ImageFormat.Png);

                    memory.Position = 0;
                    image.StreamSource = memory;
                    image.EndInit();
                    image.Freeze();

                    memory.Position = 0;
                    preview.StreamSource = memory;
                    preview.CacheOption = BitmapCacheOption.OnLoad;
                    preview.EndInit();
                }
            }
            else
            {
                image.CacheOption = BitmapCacheOption.None;
                image.UriSource = new Uri(filePath, UriKind.Absolute);
                image.EndInit();
                image.Freeze();

                preview.UriSource = new Uri(filePath, UriKind.Absolute);
                preview.EndInit();
            }
            resolution = Convert.ToString(image.PixelWidth) + "x" + Convert.ToString(image.PixelHeight);
            if (subNames.Length >= 2)
                GetData(subNames[1]);
            if (subNames.Length >= 3)
                name = subNames[2];
        }

        public Layer(BitmapImage image, string name, BitmapEncoder encoder = null)
        {
            this.name = name;
            this.image = image;
            resolution = Convert.ToString(image.PixelWidth) + "x" + Convert.ToString(image.PixelHeight);
            preview = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                if (encoder == null) encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(memoryStream);
                memoryStream.Position = 0;
                preview.BeginInit();
                preview.DecodePixelWidth = 64;
                preview.CacheOption = BitmapCacheOption.OnLoad;
                preview.StreamSource = memoryStream;
                preview.EndInit();
                preview.Freeze();
            }

            data = DateTime.Now.ToString();
        }

        private void GetData(string text)
        {
            string year = text.Substring(0, 4);
            string month = text.Substring(4, 2);
            string day = text.Substring(6, 2);
            string hour = text.Substring(9, 2);
            string minute = text.Substring(11, 2);
            string second = text.Substring(13, 2);
            data = day + "." + month + "." + year + " " + hour + ":" + minute + ":" + second;
        }

        public async Task<BitmapImage> GetImage()
        {
            if (this.image == null) return null;
            return await Task.Run(() =>
            {
                return this.image;
            });
        }
    }
}
