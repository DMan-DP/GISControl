using System;
using System.Drawing;

namespace GISControl.Model.Index
{
    class IPVI : SynthImage
    {
        public IPVI()
        {
            double[] valuesNDVI = new double[] {0, 0.05, 0.1, 0.15, 0.2, 0.25, 0.3,
                0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.65, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1};
            colorPalette = new ColorPalette.ColorPalette(valuesNDVI);
        }

        protected override Color GetIndex(Color[] pixel)
        {
            double nir = Convert.ToDouble(pixel[0].R) / 255.0;
            double red = Convert.ToDouble(pixel[1].R) / 255.0; 
            double ipvi;
            
            if (nir + red == 0)
            {
                if (nir == 0)
                    ipvi = 0;
                else
                    ipvi = nir;
            }
            else
            {
                ipvi = nir / (red + nir);
            }
            return colorPalette.GetColorPallete(ipvi);
        }
    }
}
