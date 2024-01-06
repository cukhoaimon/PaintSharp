using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace US_IShape
{
    public class RelativeToAbsoluteConverter
    {
        public static string Convert(string value)
        {
            string relative = value;
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string absolute = $"{folder}{relative}";
            return absolute;
        }
    }
}
