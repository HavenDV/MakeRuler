using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Slice
    {
        public SortedDictionary<int, Row> Rows { get; set; }
        public Bitmap Bitmap { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Row CenterRow
        {
            get
            {
                var center = ((Rows.First().Key + Rows.Last().Key)/2.0).Round();
                if (Rows.ContainsKey(center))
                {
                    return Rows[center];
                }

                return new Row();
            }
        }

        public Row CenterColumn 
        {
            get 
            {
                var newRow = new Row();
                if (CenterRow.Data.Count == 0)
                {
                    return newRow;
                }

                var center = (CenterRow.Data.First().Key + (CenterRow.Data.Last().Key - CenterRow.Data.First().Key) / 2.0).Round();
                foreach (var row in Rows)
                {
                    newRow.Data[row.Key] = row.Value.Data.ContainsKey(center) ?
                        row.Value.Data[center] : 0;
                }
                
                return newRow;
            }
        }

        public Slice()
        {
            Rows = new SortedDictionary<int, Row>();
            Width = 1;
            Height = 1;
        }

        public void Add(Object obj)
        {
            foreach (var line in obj.ToLines())
            {
                var rowId = line.Key;
                if (!Rows.ContainsKey(rowId))
                {
                    Rows.Add(rowId, new Row());
                }
                Rows[rowId].AddLine(line.Value);
                Width = Math.Max(Width, Rows[rowId].Data.Last().Key);
                Height = Math.Max(Height, rowId);
            }
        }

        public void AddRow(int rowId, Row row)
        {
            Rows[rowId] = row;
            Width = Math.Max(Width, row.Width);
            Height = Math.Max(Height, rowId);
        }

        public static Color GetColor(int material)
        {
            switch (material)
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
                case 8:
                    return Color.Aqua;
            }
            return Color.Transparent;
        }

        public static Bitmap RowToBitmap(Row row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var lines = row.ToLines();
            if (lines.Count == 0)
            {
                return new Bitmap(1, 1);
            }

            var firstPixel = lines.First().Start;
            var bitmap = new Bitmap(lines.Last().End + firstPixel, 1);
            var g = Graphics.FromImage(bitmap);
            foreach (var line in lines)
            {
                var pen = new Pen(GetColor(line.Material), 1);
                g.DrawLine(pen, line.Start, 0, line.End, 0);
            }

            return bitmap;
        }

        public Bitmap ToBitmap()
        {
            // + 3 For edges on bottom and right
            var bitmap = new Bitmap(Width + 3, Height + 3);
            var g = Graphics.FromImage(bitmap);
            foreach (var row in Rows)
            {
                g.DrawImage(RowToBitmap(row.Value), new Point(0, row.Key));
            }

            return bitmap;
        }

        public string ToText(int sliceId, bool isSimple = false)
        {
            return Conventer.ToText(new KeyValuePair<int, Slice>(sliceId, this), isSimple);
        }

        static public KeyValuePair<int, Slice> FromText(string text)
        {
            return Conventer.SliceFromText(text);
        }
    }
}