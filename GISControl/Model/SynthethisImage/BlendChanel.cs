using System;
using System.Drawing;

namespace GISControl.Model.SynthethisImage
{
    class BlendChanel : SynthImage
    {
        protected override Color GetIndex(Color[] pixel)
        {
            return Color.FromArgb(pixel[0].R, pixel[1].G, pixel[2].B);
        }
    }
}
