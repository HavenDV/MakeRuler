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

        private async Task<Scene> CreateScene(double step, double height)
        {
            var scene = new Scene();
            scene.Depth = (int)height;
            scene.Step = step;
            scene.XYScale = 1 / 0.5;
            
            //CORG(1) = 'phantom';
            scene.AddObject(new Cylinder(
                new Circle(80, 80, 80, 1),
                new Circle(160, 80, 40, 1)
            ));
            scene.AddObject(new Cylinder(
                new Circle(280, 80, 80, 1),
                new Circle(200, 80, 40, 1)
            ));
            scene.AddObject(new Parallelepiped(
                new Rect(80, 0, 280, 160, 1),
                new Rect(160, 40, 200, 120, 1)
            ));

            //CORG(2) = 'center';
            scene.AddObject(new Cylinder(
                new Circle(180, 80, 5.5, 2),
                new Circle(180, 80, 5.5, 2)
            ));
            
            //CORG(3) = 'left';
            scene.AddObject(new Cylinder(
                new Circle(30, 80, 5.5, 3),
                new Circle(150, 80, 5.5, 3)
            ));
            
            //CORG(4) = 'bottom';
            scene.AddObject(new Cylinder(
                new Circle(180, 140, 5.5, 4),
                new Circle(180, 100, 5.5, 4)
            ));

            //CORG(5) = 'right';
            scene.AddObject(new Cylinder(
                new Circle(330, 80, 5.5, 5),
                new Circle(210, 80, 5.5, 5)
            ));

            //CORG(6) = 'top';
            scene.AddObject(new Cylinder(
                new Circle(180, 20, 5.5, 6),
                new Circle(180, 60, 5.5, 6)
            ));

            //center = 180 х 80
            //180 х 160
            //CORG(7) = 'table'; ??
            var table = new Circle(180, 160 - 800, 800, 7);
            table.Min = new Point2D(0, 140);
            table.Max = new Point2D(360, 160);
            table.Lines = table.ComputeLines();
            //slice.Add(table);

            var table2 = new Circle(180, 160 - 800, 790, 1);
            table2.Min = new Point2D(0, 140);
            table2.Max = new Point2D(360, 160);
            //table2.Lines = table2.ComputeLines();
            //slice.Add(table2);

            return await scene.WithComputedSlices();
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
            var step = 150.0;
            var height = 300.0;

            //Fix step for reduce slices as -1
            step = height / (-1 + height / step);

            //Scene.FromFile("CTDIcone(1).data");//
            CachedScene = CachedScene ?? await CreateScene(step, height);
            await ComputeData(CachedScene);
            foreach (var slice in CachedScene.Slices)
            {
                SaveBitmap(slice.Value.Bitmap, $"slices/slice{slice.Key}.png");
                DrawBitmap(slice.Value.Bitmap, 30, (CachedScene.Slices.Count - slice.Key - 1.0) / CachedScene.Slices.Count);
            }

            FrontPictureBox.Image = CachedScene.ToFrontBitmap();
            SidePictureBox.Image = CachedScene.ToSideBitmap();
            Refresh();

            CachedScene.ToFile("output.txt");
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
    }
}
