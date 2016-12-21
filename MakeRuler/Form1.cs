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
            get
            {
                var path = Assembly.GetExecutingAssembly().CodeBase;
                return Path.GetDirectoryName(path).Substring(6);
            }
        }

        private List<GeometryData> ReadData(string path)
        {
            var datalist = new List<GeometryData>();
            var lines = File.ReadAllLines(path);

            // Display the file contents by using a foreach loop.
            for (int i=0;i<lines.Length;i+=2)
            {
                // Use a tab to indent each line of the file.
                string[] args = lines[i].Split(new[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
                datalist.Add(new GeometryData(args[0], args[1], args[2]));
                string[] args1 = lines[i+1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j=0;j<int.Parse(args[2]);j++)
                {
                    datalist[datalist.Count - 1].datas.Add(new Datas(args1[2 * j], args1[2 * j + 1]));
                }
            }

            return datalist;
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

        private void button1_Click(object sender, EventArgs e)
        {
            //-------------

            int grid = 40;
            int margin = grid / 2;
            int diameter0 = 760;
            int holds = diameter0 / grid;
            int height = 60;
            int diameter = 9;
            double xL = 0.5;
            double yL = 0.5;
            double zL = 0.25;
            int n_pY0 = (int)(diameter0 / yL);
            int n_pY1 = (int)(height / xL);

            var geoObj = new GeometryObject[41];
            geoObj[1] = new GeometryObject();
            geoObj[1].m_centerX = diameter0 / 2;
            geoObj[1].m_centerY = 30;
            geoObj[1].p_minY = (n_pY0 - n_pY1) / 2 + 1;
            geoObj[1].p_maxY = (n_pY0 - n_pY1) / 2 + n_pY1;
            for (int i = geoObj[1].p_minY; i <= geoObj[1].p_maxY; i++)
            {
                double y = Math.Abs((geoObj[1].p_maxY + geoObj[1].p_minY) / 2.0 - i) * yL;
                double dy = y;//geoObj[1].m_centerY - y;
                double x = Math.Sqrt(diameter0 * diameter0 / 4 - dy * dy);
                double x1 = geoObj[1].m_centerX - x;
                double x2 = geoObj[1].m_centerX + x;
                int px1 = GeometryObject.ConvertToPixelX(x1);
                int px2 = GeometryObject.ConvertToPixelX(x2);
                geoObj[1].Lines.Add(new Line(px1, px2));
            }
            int n_pY = (int)(diameter / yL);
            int n_pY11 = geoObj[1].p_maxY - geoObj[1].p_minY + 1;
            for (int i_obj = 2; i_obj <= holds + 1; i_obj++)
            {
                geoObj[i_obj] = new GeometryObject();
                geoObj[i_obj].m_centerX = margin + (i_obj - 2) * grid;
                geoObj[i_obj].m_centerY = 30;
                geoObj[i_obj].p_minY = geoObj[1].p_minY + (n_pY11 - n_pY) / 2 + 1;
                geoObj[i_obj].p_maxY = geoObj[1].p_minY + (n_pY11 + n_pY) / 2;
                for (int i = geoObj[i_obj].p_minY; i <= geoObj[i_obj].p_maxY; i++)
                {
                    double y = Math.Abs((geoObj[i_obj].p_maxY + geoObj[i_obj].p_minY) / 2.0 - i) * yL;
                    double dy = y;// geoObj[i_obj].m_centerY - y;
                    double x = Math.Sqrt(diameter * diameter / 4 - dy * dy);
                    double x1 = geoObj[i_obj].m_centerX - x;
                    double x2 = geoObj[i_obj].m_centerX + x;
                    int px1 = GeometryObject.ConvertToPixelX(x1);
                    int px2 = GeometryObject.ConvertToPixelX(x2);
                    geoObj[i_obj].Lines.Add(new Line(px1, px2));
                }
            }

            var datalist1 = new List<GeometryData>();
            for (int r = geoObj[1].p_minY; r <= geoObj[1].p_maxY; r++)
            {
                var line = geoObj[1].Lines[r - geoObj[1].p_minY];
                int firstPixel = line.Start;
                int endPixel = line.End;
                int numberArea = 1;
                if (r >= geoObj[2].p_minY && r <= geoObj[2].p_maxY) numberArea += 2 * 39;
                var tmp = new GeometryData(r, firstPixel, numberArea);
                if (numberArea > 1)
                {
                    for (int i = 2; i <= holds + 1; i++)
                    {
                        tmp.datas.Add(new Datas(geoObj[i].Lines[r - geoObj[i].p_minY].Start, 1));
                        tmp.datas.Add(new Datas(geoObj[i].Lines[r - geoObj[i].p_minY].End, i));
                    }
                }
                tmp.datas.Add(new Datas(endPixel, 1));
                datalist1.Add(tmp);
            }
            DrawData(datalist1);
            MakeDatafile(datalist1, 20, geoObj[1].p_minY);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //-------------
            
            int grid = 20;
            int margin = grid/2;
            int diameter0 = 780;
            int holds = diameter0 / grid;
            int height = 60;
            int diameter = 9;
            double xL = 0.5;
            double yL = 0.5;
            double zL = 0.25;
            int n_pY0 = (int)(diameter0 / yL);
            int n_pY1 = (int)(height / xL);

            var geoObj = new GeometryObject[41];
            geoObj[1] = new GeometryObject();
            geoObj[1].m_centerX = diameter0/2;
            geoObj[1].m_centerY = 30;
            geoObj[1].p_minY = (n_pY0 - n_pY1) / 2 + 1;
            geoObj[1].p_maxY = (n_pY0 - n_pY1) / 2 + n_pY1;
            for (int i=geoObj[1].p_minY;i<=geoObj[1].p_maxY;i++)
            {
                double y = Math.Abs((geoObj[1].p_maxY + geoObj[1].p_minY ) / 2.0 - i)*yL;
                double dy = y;//geoObj[1].m_centerY - y;
                double x = Math.Sqrt(diameter0 * diameter0/4 - dy * dy);
                double x1 = geoObj[1].m_centerX - x;
                double x2 = geoObj[1].m_centerX + x;
                int px1 = GeometryObject.ConvertToPixelX(x1);
                int px2 = GeometryObject.ConvertToPixelX(x2);
                geoObj[1].Lines.Add(new Line(px1,px2));
            }
            for (int i_obj = 2; i_obj <= holds+1; i_obj++)
            {
                geoObj[i_obj] = new GeometryObject();
                geoObj[i_obj].m_centerX = margin+(i_obj-2)*grid;
                geoObj[i_obj].m_centerY = 30;
                int n_pY = (int)(diameter / yL);
                int n_pY11 = geoObj[1].p_maxY - geoObj[1].p_minY+1;

                geoObj[i_obj].p_minY = geoObj[1].p_minY + (n_pY11 - n_pY) / 2 + 1;
                geoObj[i_obj].p_maxY = geoObj[1].p_minY + (n_pY11 + n_pY) / 2;
                for (int i = geoObj[i_obj].p_minY; i <= geoObj[i_obj].p_maxY; i++)
                {
                    double y = Math.Abs((geoObj[i_obj].p_maxY + geoObj[i_obj].p_minY) / 2.0 - i) * yL;
                    double dy = y;// geoObj[i_obj].m_centerY - y;
                    double x = Math.Sqrt(diameter * diameter/4 - dy * dy);
                    double x1 = geoObj[i_obj].m_centerX - x;
                    double x2 = geoObj[i_obj].m_centerX + x;
                    int px1 = GeometryObject.ConvertToPixelX(x1);
                    int px2 = GeometryObject.ConvertToPixelX(x2);
                    geoObj[i_obj].Lines.Add(new Line(px1, px2));
                }
            }

            var datalist1 = new List<GeometryData>();
            for (int r = geoObj[1].p_minY; r <= geoObj[1].p_maxY; r++)
            {
                int firstPixel = geoObj[1].Lines[r - geoObj[1].p_minY].Start;
                int endPixel = geoObj[1].Lines[r - geoObj[1].p_minY].End;
                int numberArea=1;
                if (r >= geoObj[2].p_minY && r <= geoObj[2].p_maxY) numberArea += 2 * 39;
                var tmp = new GeometryData(r, firstPixel, numberArea);
                if (numberArea>1){
                    for (int i = 2; i <= holds+1; i++)
                    {
                        tmp.datas.Add(new Datas(geoObj[i].Lines[r - geoObj[i].p_minY].Start, 1));
                        tmp.datas.Add(new Datas(geoObj[i].Lines[r - geoObj[i].p_minY].End, i));
                    }
                }
                tmp.datas.Add(new Datas(endPixel, 1));
                datalist1.Add(tmp);
            }
            DrawData(datalist1);
            MakeDatafile(datalist1, 40, geoObj[1].p_minY);
        }

        public static string ToString(int value, int lenght)
        {
            return value.ToString().PadLeft(lenght);
        }

        private void MakeDatafile(List<GeometryData> datalist, int holds, int firstRow)
        {
            var lines = new List<string>();
            for (int i = 1; i <= 6; ++i)
            {
                //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
                lines.Add($"SLICE NUMBER:{ToString(i, 3)}  FIRST ROW:{ToString(firstRow, 4)}  LAST ROW:{ToString(firstRow + datalist.Count - 1, 4)}");
                if (i == 1 || i == 6)
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  1
                    //  173   1
                    for (int j = 0; j < datalist.Count; j++)
                    {
                        lines.Add("ROW NR." + ToString(j + firstRow, 4) + "  FIRST PIXEL:" + ToString(datalist[j].FirstPixel, 4) + "  NUMBER OF AREAS:" + ToString(1, 3));
                        lines.Add(ToString(datalist[j].datas.Last().EndPixel, 5) + ToString(1, 4));                                    
                    }
                }
                else
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  3
                    //  152   1  168   6  247   1
                    for (int j = 0; j < datalist.Count; j++)
                    {
                        lines.Add("ROW NR." + ToString(j + firstRow, 4) + "  FIRST PIXEL:" + ToString(datalist[j].FirstPixel, 4) + "  NUMBER OF AREAS:" + ToString(datalist[j].datas.Count, 3));                        
                        var tmp = "";
                        foreach (var data in datalist[j].datas)
                            tmp += ToString(data.EndPixel, 5) + ToString(data.medium, 4);
                        lines.Add(tmp);
                    }
                }
            }
            var filename = $"out{holds}.txt";
            File.WriteAllLines(Path.Combine(AppPath, filename), lines);
        }

        private void DrawData(List<GeometryData> datalist)
        {
            pictureBox2.Refresh();

            var map = (Bitmap)pictureBox2.Image;
            var g = pictureBox2.CreateGraphics();

            for (int i = 0; i < datalist.Count; i++)
            {
                var start = new Point(datalist[i].FirstPixel, i + 1);
                //create a pen object and setting the color and width for the pen

                //draw line between  point p1 and p2
                foreach (var data in datalist[i].datas)
                {
                    var end = new Point(data.EndPixel, i + 1);
                    Pen p = new Pen(GetColor1(data.medium), 1);
                    g.DrawLine(p, start, end);
                    start = end;
                }
                //pictureBox2.Image = map;
                //dispose pen and graphics object

            }


        }

    }
}
