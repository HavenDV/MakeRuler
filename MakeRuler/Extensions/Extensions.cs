using System;
using System.Collections.Generic;

namespace MakeRuler.Extensions
{
    public static class Extensions
    {
        public static string ToText(this Slice slice, int sliceId, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Slice>(sliceId, slice), isSimple);
        }

        public static string ToText(this Row row, int rowId, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Row>(rowId, row), isSimple);
        }
    }
}
