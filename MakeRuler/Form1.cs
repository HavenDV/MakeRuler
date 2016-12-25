using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace MakeRuler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Scene CreateLayer(int height)
        {
            var scene = new Scene();
            var h = height / 300.0;
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
            return scene;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var step = 50;

            var lines = new List<string>();
            for (var i = 0; i < 300; i += step)
            {
                var layer = CreateLayer(i);
                var bitmap = layer.ToBitmap();
                var slice = 1 + i / step;
                Directory.CreateDirectory("slices");
                bitmap.Save($"slices/slice{slice}.png");
                DrawBitmap(bitmap);
                lines.Add(layer.ToText(slice, false));
                progressBar1.Value = (int)(100.0 * slice / (300.0 / step));
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
