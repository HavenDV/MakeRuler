using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MakeRuler.Extensions;

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
                            return ToString(rowLine.End, 5) + ToString(rowLine.Material, 4);
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

        public static KeyValuePair<int, Row> RowFromText(string rowData, string areasData)
        {
            //Expected : 
            //ROW NR.   1  FIRST PIXEL:   1  NUMBER OF AREAS:  0
            var rowId = int.Parse(rowData.Substring(7, 4).Trim(' '));
            var startPixel = int.Parse(rowData.Substring(25, 4).Trim(' '));
            var numberOfAreas = int.Parse(rowData.Substring(47, 3).Trim(' '));

            //Expected : 
            //  357   1  364   6  653   1
            // or
            //
            var lines = new List<Line>();
            var start = startPixel;
            var offset = 0;
            for (var i = 0; i < numberOfAreas; ++i)
            {
                var endPixel = int.Parse(areasData.Substring(offset, 5).Trim(' '));
                var material = int.Parse(areasData.Substring(offset + 5, 4).Trim(' '));
                lines.Add(new Line(start, endPixel, material));
                start = endPixel;
                offset += 9;
            }

            var row = new Row();
            foreach (var line in lines)
            {
                row.AddLine(line);
            }
            return new KeyValuePair<int, Row>(rowId, row);
        }

        public static SortedDictionary<int, Scene> SceneFromText(string text)
        {
            var scene = new SortedDictionary<int, Scene>();
            var lines = text.ToLines();
            for (var i = 0; i < lines.Count; ++i)
            {
                //Expected: 
                //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
                // or
                //
                var sliceData = lines[i];
                if (string.IsNullOrWhiteSpace(sliceData))
                {
                    break;
                }

                var layer = new Scene();
                var sliceNumber = int.Parse(sliceData.Substring(13, 3).Trim(' '));
                var firstRow = int.Parse(sliceData.Substring(28, 3).Trim(' '));
                var lastRow = int.Parse(sliceData.Substring(42, 3).Trim(' '));

                //Expected: 2 * rowCount Rows
                ++i;
                var rowCount = lastRow - firstRow + 1;
                for (var j = 0; j < rowCount; ++j)
                {
                    var row = RowFromText(lines[i], lines[i + 1]);
                    layer.AddRow(row.Key, row.Value);
                    i += 2;
                }

                scene.Add(sliceNumber, layer);
            }

            return scene;
        }

        public static SortedDictionary<int, Scene> FromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("File is not exists");
            }

            return SceneFromText(File.ReadAllText(path));
        }
    }
}
