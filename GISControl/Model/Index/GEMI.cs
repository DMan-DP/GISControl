using System;
using System.Drawing;

namespace GISControl.Model.Index
{
    class GEMI : SynthImage
    {
        public GEMI()
        {
            double[] valuesNDVI = new double[] {0, 0.05, 0.1, 0.15, 0.2, 0.25, 0.3,
                0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1};
            colorPalette = new ColorPalette.ColorPalette(valuesNDVI);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double nir = Convert.ToDouble(pixel[0].R) / 255.0;
            double red = Convert.ToDouble(pixel[1].R) / 255.0; 
            double gemi;
            double E = (2 * (nir * nir - red * red) + 1.5 * nir + 0.5 * red) / 
                       (nir + red + 0.5);
            if (1 - red == 0)
            {
                if (red - 0.125 == 0)
                    gemi = E * (1 - 0.25 * E);
                else
                    gemi = E * (1 - 0.25 * E) - (red - 0.125);
            }
            else
            {
                gemi = E * (1 - 0.25 * E) - (red - 0.125) / (1 - red);
            }
            return colorPalette.GetColorPallete(gemi);
        }
    }
}