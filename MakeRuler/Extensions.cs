using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler.Extensions
{
    static class Extensions
    {
        public static int Round(this double value)
        {
            //return Convert.ToInt32(value);
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }
    }
}
