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
        private SortedDictionary<int, Scene> Cache { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private Scene CreateLayer(int slice, double step, double height)
        {
            var scene = new Scene();
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

        private async Task<SortedDictionary<int, Scene>> CreateLayers(double step, double height)
        {
            var layers = await Task.WhenAll(
                Enumerable.Range(1, (int)(height / step)).Select(
                    i => Task.Run(
                        () => new KeyValuePair<int, Scene>(i, CreateLayer(i, step, height))
                    )
                )
            );
            return new SortedDictionary<int, Scene>(layers.ToDictionary(i => i.Key, i => i.Value));
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            var step = 50.0;
            var height = 300.0;

            Cache = Cache ?? await CreateLayers(step, height);
            var lines = new List<string>();
            foreach (var layer in Cache)
            {
                var bitmap = layer.Value.Bitmap;
                SaveBitmap(bitmap, $"slices/slice{layer.Key}.png");
                DrawBitmap(bitmap);
                lines.Add(layer.Value.Text);
            }

            SaveData(Cache, "output.txt");
        }


        private void DrawBitmap(Bitmap bitmap, int sleep = 30)
        {
            pictureBox2.Image = bitmap;
            Refresh();
            Thread.Sleep(sleep);
        }


        private void SaveBitmap(Bitmap bitmap, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            bitmap.Save(path);
        }

        private void SaveData(SortedDictionary<int, Scene> layers, string path)
        {
            File.WriteAllLines(path, layers.Select(i=>i.Value.Text));
        }

    }
}
