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
        public string m_text = null;

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
                    var pen = new Pen(GetColor(line.Material), 1);
                    g.DrawLine(pen, line.Start, row.Key, line.End, row.Key);
                }
            }

            m_bitmap = bitmap;

            return bitmap;
        }
    }
}