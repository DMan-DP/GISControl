using System;

namespace GISControl.Model.Correct
{
    class BrightnessCorrect : ColorCorrect
    {
        protected override byte GetCorrectPixel(byte pixelColor, double value)
        {
            int color = pixelColor + Convert.ToInt32(value);

            if (color < 0) color = 0;
            if (color > 255) color = 255;

            return Convert.ToByte(color);
        }
    }
}
