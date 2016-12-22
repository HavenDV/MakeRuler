using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Example1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static public string AppPath {
            get {
                var path = Assembly.GetExecutingAssembly().CodeBase;
                return Path.GetDirectoryName(path).Substring(6);
            }
        }

        private Scene CreateScene(int holds)
        {
            int diameter0 = 780;
            int grid = diameter0 / holds;
            int margin = grid / 2;
            int height = 60;
            int radius = 9;

            var bigCircle = new Circle(diameter0 / 2, diameter0 + 1, diameter0);
            bigCircle.p_minY = diameter0 - height;
            bigCircle.p_maxY = bigCircle.p_minY + height * 2;

            var scene = new Scene();
            scene.Add(bigCircle, 1);
            for (int i = 0; i < holds; i++)
            {
                scene.Add(new Circle(margin + i * grid, bigCircle.p_minY + height, radius), i + 2);
            }

            return scene;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var scene = CreateScene(20);
            DrawScene(scene);
            scene.ToFile(Path.Combine(AppPath, "output20.txt"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var scene = CreateScene(40);
            DrawScene(scene);
            scene.ToFile(Path.Combine(AppPath, "output40.txt"));
        }


        private Color GetColor1(int mediumID)
        {
            if (mediumID == 1)
                return Color.DarkGray;
            else
                return Color.Blue;
        }

        private Color GetColor(int mediumID)
        {
            switch (mediumID)
            {
                case 1:
                    return Color.DarkGray;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Green;
                case 5:
                    return Color.Yellow;
                case 6:
                    return Color.Pink;
                case 7:
                    return Color.MediumSeaGreen;
            }
            return Color.White; ;
        }

        private void DrawScene(Scene scene)
        {
            pictureBox2.Refresh();
            var rows = scene.Rows;
            var map = (Bitmap)pictureBox2.Image;
            var g = pictureBox2.CreateGraphics();

            var i = 0;
            foreach (var row in rows)
            {
                var start = new Point(row.Value.FirstPixel, i + 1);
                foreach (var data in row.Value.Areas)
                {
                    var end = new Point(data.EndPixel, i + 1);
                    Pen p = new Pen(GetColor1(data.Medium), 1);
                    g.DrawLine(p, start, end);
                    start = end;
                }
                ++i;
            }
        }

    }
}
