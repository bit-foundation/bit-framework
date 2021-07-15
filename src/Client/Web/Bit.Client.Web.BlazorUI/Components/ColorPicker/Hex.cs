using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit.Client.Web.BlazorUI
{
    public class Hex
    {
        public string ColorCode { get; set; }

        public RGB ToRGB()
        {
            return new RGB()
            {
                Red = int.Parse(ColorCode.Substring(0, 2), NumberStyles.AllowHexSpecifier),
                Green = int.Parse(ColorCode.Substring(2, 2), NumberStyles.AllowHexSpecifier),
                Blue = int.Parse(ColorCode.Substring(4, 2), NumberStyles.AllowHexSpecifier)
            };
        }
    }
}
