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
    }
}
