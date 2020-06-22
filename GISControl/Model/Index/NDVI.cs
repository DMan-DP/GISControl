using System;
using System.Drawing;

namespace GISControl.Model.Index
{
    class NDVI : SynthImage
    {
        public NDVI()
        {
            double[] valuesNDVI = new double[] {-1, 0, 0.033, 0.066, 0.1, 0.133, 0.166, 
                0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.6, 0.7, 0.8, 0.9, 1, 1};
            colorPalette = new ColorPalette.ColorPalette(valuesNDVI);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double nir = Convert.ToDouble(pixel[0].R) / 255.0;
            double red = Convert.ToDouble(pixel[1].R) / 255.0;
            double ndvi;

            if (nir + red <= 0)
            {
                ndvi = 0;
            }
            else
            {
                ndvi = (nir - red) / (red + nir);
            }
            return colorPalette.GetColorPallete(ndvi);
        }
    }
}
