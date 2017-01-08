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
        public Bitmap PerspectiveBitmap { get; set; }
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Row CenterRow
        {
            get {
                if (Rows.Count == 0)
                {
                    return new Row();
                }

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
                if (CenterRow.IsEmpty)
                {
                    return newRow;
                }

                var center = CenterRow.Center;
                foreach (var row in Rows)
                {
                    newRow.Data[row.Key] = row.Value.GetValue(center);
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
                Width = Math.Max(Width, Rows[rowId].Width);
                Height = Math.Max(Height, rowId);
            }
        }

        public void AddRow(int rowId, Row row)
        {
            Rows[rowId] = row;
            Width = Math.Max(Width, row.Width);
            Height = Math.Max(Height, rowId);
        }

        public Bitmap ToBitmap()
        {
            return BitmapConventer.ToBitmap(this);
        }

        public string ToText(int sliceId, bool isSimple = false)
        {
            return TextConventer.ToText(new KeyValuePair<int, Slice>(sliceId, this), isSimple);
        }

        static public KeyValuePair<int, Slice> FromText(string text)
        {
            return TextConventer.SliceFromText(text);
        }
    }
}