using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMK_Assistant
{
    public static class KeyboardFunctions
    {

        public static int Modulo(int value, int modbase)
        {
            int neg_adj = 0;
            if (value < 0)
            {
                neg_adj = modbase;
            }

            return value % modbase + neg_adj;
        }

        public static ColorBrightness GetColorBrightness(double r, double g, double b)
        {
            double bright = Math.Sqrt(.299 * Math.Pow(r, 2) + .587 * Math.Pow(g, 2) + .144 * Math.Pow(b, 2));

            if (bright > 127.5)
            {
                return ColorBrightness.Light;
            }
            return ColorBrightness.Dark;
        }


        public static ColorBrightness GetTooDark(double r, double g, double b)
        {
            double bright = Math.Sqrt(.299 * Math.Pow(r, 2) + .587 * Math.Pow(g, 2) + .144 * Math.Pow(b, 2));

            if (bright > 31.5)
            {
                return ColorBrightness.Light;
            }
            return ColorBrightness.Dark;
        }



    }
}
