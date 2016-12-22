using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Example1
{
    class Scene
    {
        public Dictionary<int, Row> Rows { get; set; }

        public Scene()
        {
            Rows = new Dictionary<int, Row>();
        }

        private bool RowExists(int rowId)
        {
            return Rows.ContainsKey(rowId);
        }

        private void AddNewRow(Line line, int rowId, int mediumId)
        {
            var row = new Row(rowId, line.Start);
            row.Areas.Add(new Area(line.End, mediumId));
            Rows.Add(rowId, row);
        }

        private void AddToRow(Line line, int rowId, int mediumId)
        {
            Rows[rowId].Areas.Add(new Area(line.Start, 1));
            Rows[rowId].Areas.Add(new Area(line.End, mediumId));
            Rows[rowId].Areas.Sort((area1, area2) =>
            {
                return area1.EndPixel.CompareTo(area2.EndPixel);
            });
        }

        public void Add(Circle circle, int mediumId)
        {
            for (int rowId = circle.p_minY; rowId <= circle.p_maxY; rowId++)
            {
                var line = circle.Lines[rowId];
                if (RowExists(rowId))
                {
                    AddToRow(line, rowId, mediumId);
                }
                else
                {
                    AddNewRow(line, rowId, mediumId);
                }
            }
        }

        public static Scene FromFile(string path)
        {
            var scene = new Scene();
            var lines = File.ReadAllLines(path);

            // Display the file contents by using a foreach loop.
            for (int i = 0; i < lines.Length; i += 2)
            {
                // Use a tab to indent each line of the file.
                string[] args = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //scene.Rows.Add(new Row(args[0], args[1]));
                string[] args1 = lines[i + 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < int.Parse(args[2]); j++)
                {
                    //rows[rows.Count - 1].Areas.Add(new Area(args1[2 * j], args1[2 * j + 1]));
                }
            }

            return scene;
        }

        public static string ToString(int value, int lenght)
        {
            return value.ToString().PadLeft(lenght);
        }

        public void ToFile(string path)
        {
            var lines = new List<string>();
            for (int i = 1; i <= 6; ++i)
            {
                //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
                lines.Add($"SLICE NUMBER:{ToString(i, 3)}  FIRST ROW:{ToString(Rows.First().Key, 4)}  LAST ROW:{ToString(Rows.Last().Key, 4)}");
                if (i == 1 || i == 6)
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  1
                    //  173   1
                    foreach (var row in Rows)
                    {
                        lines.Add("ROW NR." + ToString(row.Key, 4) + "  FIRST PIXEL:" + ToString(row.Value.FirstPixel, 4) + "  NUMBER OF AREAS:" + ToString(1, 3));
                        lines.Add(ToString(row.Value.Areas.Last().EndPixel, 5) + ToString(1, 4));
                    }
                }
                else
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  3
                    //  152   1  168   6  247   1
                    foreach (var row in Rows)
                    {
                        lines.Add("ROW NR." + ToString(row.Key, 4) + "  FIRST PIXEL:" + ToString(row.Value.FirstPixel, 4) + "  NUMBER OF AREAS:" + ToString(row.Value.Areas.Count, 3));
                        var tmp = "";
                        foreach (var area in row.Value.Areas)
                            tmp += ToString(area.EndPixel, 5) + ToString(area.Medium, 4);
                        lines.Add(tmp);
                    }
                }
            }
            File.WriteAllLines(path, lines);
        }
    }
}