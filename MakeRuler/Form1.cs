using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public partial class Form1 : Form
    {
        private Scene CachedScene { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private Slice CreateSlice(int slice, double step, double height)
        {
            var scene = new Slice();
            var h = (slice-1) * step / height;
            var scale = 1 / 0.5;
            //CORG(1) = 'phantom';
            scene.Add(new Rect(scale * (80 + 80 * h), scale * 40 * h, scale * (280 - 80 * h), scale * (160 - 40 * h), 1));
            scene.Add(new Circle(scale * (80 + 80 * h), scale * 80, scale * (80 - 40 * h), 1));
            scene.Add(new Circle(scale * (280 - 80 * h), scale * 80, scale * (80 - 40 * h), 1));

            //CORG(2) = 'center';
            scene.Add(new Circle(scale * 180, scale * 80, scale * 5.5, 2));

            //CORG(3) = 'left';
            scene.Add(new Circle(scale * (110 + 40 * h), scale * 80, scale * 5.5, 3));

            //CORG(4) = 'bottom';
            scene.Add(new Circle(scale * 180, scale * (140 - 40 * h), scale * 5.5, 4));

            //CORG(5) = 'right';
            scene.Add(new Circle(scale * (250 - 40 * h), scale * 80, scale * 5.5, 5));

            //CORG(6) = 'top';
            scene.Add(new Circle(scale * 180, scale * (20 + 40 * h), scale * 5.5, 6));

            scene.Bitmap = scene.ToBitmap();
            scene.Text = scene.ToText(slice, false);
            return scene;
        }

        private async Task<Scene> CreateSlices(double step, double height)
        {
            var slices = await Task.WhenAll(
                Enumerable.Range(1, (int)(1 + height / step)).Select(
                    i => Task.Run(
                        () => new KeyValuePair<int, Slice>(i, CreateSlice(i, step, height))
                    )
                )
            );
            var scene = new Scene();
            foreach (var slice in slices)
            {
                scene.AddSlice(slice.Key, slice.Value);
            }

            return scene;
        }

        private async Task<int[]> ComputeData(Scene scene)
        {
            return await Task.WhenAll(
                scene.Slices.Select(
                    slice => Task.Run(
                        () =>
                        {
                            slice.Value.Bitmap = slice.Value.Bitmap ?? slice.Value.ToBitmap();
                            slice.Value.Text = slice.Value.Text ?? slice.Value.ToText(slice.Key, false);
                            return slice.Key;
                        }
                    )
                )
            );
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            var step = 50.0;
            var height = 300.0;
            //Conventer.FromFile("CTDIcone(1).data");//
            CachedScene = CachedScene ?? await CreateSlices(step, height);
            await ComputeData(CachedScene);
            foreach (var slice in CachedScene.Slices)
            {
                SaveBitmap(slice.Value.Bitmap, $"slices/slice{slice.Key}.png");
                FrontPictureBox.Image = CachedScene.ToFrontBitmap();
                DrawBitmap(slice.Value.Bitmap, 30, (CachedScene.Slices.Count - slice.Key - 1.0) / CachedScene.Slices.Count);
            }

            //SaveData(Cache, "output.txt");
        }


        private void DrawBitmap(Bitmap bitmap, int sleep = 30, double h = 0.0)
        {
            PointF[] destinationPoints = {
                new PointF(0.0F, 0.0F),
                new PointF(bitmap.Width, 0.0F),
                new PointF(0.5F * bitmap.Height, 0.5F * bitmap.Height)
            };

            var perspectiveBitmap = PerspectivePictureBox.Image ?? new Bitmap(800 + bitmap.Height/2, 400 + (int)(h * 300));
            var newBitmap = new Bitmap(bitmap.Width + bitmap.Height / 2, bitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.DrawImage(bitmap, destinationPoints);
            graphics = Graphics.FromImage(perspectiveBitmap);
            graphics.DrawImage(newBitmap.CreateBorder(Color.Black), new Point((int)(0), (int)(50 + h * 300)));
            PerspectivePictureBox.Image = perspectiveBitmap;

            TopPictureBox.Image = TopPictureBox.Image ?? bitmap;
            graphics = Graphics.FromImage(TopPictureBox.Image);
            graphics.DrawImage(bitmap.CreateBorder(Color.Black), new Point(0, 0));
            //TopPictureBox.Image = bitmap;

            Refresh();
            Thread.Sleep(sleep);
        }


        private void SaveBitmap(Bitmap bitmap, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            bitmap.Save(path);
        }

        private void SaveData(Scene scene, string path)
        {
            File.WriteAllLines(path, scene.Slices.Select(i=>i.Value.Text));
        }
    }
}
