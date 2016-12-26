﻿using System;
using System.Collections.Generic;
using System.Linq;
using MakeRuler.Extensions;

namespace MakeRuler
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

            var material = Data.First().Value;
            var start = Data.First().Key + 1;
            for (var i = start; i <= Data.Last().Key; ++i)
            {
                if (Data[i] != material)
                {
                    lines.Add(new Line(start, i, material));
                    material = Data[i];
                    start = i;
                }
            }
            lines.Add(new Line(start, Data.Last().Key, material));

            return lines;
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