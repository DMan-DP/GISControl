using System;
using System.Drawing;
using GISControl.ModelPalette;

namespace GISControl.Model.Index
{
    class MSAVI : SynthImage
    {
        public MSAVI()
        {
            double[] values = new double[] {-1, -0.9, -0.8, -0.7, -0.6, -0.5, -0.4,
                -0.3, -0.2, -0.1, 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1};
            colorPalette = new MapValue.ColorPalette(values);
            plotValue = new PlotValue(values);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double nir = Convert.ToDouble(pixel[0].R) / 255.0;
            double red = Convert.ToDouble(pixel[1].R) / 255.0; double msavi;
            double L = 1 - ((2 * nir + 1 - Math.Sqrt(Math.Pow(2 * nir + 1, 2) - 8 * (nir - red))) / 2);

            if (nir + red + L <= 0)
            {
                msavi = 0;
            }
            else
            {
                msavi = (nir - red) / (red + nir + L) * (1 + L);
            }

            plotValue.SetIndexValue(msavi);
            return colorPalette.GetColorPallete(msavi);
        }
    }
}