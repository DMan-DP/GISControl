using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GISControl.Model;
using GISControl.Model.Correct;
using GISControl.Model.MapValue;

namespace GISControl.ViewModel
{
    static class Controller
    {
        public static async Task<BitmapImage> CollorCorrectTask(ColorCorrect colorCorrect, BitmapImage image, double value)
        {
            return await Task.Run(() =>
            {
                return colorCorrect.Correct(image, value);
            });
        }

        public static async Task<BitmapImage> SynthImageTask(SynthImage synthImage, Layer[] layers, Palette palette)
        {
            return await Task.Run(() =>
            {
                return synthImage.GetIndexImage(layers, palette);
            });
        }
    }
}
