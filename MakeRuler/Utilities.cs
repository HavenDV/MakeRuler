using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler
{
    static class Utilities
    {
        public static int Round(this double value)
        {
            return Convert.ToInt32(value);
        }
    }
}
