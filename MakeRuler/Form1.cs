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
            //CORG(2) = 'center';
            //CORG(3) = 'left';
            //CORG(4) = 'bottom';
            //CORG(5) = 'right';
            //CORG(6) = 'top';
            scene.Add(new Rect(scale * (80 + 80 * h), scale * 40 * h, scale * (280 - 80 * h), scale * (160 - 40 * h), 1));
            scene.Add(new Circle(scale * (80 + 80 * h), scale * 80, scale * (80 - 40 * h), 1));
            scene.Add(new Circle(scale * (280 - 80 * h), scale * 80, scale * (80 - 40 * h), 1));
            scene.Add(new Circle(scale * 180, scale * 80, scale * 5.5, 2));
            scene.Add(new Circle(scale * (30 + 120 * h), scale * 80, scale * 5.5, 3));
            scene.Add(new Circle(scale * 180, scale * (140 - 40 * h), scale * 5.5, 4));
            scene.Add(new Circle(scale * (330 - 120 * h), scale * 80, scale * 5.5, 5));
            scene.Add(new Circle(scale * 180, scale * (20 + 40 * h), scale * 5.5, 6));
            scene.ToBitmap();
            scene.m_text = scene.ToText(slice, false);
            return scene;
        }

        private async Task<Scene[]> CreateLayers(int num, double step, double height)
        {
            return await Task.WhenAll(
                Enumerable.Range(1, num).Select(
                    i => Task.Run(
                            () => CreateLayer(i, step, height)
                        )
                    )
                );
        }

        private Scene[] layers = null;
        private async void CreateButton_Click(object sender, EventArgs e)
        {
            var step = 150.0;
            var height = 300.0;
            var num = height / step;

            layers = layers ?? await CreateLayers((int)num, step, height);
            var lines = new List<string>();
            for (var slice = 1; slice <= num; ++slice)
            {
                var layer = layers[slice-1];
                var bitmap = layer.ToBitmap();
                Directory.CreateDirectory("slices");
                bitmap.Save($"slices/slice{slice}.png");
                DrawBitmap(bitmap);
                lines.Add(layer.m_text);
                progressBar1.Value = (int)(100.0 * slice / num);
                Refresh();
            }

            File.WriteAllLines("output.txt", lines);
        }


        private void DrawBitmap(Bitmap bitmap)
        {
            pictureBox2.Image = bitmap;
            Thread.Sleep(30);
        }

    }
}
