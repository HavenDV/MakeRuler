using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MakeRuler
{
    public class Slice
    {
        public Dictionary<int, Row> Rows { get; set; }
        public Bitmap Bitmap { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Slice()
        {
            Rows = new Dictionary<int, Row>();
            Width = 1;
            Height = 1;
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
                Width = Math.Max(Width, Rows[rowId].Data.Last().Key);
                Height = Math.Max(Height, rowId);
            }
        }

        public void AddRow(int rowId, Row row)
        {
            Rows[rowId] = row;
            Width = Math.Max(Width, Rows[rowId].Data.Count > 0 ? Rows[rowId].Data.Last().Key : Width);
            Height = Math.Max(Height, rowId);
        }

        private Color GetColor(int material)
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
            }
            return Color.White;
        }

        public Bitmap RowToBitmap(Row row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            var lines = row.ToLines();
            if (lines.Count == 0)
            {
                return new Bitmap(1,1);
            }

            var bitmap = new Bitmap(lines.Last().End, 1);
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
            var bitmap = new Bitmap(Width, Height);
            var g = Graphics.FromImage(bitmap);
            foreach (var row in Rows)
            {
                g.DrawImage(RowToBitmap(row.Value), new Point(0, row.Key));
            }

            return bitmap;
        }
    }
}