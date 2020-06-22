using System;
using System.Drawing;

namespace GISControl.Model.Index
{
    class ARVI : SynthImage
    {
        public ARVI()
        {
            double[] valuesNDVI = new double[] {-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4,
                -0.3, -0.2, -0.1, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1};
            colorPalette = new ColorPalette.ColorPalette(valuesNDVI);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double nir = Convert.ToDouble(pixel[0].R) / 255.0;
            double red = Convert.ToDouble(pixel[1].R) / 255.0;
            double blue = Convert.ToDouble(pixel[2].R) / 255.0;
            double arvi;
            double rb = red - coeff * (red - blue);

            if (nir + rb == 0)
            {
                if (nir - rb == 0)
                    arvi = 0;
                else
                    arvi = nir - rb;
            }
            else
            {
                arvi = (nir - rb) / (red + rb);
            }
            return colorPalette.GetColorPallete(arvi);
        }
    }
}