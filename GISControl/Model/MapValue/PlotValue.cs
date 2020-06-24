using System.Dynamic;

namespace GISControl.ModelPalette
{
    public class PlotValue
    {
        public double[,] values { get; private set; } = new double[20,2];

        public PlotValue(double[] values)
        {
            for (int i = 0; i < 20; i++)
            {
                this.values[i,0] = values[i];
                this.values[i, 1] = 0;
            }
        }

        public void SetIndexValue(double value)
        {
            if (value >= values[0,0] && value < values[1,0]) ++values[0,1];
            else if (value >= values[1,0] && value < values[2,0]) ++values[1,1];
            else if (value >= values[2,0] && value < values[3,0]) ++values[2,1];
            else if (value >= values[3,0] && value < values[4,0]) ++values[3,1];
            else if (value >= values[4,0] && value < values[5,0]) ++values[4,1];
            else if (value >= values[5,0] && value < values[6,0]) ++values[5,1];
            else if (value >= values[6,0] && value < values[7,0]) ++values[6,1];
            else if (value >= values[7,0] && value < values[8,0]) ++values[7,1];
            else if (value >= values[8,0] && value < values[9,0]) ++values[8,1];
            else if (value >= values[9,0] && value < values[10,0]) ++values[9,1];
            else if (value >= values[10,0] && value < values[11,0]) ++values[10,1];
            else if (value >= values[11,0] && value < values[12,0]) ++values[11,1];
            else if (value >= values[12,0] && value < values[13,0]) ++values[12,1];
            else if (value >= values[13,0] && value < values[14,0]) ++values[13,1];
            else if (value >= values[14,0] && value < values[15,0]) ++values[14,1];
            else if (value >= values[15,0] && value < values[16,0]) ++values[15,1];
            else if (value >= values[16,0] && value < values[17,0]) ++values[16,1];
            else if (value >= values[17,0] && value <= values[18,0]) ++values[17,1];
        }
    }
}
