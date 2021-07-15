using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit.Client.Web.BlazorUI
{
    public class HSV
    {
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Value { get; set; }

        public RGB ToRGB()
        {
            var c = Value * Saturation;
            var x = c * (1 - Math.Abs((Hue / 60) % 2 - 1));
            var m = Value - c;

            (double r, double g, double b) color =
                  Hue >= 0 && Hue < 60 ? (c, x, 0)
                : Hue >= 60 && Hue < 120 ? (x, c, 0)
                : Hue >= 120 && Hue < 180 ? (0, c, x)
                : Hue >= 180 && Hue < 240 ? (0, x, c)
                : Hue >= 240 && Hue < 300 ? (x, 0, c)
                : (c, 0, x);

            return new RGB()
            {
                Red = Math.Floor((color.r + m) * 255),
                Green = Math.Floor((color.g + m) * 255),
                Blue = Math.Floor((color.b + m) * 255),
            };

        }
    }
}
