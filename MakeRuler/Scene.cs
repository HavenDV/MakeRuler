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

        public static Scene FromFile(string path)
        {
            var scene = new Scene();
            var lines = File.ReadAllLines(path);

            // Display the file contents by using a foreach loop.
            for (int i = 0; i < lines.Length; i += 2)
            {
                // Use a tab to indent each line of the file.
                string[] args = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //scene.Rows.Add(new Row(args[0], args[1]));
                string[] args1 = lines[i + 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < int.Parse(args[2]); j++)
                {
                    //rows[rows.Count - 1].Areas.Add(new Area(args1[2 * j], args1[2 * j + 1]));
                }
            }

            return scene;
        }

        private Color GetColor(int mediumID)
        {
            switch (mediumID)
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
                    var pen = new Pen(GetColor(line.Medium), 1);
                    g.DrawLine(pen, line.Start, row.Key, line.End, row.Key);
                }
            }

            m_bitmap = bitmap;

            return bitmap;
        }
    }
}