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
    public static class Constants
    {
        public static readonly int AirMaterial = 8;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task<Scene> CreateScene1()
        {
            //Create scene with dimensions 0.5 * 0.5 * 5.0
            var scene = new Scene(0.5, 0.5, 5.0);
            var height = 300.0;

            //CORG(2) = 'table';
            var radius = 10;
            scene.AddObject(new Parallelepiped(
                new Rect(0, 0, 360, 160 + radius, Constants.AirMaterial),
                new Rect(0, 0, 360, 120 + radius, Constants.AirMaterial),
                0, height
            ));
            /*
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800, 0, 130 + radius, 360, 160 + radius, 2),
                new Circle(180, 120 + radius - 760, 760, 0, 90 + radius, 360, 120 + radius, 2),
                0, height
            ));
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800 - radius, 0, 130 + radius, 360, 160 + radius, Constants.AirMaterial),
                new Circle(180, 120 + radius - 760, 760 - radius, 0, 90 + radius, 360, 120 + radius, Constants.AirMaterial),
                0, height
            ));
            var inclinionFix = 0;
            //*/
            /*
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800, 0, 130 + radius, 360, 160 + radius, 2),
                0, height
            ));
            scene.AddObject(new Cylinder(
                new Circle(180, 160 + radius - 800, 800 - radius, 0, 130 + radius, 360, 160 + radius, Constants.AirMaterial),
                0, height
            ));
            var inclinionFix = 40;
            //*/

            var inclinionFix = 0;
            scene.AddObject(new Parallelepiped(
                new Rect(0, 160, 360, 160 + radius, 2),
                0, height
            ));

            //CORG(1) = 'phantom';
            scene.AddObject(new Cylinder(
                new Circle(80, 80, 80, 1),
                new Circle(160, inclinionFix + 80, 40, 1),
                0, height
            ));
            scene.AddObject(new Cylinder(
                new Circle(280, 80, 80, 1),
                new Circle(200, inclinionFix + 80, 40, 1),
                0, height
            ));
            scene.AddObject(new Parallelepiped(
                new Rect(80, 0, 280, 160, 1),
                new Rect(160, inclinionFix + 40, 200, inclinionFix + 120, 1),
                0, 300
            ));

            //CORG(3) = 'center';
            scene.AddObject(new Cylinder(
                new Circle(180, 80, 5.5, 3),
                new Circle(180, inclinionFix + 80, 5.5, 3),
                0, height
            ));
            
            //CORG(4) = 'left';
            scene.AddObject(new Cylinder(
                new Circle(30, 80, 5.5, 4),
                new Circle(150, inclinionFix + 80, 5.5, 4),
                0, height
            ));
            
            //CORG(5) = 'bottom';
            scene.AddObject(new Cylinder(
                new Circle(180, 140, 5.5, 5),
                new Circle(180, inclinionFix + 100, 5.5, 5),
                0, height
            ));

            //CORG(6) = 'right';
            scene.AddObject(new Cylinder(
                new Circle(330, 80, 5.5, 6),
                new Circle(210, inclinionFix + 80, 5.5, 6),
                0, height
            ));

            //CORG(7) = 'top';
            scene.AddObject(new Cylinder(
                new Circle(180, 20, 5.5, 7),
                new Circle(180, inclinionFix + 60, 5.5, 7),
                0, height
            ));
            

            return await scene.WithComputedSlices();
        }

        private Scene AddObjects(Scene scene, double z1, double z2, double radius, double thickness = 10.0)
        {
            var holeRadius = 6.0;
            var dist = 10.0 + holeRadius;

            //CORG(8) = 'air';
            scene.AddObject(new Parallelepiped(
                new Rect(0, 0, radius * 2, radius * 2, Constants.AirMaterial),
                z1, z2
            ));

            //CORG(1) = 'phantom';
            scene.AddObject(new Cylinder(
                new Circle(radius, radius, radius, 1),
                z1, z2
            ));

            //CORG(2) = 'table';
            scene.AddObject(new Parallelepiped(
                new Rect(0, radius * 2, radius * 2, radius * 2 + thickness, 2),
                z1, z2
            ));

            //CORG(3) = 'center';
            scene.AddObject(new Cylinder(
                new Circle(radius, radius, holeRadius, 3),
                z1, z2
            ));

            //CORG(4) = 'left';
            scene.AddObject(new Cylinder(
                new Circle(dist, radius, holeRadius, 4),
                z1, z2
            ));

            //CORG(5) = 'bottom';
            scene.AddObject(new Cylinder(
                new Circle(radius, 2 * radius - dist, holeRadius, 5),
                z1, z2
            ));

            //CORG(6) = 'right';
            scene.AddObject(new Cylinder(
                new Circle(2 * radius - dist, radius, holeRadius, 6),
                z1, z2
            ));

            //CORG(7) = 'top';
            scene.AddObject(new Cylinder(
                new Circle(radius, dist, holeRadius, 7),
                z1, z2
            ));

            return scene;
        }

        private async Task<Scene> CreateScene2()
        {
            //Create scene with dimensions 0.5 * 0.5 * 5.0
            var scene = new Scene(0.5, 0.5, 5.0);

            scene = AddObjects(scene, 0, 150, 160, 10.0);

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
                            slice.Value.PerspectiveBitmap = slice.Value.Bitmap.ToPerspective().WithBorder(Color.Black);
                            slice.Value.Text = slice.Value.Text ?? slice.Value.ToText(slice.Key, false);
                            return slice.Key;
                        }
                    )
                )
            );
        }

        private async void CreateButton_Click(object sender, EventArgs e)
        {
            var scene = 
                //Scene.FromFile("CTDIcone(4).data");
                await CreateScene2();
            await ComputeData(scene);
            scene.ToFile("output.txt");

            foreach (var slice in scene.Slices)
            {
                SaveBitmap(slice.Value.Bitmap, $"slices/slice{slice.Key}.png");
                DrawBitmap(slice.Value.Bitmap, slice.Value.PerspectiveBitmap, 30, (scene.Slices.Count - slice.Key - 1.0) / scene.Slices.Count);
            }

            FrontPictureBox.Image = scene.ToFrontBitmap();
            SidePictureBox.Image = scene.ToSideBitmap();
            Refresh();
        }

        private void DrawBitmap(Bitmap bitmap, Bitmap perspective, int sleep = 30, double h = 0.0)
        {
            var center = new Point(0, 50  + (int)(h * 300));

            var perspectiveBitmap = PerspectivePictureBox.Image ?? new Bitmap(800 + bitmap.Height, 400 + (int)(h * 300));
            var graphics = Graphics.FromImage(perspectiveBitmap);
            graphics.DrawImage(perspective, new Point(center.X, center.Y));
            PerspectivePictureBox.Image = perspectiveBitmap;

            TopPictureBox.Image = TopPictureBox.Image ?? bitmap;
            graphics = Graphics.FromImage(TopPictureBox.Image);
            graphics.DrawImage(bitmap.WithBorder(Color.Black), new Point(0, 0));
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
