﻿using System;
using System.Collections.Generic;
using System.Linq;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Row
    {
        public SortedDictionary<int, int> Data { get; set; }

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
            for (var i = line.Start; i <= line.End; ++i)
            {
                Data[i] = line.Material;
            }
        }

        public List<Line> ToLines()
        {
            var lines = new List<Line>();

            if (Data.Count == 0)
            {
                return lines;
            }

            var start = Data.First().Key;
            var material = Data.First().Value;
            for (var i = start; i <= Data.Last().Key; ++i)
            {
                if (!Data.ContainsKey(i))
                {
                    Data[i] = 0;
                }

                if (Data[i] != material && start < i - 1)
                {
                    lines.Add(new Line(start, i - 1, material));
                    material = Data[i];
                    start = i;
                }
            }
            lines.Add(new Line(start, Data.Last().Key, material));

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
            
            return Conventer.RowFromText(lines[0], lines[1]);
        }
    }
}