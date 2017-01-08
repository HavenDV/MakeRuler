using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Row
    {
        public SortedDictionary<int, int> Data { get; private set; }

        public int Width
        {
            get { return Data.Count > 0 ? Data.Last().Key : 0; }
        }

        public Row()
        {
            Data = new SortedDictionary<int, int>();
        }

        public void AddLine(Line line)
        {
            for (var i = line.Start; i < line.End; ++i)
            {
                Data[i + 1] = line.Material;
            }
        }

        public List<Line> ToLines()
        {
            var lines = new List<Line>();

            if (Data.Count == 0)
            {
                return lines;
            }

            var start = Data.First().Key - 1;
            var material = Data.First().Value;
            for (var i = Data.First().Key; i <= Data.Last().Key; ++i)
            {
                if (!Data.ContainsKey(i))
                {
                    Data[i] = 0;
                }

                if (Data[i] != material)
                {
                    lines.Add(new Line(start, i - 1, material));
                    material = Data[i];
                    start = i - 1;
                }
            }
            if (start < Data.Last().Key)
            {
                lines.Add(new Line(start, Data.Last().Key, material));
            }

            return lines;
        }

        public string ToText(int rowId, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Row>(rowId, this), isSimple);
        }

        public static KeyValuePair<int, Row> FromText(string text)
        {
            var lines = text.ToLines();
            if (lines.Count < 2)
            {
                throw new ArgumentException("Text contains less 2 lines");
            }
            if (lines.Count > 2)
            {
                Debug.WriteLine($"Text contains more 2 lines: Text: {text}");
            }

            return Conventer.RowFromText(lines[0], lines[1]);
        }
    }
}