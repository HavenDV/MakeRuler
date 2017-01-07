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
        public double Step { get; set; }
        public double XYScale { get; set; }
        public List<IObject3D> Objects { get; set; }


        public Scene()
        {
            Slices = new SortedDictionary<int, Slice>();
            Width = 1;
            Height = 1;
            Depth = 1;
            Step = Depth;
            XYScale = 1.0;
            Objects = new List<IObject3D>();
        }

        public void AddSlice(int sliceId, Slice slice)
        {
            Slices.Add(sliceId, slice);
            Width = Math.Max(Width, slice.Width);
            Height = Math.Max(Height, slice.Height);
        }

        public void AddObject(IObject3D obj)
        {
            Objects.Add(obj);
        }

        public async Task<Scene> WithComputedSlices()
        {
            await Task.WhenAll(
                Enumerable.Range(1, (int)(Depth / Step)).Select(
                    sliceId => Task.Run(() =>
                        {
                            var h = (sliceId - 1) * Step / Depth;
                            var slice = new Slice();
                            foreach (var obj in Objects)
                            {
                                slice.Add(obj.GetObject(h, XYScale));
                            }
                            slice.Bitmap = slice.ToBitmap();
                            slice.Text = slice.ToText(sliceId, false);
                            AddSlice(sliceId, slice);
                        }
                    )
                )
            );

            return this;
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
