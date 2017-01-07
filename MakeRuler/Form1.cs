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

            //CORG(2) = 'table';
            var radius = 10;
            /*
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800, 0, 130 + radius, 360, 160 + radius, 2),
                new Circle(180, 120 + radius - 760, 760, 0, 90 + radius, 360, 120 + radius, 2)
            ));
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800 - radius, 0, 130 + radius, 360, 160 + radius, 8),
                new Circle(180, 120 + radius - 760, 760 - radius, 0, 90 + radius, 360, 120 + radius, 8)
            ));
            var inclinionFix = 0;
            //*/
            ///*
            scene.AddObject(new Parallelepiped(
                new Rect(0, 0, 360, 160 + radius, 8),
                new Rect(0, 0, 360, 160 + radius, 8)
            ));
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800, 0, 130 + radius, 360, 160 + radius, 2),
                new Circle(180, 160 + radius - 800, 800, 0, 130 + radius, 360, 160 + radius, 2)
            ));
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800 - radius, 0, 130 + radius, 360, 160 + radius, 8),
                new Circle(180, 160 + radius - 800, 800 - radius, 0, 130 + radius, 360, 160 + radius, 8)
            ));
            
            var inclinionFix = 40;
            //*/

            //CORG(1) = 'phantom';
            scene.AddObject(new Cylinder(
                new Circle(80, 80, 80, 1),
                new Circle(160, inclinionFix + 80, 40, 1)
            ));
            scene.AddObject(new Cylinder(
                new Circle(280, 80, 80, 1),
                new Circle(200, inclinionFix + 80, 40, 1)
            ));
            scene.AddObject(new Parallelepiped(
                new Rect(80, 0, 280, 160, 1),
                new Rect(160, inclinionFix + 40, 200, inclinionFix + 120, 1)
            ));

            //CORG(3) = 'center';
            scene.AddObject(new Cylinder(
                new Circle(180, 80, 5.5, 3),
                new Circle(180, inclinionFix + 80, 5.5, 3)
            ));
            
            //CORG(4) = 'left';
            scene.AddObject(new Cylinder(
                new Circle(30, 80, 5.5, 4),
                new Circle(150, inclinionFix + 80, 5.5, 4)
            ));
            
            //CORG(5) = 'bottom';
            scene.AddObject(new Cylinder(
                new Circle(180, 140, 5.5, 5),
                new Circle(180, inclinionFix + 100, 5.5, 5)
            ));

            //CORG(6) = 'right';
            scene.AddObject(new Cylinder(
                new Circle(330, 80, 5.5, 6),
                new Circle(210, inclinionFix + 80, 5.5, 6)
            ));

            //CORG(7) = 'top';
            scene.AddObject(new Cylinder(
                new Circle(180, 20, 5.5, 7),
                new Circle(180, inclinionFix + 60, 5.5, 7)
            ));
            

            return await scene.WithComputedSlices();
        }


        private async Task<Scene> CreateScene2(double step, double height)
        {
            var scene = new Scene();
            scene.Depth = (int)height;
            scene.Step = step;
            scene.XYScale = 1 / 1.0;

            //CORG(1) = 'phantom';
            scene.AddObject(new Cylinder(
                new Circle(160, 160, 160, 1),
                new Circle(160, 160, 160, 1)
            ));

            //CORG(2) = 'center';
            scene.AddObject(new Cylinder(
                new Circle(160, 160, 5, 2),
                new Circle(160, 160, 5, 2)
            ));

            //CORG(3) = 'left';
            scene.AddObject(new Cylinder(
                new Circle(10, 160, 5, 3),
                new Circle(10, 160, 5, 3)
            ));

            //CORG(4) = 'bottom';
            scene.AddObject(new Cylinder(
                new Circle(160, 310, 5, 4),
                new Circle(160, 310, 5, 4)
            ));

            //CORG(5) = 'right';
            scene.AddObject(new Cylinder(
                new Circle(310, 160, 5, 5),
                new Circle(310, 160, 5, 5)
            ));

            //CORG(6) = 'top';
            scene.AddObject(new Cylinder(
                new Circle(160, 10, 5, 6),
                new Circle(160, 10, 5, 6)
            ));


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
            var step = 5.0;
            var height = 300.0;

            //Fix step for reduce slices as -1
            //step = height / (-1 + height / step);

            //Scene.FromFile("CTDIcone1.data");//
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
            var center = new Point(0, 50  + (int)(h * 300));

            var perspectiveBitmap = PerspectivePictureBox.Image ?? new Bitmap(800 + bitmap.Height, 400 + (int)(h * 300));
            var newBitmap = new Bitmap(bitmap.Width + bitmap.Height / 2, bitmap.Height);
            var graphics = Graphics.FromImage(newBitmap);
            graphics.DrawImage(bitmap, destinationPoints);

            graphics = Graphics.FromImage(perspectiveBitmap);
            graphics.DrawImage(newBitmap.WithBorder(Color.Black), new Point(center.X, center.Y));
            PerspectivePictureBox.Image = perspectiveBitmap;

            TopPictureBox.Image = TopPictureBox.Image ?? bitmap;
            graphics = Graphics.FromImage(TopPictureBox.Image);
            graphics.DrawImage(bitmap.WithBorder(Color.Black), new Point(0, 0));
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
