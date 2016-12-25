using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler.Extensions
{
    public static class Extensions
    {
        public static int Round(this double value)
        {
            //return Convert.ToInt32(value);
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int RoundMax(this double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int RoundMin(this double value)
        {
            var smallest = Math.Floor(value);
            var delta = value - smallest;
            if (delta <= 0.5)
            {
                return (int)smallest;
            }
            return (int)(smallest + 1.0);
        }

        public static string ToText(this Scene scene, int slice, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Scene>(slice, scene), isSimple);
        }

        public static string ToText(this Row row, int rowId, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Row>(rowId, row), isSimple);
        }
    }
}
