using System.Collections.Generic;
using System.Linq;

namespace Example1
{
    public class Row
    {

        public SortedDictionary<int, int> Data { get; set; }

        public Row()
        {
            Data = new SortedDictionary<int, int>();
        }

        public void AddLine(Line line)
        {
            for (var i = line.Start; i <= line.End; ++i)
            {
                Data[i] = line.Medium;
            }
        }

        public List<Line> ToLines()
        {
            var lines = new List<Line>();

            var currentMedium = Data.First().Value;
            var start = Data.First().Key;
            for (var i = start;i <= Data.Last().Key; ++i)
            {
                if (Data[i] != currentMedium)
                {
                    lines.Add(new Line(start, i, currentMedium));
                    currentMedium = Data[i];
                    start = i;
                }
            }
            lines.Add(new Line(start, Data.Last().Key, currentMedium));

            return lines;
        }
    }
}