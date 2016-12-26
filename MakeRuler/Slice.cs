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

        public Slice()
        {
            Rows = new Dictionary<int, Row>();
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

        public void AddRow(int rowId, Row row)
        {
            Rows[rowId] = row;
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

        public Bitmap ToBitmap(int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(bitmap);
            foreach (var row in Rows)
            {
                foreach (var line in row.Value.ToLines())
                {
                    var pen = new Pen(GetColor(line.Material), 1);
                    g.DrawLine(pen, line.Start, row.Key, line.End, row.Key);
                }
            }

            return bitmap;
        }
    }
}