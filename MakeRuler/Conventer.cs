using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler
{
    public static class Conventer
    {

        public static string ToString(int value, int lenght)
        {
            return value.ToString().PadLeft(lenght);
        }

        //Simple row
        //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  1
        //  173   1
        //No simple row
        //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  3
        //  152   1  168   6  247   1
        public static string ToText(KeyValuePair<int, Row> row, bool isSimple)
        {
            var lines = new List<string>();
            var rowLines = row.Value.ToLines();
            lines.Add($"ROW NR.{ToString(row.Key, 4)}  FIRST PIXEL:{ToString(rowLines.Count > 0 ? rowLines[0].Start : 1, 4)}  NUMBER OF AREAS:{ToString(isSimple ? 1 : rowLines.Count, 3)}");

            if (!isSimple)
            {
                lines.Add(string.Concat(
                        rowLines.Select(rowLine =>
                        {
                            return ToString(rowLine.End, 5) + ToString(rowLine.Medium, 4);
                        })
                    )
                );
            }
            else if (isSimple && rowLines.Count > 0)
            {
                lines.Add(ToString(rowLines.Last().End, 5) + ToString(1, 4));
            }
            else
            {
                lines.Add(string.Empty);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public static string ToText(KeyValuePair<int, Scene> layer, bool isSimple)
        {
            var lines = new List<string>();
            var rows = layer.Value.Rows;
            //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
            lines.Add($"SLICE NUMBER:{ToString(layer.Key, 3)}  FIRST ROW:{ToString(rows.First().Key, 3)}  LAST ROW:{ToString(rows.Last().Key, 3)}");
            foreach (var row in rows)
            {
                lines.Add(ToText(row, isSimple));
            }

            return string.Join(Environment.NewLine, lines);
        }

        public static void ToFile(KeyValuePair<int, Scene> layer, string path)
        {
            File.WriteAllText(path, ToText(layer, false));
        }
    }
}
