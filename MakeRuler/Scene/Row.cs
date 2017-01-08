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
        public List<Line> Lines { get; private set; }

        public int Width
        {
            get { return Data.Count > 0 ? Data.Last().Key : 0; }
            //get { return Lines.Count > 0 ? Lines.Last().End : 0; }
        }

        public bool IsEmpty 
        {
            get { return Data.Count == 0; }
            //get { return Lines.Count == 0; }
        }

        public int Center {
            get { return (Data.First().Key - 1 + (Data.Last().Key - Data.First().Key) / 2.0).Round(); }
            //get { return (Lines.First().Start + (Lines.Last().End - Lines.First().Start) / 2.0).Round(); }
        }

        public int GetValue(int key)
        {
            return Data.ContainsKey(key + 1) ? Data[key + 1] : Constants.AirMaterial;
            //for (var i = 0; i < Lines.Count; ++i)
            //{
            //    if (Lines[i].Contains(key))
            //    {
            //        return Lines[i].Material;
            //    }
            //}

            //return Constants.AirMaterial;
        }

        public Row()
        {
            Data = new SortedDictionary<int, int>();
            Lines = new List<Line>();
        }

        public void AddLine(Line line)
        {
            for (var i = line.Start; i < line.End; ++i)
            {
                Data[i + 1] = line.Material;
            }
            /*
            for (var i = 0; i < Lines.Count; ++i)
            {
                if (Lines[i].Contains(line.Start))
                {
                    if (Lines[i].End > line.End)
                    {
                        Lines.Add(new Line(line.End, Lines[i].End, Lines[i].Material));
                    }
                    Lines[i].End = line.Start;
                }
                if (Lines[i].Contains(line.End))
                {
                    if (Lines[i].Start < line.Start)
                    {
                        Lines.Add(new Line(Lines[i].Start, line.Start, Lines[i].Material));
                    }
                    //Lines[i].Start = line.End;
                }
            }
            Lines.Add(line);
            Lines.Sort(new Comparison<Line>((line1, line2) => line1.Start));
            */
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
                    Data[i] = Constants.AirMaterial;
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
            return TextConventer.ToText(new KeyValuePair<int, Row>(rowId, this), isSimple);
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

            return TextConventer.RowFromText(lines[0], lines[1]);
        }
    }
}