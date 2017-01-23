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
        public int Depth { get; private set; }
        public double Step { get; set; }
        public double XScale { get; set; }
        public double YScale { get; set; }
        public List<Object3D> Objects { get; set; }

        public void SetDimensions(double xStep, double yStep, double zStep)
        {
            XScale = 1.0 / xStep;
            YScale = 1.0 / yStep;
            Step = zStep;
        }

        public Scene(double xStep, double yStep, double zStep)
        {
            Slices = new SortedDictionary<int, Slice>();
            SetDimensions(xStep, yStep, zStep);
            Width = 1;
            Height = 1;
            Depth = 1;
            Objects = new List<Object3D>();
        }

        public Scene() :
            this(1.0, 1.0, 1.0)
        {}

        public void AddSlice(int sliceId, Slice slice)
        {
            Slices.Add(sliceId, slice);
            Width = Math.Max(Width, slice.Width);
            Height = Math.Max(Height, slice.Height);
        }

        public void AddObject(Object3D obj)
        {
            Objects.Add(obj);
            Depth = Math.Max(Depth, obj.maxZ2);
        }

        public async Task<Scene> WithComputedSlices()
        {
            await Task.WhenAll(
                Enumerable.Range(1, (int)(Depth / Step)).Select(
                    sliceId => Task.Run(() =>
                        {
                            var slice = new Slice();
                            foreach (var obj in Objects)
                            {
                                var obj2D = obj.GetObject((sliceId - 1) * Step, XScale);
                                slice.Add(obj2D);
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
            return BitmapConventer.ToFrontBitmap(this);
        }

        public Bitmap ToSideBitmap()
        {
            return BitmapConventer.ToSideBitmap(this);
        }

        public string ToText()
        {
            return TextConventer.ToText(this);
        }

        static public Scene FromText(string text)
        {
            return TextConventer.SceneFromText(text);
        }

        public void ToFile(string path)
        {
            TextConventer.ToFile(this, path);
        }

        static public Scene FromFile(string path)
        {
            return TextConventer.SceneFromFile(path);
        }
    }
}
