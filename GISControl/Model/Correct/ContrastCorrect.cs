using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISControl.Model.Correct
{
    public class ContrastCorrect : ColorCorrect
    {
        protected override byte GetCorrectPixel(byte pixelColor, double value)
        {
            double contrast = (100.0 + value) / 255.0;
            contrast = contrast * contrast;
            double color = Convert.ToDouble(pixelColor) / 255.0;
            color -= 0.5;
            color *= contrast;
            color += 0.5;
            color *= 255;

            if (color < 0) color = 0;
            if (color > 255) color = 255;

            return Convert.ToByte(color);
        }
    }
}
