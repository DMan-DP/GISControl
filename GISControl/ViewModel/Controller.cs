using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using GISControl.Model.Correct;

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
    }
}
