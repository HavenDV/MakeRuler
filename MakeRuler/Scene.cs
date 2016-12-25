using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MakeRuler
{
    public class Scene
    {
        public Dictionary<int, Row> Rows { get; set; }
        private Bitmap m_bitmap = null;
        private string m_text = null;

        public Scene()
        {
            Rows = new Dictionary<int, Row>();
        }

        public void ClearCache()
        {
            m_bitmap = null;
            m_text = null;
        }

        public void Add(Object obj)
        {
            foreach (var line in obj.Lines)
            {
                var rowId = line.Key;
                if (!Rows.ContainsKey(rowId))
                {
                    Rows.Add(rowId, new Row());
                }
                Rows[rowId].AddLine(line.Value);
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

        //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
        //Simple row
        //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  1
        //  173   1
        //No simple row
        //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  3
        public string ToText(int slice, bool isSimple)
        {
            if (m_text != null)
            {
                return m_text;
            }

            var lines = new List<string>();
            //  152   1  168   6  247   1
            lines.Add($"SLICE NUMBER:{ToString(slice, 3)}  FIRST ROW:{ToString(Rows.First().Key, 3)}  LAST ROW:{ToString(Rows.Last().Key, 3)}");
            foreach (var row in Rows)
            {
                var rowLines = row.Value.ToLines();
                lines.Add("ROW NR." + ToString(row.Key, 4) + "  FIRST PIXEL:" + ToString(rowLines.First().Start, 4) + "  NUMBER OF AREAS:" + ToString(isSimple ? 1 : rowLines.Count, 3));

                if (isSimple)
                    lines.Add(ToString(rowLines.Last().End, 5) + ToString(1, 4));
                else
                    lines.Add(string.Concat(
                            rowLines.Select(rowLine =>
                            {
                                return ToString(rowLine.End, 5) + ToString(rowLine.Medium, 4);
                            })
                        )
                    );
            }

            var text = string.Join(Environment.NewLine, lines);

            m_text = text;

            return text;
        }

        public void ToFile(string path)
        {
            var lines = new List<string>();
            for (int i = 1; i <= 6; ++i)
            {
                lines.Add(ToText(i, i == 1 || i == 6));
            }
            File.WriteAllLines(path, lines);
        }

        private Color GetColor(int mediumID)
        {
            switch (mediumID)
            {
                case 1:
                    return Color.DarkGray;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Green;
                case 5:
                    return Color.Yellow;
                case 6:
                    return Color.Pink;
                case 7:
                    return Color.MediumSeaGreen;
            }
            return Color.White;
        }

        public Bitmap ToBitmap()
        {
            if (m_bitmap != null)
            {
                return m_bitmap;
            }

            var bitmap = new Bitmap(800, 400);
            var g = Graphics.FromImage(bitmap);
            foreach (var row in Rows)
            {
                foreach (var line in row.Value.ToLines())
                {
                    var pen = new Pen(GetColor(line.Medium), 1);
                    g.DrawLine(pen, line.Start, row.Key, line.End, row.Key);
                }
            }

            m_bitmap = bitmap;

            return bitmap;
        }
    }
}