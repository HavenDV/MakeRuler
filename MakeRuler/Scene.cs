using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeRuler
{
    public class Scene
    {
        public SortedDictionary<int, Slice> Slices { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; set; }

        public Scene()
        {
            Slices = new SortedDictionary<int, Slice>();
            Width = 1;
            Height = 1;
            Depth = 300;
        }

        public void AddSlice(int sliceId, Slice slice)
        {
            Slices.Add(sliceId, slice);
            Width = Math.Max(Width, slice.Width);
            Height = Math.Max(Height, slice.Height);
        }

        public Bitmap ToFrontBitmap()
        {
            var bitmap = new Bitmap(Width, Depth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var slice in Slices)
            {
                var centerRow = slice.Value.CenterRow;
                graphics.DrawImage(Slice.RowToBitmap(centerRow), new Point(0, (int)(Depth - Depth * (slice.Key-0.5) / Slices.Count)));
            }

            return bitmap;
        }

        public Bitmap ToSideBitmap()
        {
            var bitmap = new Bitmap(Height, Depth);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var slice in Slices)
            {
                var centerColumn = slice.Value.CenterColumn;
                graphics.DrawImage(Slice.RowToBitmap(centerColumn), new Point(0, (int)(Depth - Depth * (slice.Key - 0.5) / Slices.Count)));
            }

            return bitmap;
        }

        public string ToText()
        {
            return Conventer.ToText(this);
        }

        static public Scene FromText(string text)
        {
            return Conventer.SceneFromText(text);
        }

        public void ToFile(string path)
        {
            Conventer.ToFile(this, path);
        }

        static public Scene FromFile(string path)
        {
            return Conventer.SceneFromFile(path);
        }
    }
}
