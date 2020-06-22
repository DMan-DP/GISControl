using System;
using System.Drawing;

namespace GISControl.Model.Index
{
    class SAVI : SynthImage
    {
        public SAVI()
        {
            double[] valuesNDVI = new double[] {-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4,
                -0.3, -0.2, -0.1, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1};
            colorPalette = new ColorPalette.ColorPalette(valuesNDVI);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double red = Convert.ToDouble(pixel[0].R) / 255.0;
            double nir = Convert.ToDouble(pixel[1].R) / 255.0;
            double savi;

            if (nir + red + coeff <= 0)
            {
                savi = 0;
            }
            else
            {
                savi = (nir - red) / (red + nir + coeff) * (1 + coeff);
            }
            return colorPalette.GetColorPallete(savi);
        }
    }
}
