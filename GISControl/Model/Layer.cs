using System;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace GISControl.Model
{
    class Layer
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
            image.CacheOption = BitmapCacheOption.None;
            image.UriSource = new Uri(filePath, UriKind.Absolute);
            image.EndInit();
            image.Freeze();
            resolution = Convert.ToString(image.PixelWidth) + "x" + Convert.ToString(image.PixelHeight);

            preview = new BitmapImage();
            preview.BeginInit();
            preview.UriSource = new Uri(filePath, UriKind.Absolute);
            preview.DecodePixelWidth = 64;
            preview.EndInit();

            if (subNames.Length >= 2)
                GetData(subNames[1]);
            if (subNames.Length >= 3)
                name = subNames[2];
        }

        public Layer(string name, string data, BitmapImage image)
        {
            this.name = name;
            this.data = data;
            this.image = image;
            resolution = Convert.ToString(image.PixelWidth) + "x" + Convert.ToString(image.PixelHeight);
        }

        private void GetData(string text)
        {
            string year = text.Substring(0, 4);
            string month = text.Substring(4, 2);
            string day = text.Substring(6, 2);
            string hour = text.Substring(9, 2);
            string minute = text.Substring(11, 2);
            string second = text.Substring(13, 2);
            data = hour + ":" + minute + ":" + second + " " + day + "/" + month + "/" + year;
        }
    }
}
