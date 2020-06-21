using System;

namespace GISControl.Model.Correct
{
    class InvertColorCorrect : ColorCorrect
    {
        protected override byte GetCorrectPixel(byte pixelColor, double value)
        {
            return Convert.ToByte(255 - pixelColor);
        }
    }
}
