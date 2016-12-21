﻿using System;
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
        private List<GeometryData> datalist;
        private List<GeometryData> datalist1;
        GeometryObject[] geoObj = new GeometryObject[41];

        static public string AppPath {
            get
            {
                var path = Assembly.GetExecutingAssembly().CodeBase;
                return Path.GetDirectoryName(path).Substring(6);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }


        private void readData(string fn)
        {
            datalist = new List<GeometryData>();
            string[] lines = File.ReadAllLines(fn);
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
        }


        private Color getColor1(int mediumID)
        {
            if (mediumID == 1)
                return Color.DarkGray;
            else
                return Color.Blue;
        }

        private Color getColor(int mediumID)
        {            
            switch (mediumID)
            {
                case 1:
                    return Color.DarkGray;
                    break;
                case 2:
                    return Color.Blue;
                    break;
                case 3:
                    return Color.Red;
                    break;
                case 4:
                    return Color.Green;
                    break;
                case 5:
                    return Color.Yellow;
                    break;
                case 6:
                    return Color.Pink;
                    break;
                case 7:
                    return Color.MediumSeaGreen;
                    break;
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

            geoObj[1] = new GeometryObject();
            geoObj[1].m_centerX = diameter0 / 2;
            geoObj[1].m_centerY = 30;
            int n_pY0 = (int)(diameter0 / yL);
            int n_pY1 = (int)(height / xL);
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
                geoObj[1].p_seList.Add(new StartEndPixel(px1, px2));
            }
            for (int i_obj = 2; i_obj <= holds + 1; i_obj++)
            {
                geoObj[i_obj] = new GeometryObject();
                geoObj[i_obj].m_centerX = margin + (i_obj - 2) * grid;
                geoObj[i_obj].m_centerY = 30;
                int n_pY = (int)(diameter / yL);
                int n_pY11 = geoObj[1].p_maxY - geoObj[1].p_minY + 1;

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
                    geoObj[i_obj].p_seList.Add(new StartEndPixel(px1, px2));
                }
            }

            datalist1 = new List<GeometryData>();
            for (int r = geoObj[1].p_minY; r <= geoObj[1].p_maxY; r++)
            {
                int firstPixel = geoObj[1].p_seList[r - geoObj[1].p_minY].start;
                int endPixel = geoObj[1].p_seList[r - geoObj[1].p_minY].end;
                int numberArea = 1;
                if (r >= geoObj[2].p_minY && r <= geoObj[2].p_maxY) numberArea += 2 * 39;
                GeometryData tmp = new GeometryData(r, firstPixel, numberArea);
                if (numberArea > 1)
                {
                    for (int i = 2; i <= holds + 1; i++)
                    {
                        tmp.datas.Add(new Datas(geoObj[i].p_seList[r - geoObj[i].p_minY].start, 1));
                        tmp.datas.Add(new Datas(geoObj[i].p_seList[r - geoObj[i].p_minY].end, i));
                    }
                }
                tmp.datas.Add(new Datas(endPixel, 1));
                datalist1.Add(tmp);
            }
            draw1();
            makeDatafile(20);
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
            
            geoObj[1] = new GeometryObject();
            geoObj[1].m_centerX = diameter0/2;
            geoObj[1].m_centerY = 30;
            int n_pY0 = (int)(diameter0 / yL);
            int n_pY1 = (int)(height / xL);
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
                geoObj[1].p_seList.Add(new StartEndPixel(px1,px2));
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
                    geoObj[i_obj].p_seList.Add(new StartEndPixel(px1, px2));
                }
            }

            datalist1 = new List<GeometryData>();
            for (int r = geoObj[1].p_minY; r <= geoObj[1].p_maxY; r++)
            {
                int firstPixel = geoObj[1].p_seList[r - geoObj[1].p_minY].start;
                int endPixel = geoObj[1].p_seList[r - geoObj[1].p_minY].end;
                int numberArea=1;
                if (r >= geoObj[2].p_minY && r <= geoObj[2].p_maxY) numberArea += 2 * 39;
                GeometryData tmp = new GeometryData(r, firstPixel, numberArea);
                if (numberArea>1){
                    for (int i = 2; i <= holds+1; i++)
                    {
                        tmp.datas.Add(new Datas(geoObj[i].p_seList[r - geoObj[i].p_minY].start, 1));
                        tmp.datas.Add(new Datas(geoObj[i].p_seList[r - geoObj[i].p_minY].end, i));
                    }
                }
                tmp.datas.Add(new Datas(endPixel, 1));
                datalist1.Add(tmp);
            }
            draw1();
            makeDatafile(40);
        }

        public static string toString(int value, int lenght)
        {
            return value.ToString().PadLeft(lenght);
        }

        private void makeDatafile(int holds)
        {
            var lines = new List<string>();
            for (int i = 1; i <= 6; ++i)
            {
                //SLICE NUMBER:  1  FIRST ROW:  1  LAST ROW:320
                lines.Add($"SLICE NUMBER:{toString(i, 3)}  FIRST ROW:{toString(geoObj[1].p_minY, 4)}  LAST ROW:{toString(geoObj[1].p_maxY, 4)}");
                if (i == 1 || i == 6)
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  1
                    //  173   1
                    for (int j = 0; j < datalist1.Count; j++)
                    {
                        lines.Add("ROW NR." + toString(j + geoObj[1].p_minY, 4) + "  FIRST PIXEL:" + toString(datalist1[j].FirstPixel, 4) + "  NUMBER OF AREAS:" + toString(1, 3));
                        lines.Add(toString(datalist1[j].datas.Last().EndPixel, 5) + toString(1, 4));                                    
                    }
                }
                else
                {
                    //ROW NR.  1  FIRST PIXEL:148  NUMBER OF AREAS:  3
                    //  152   1  168   6  247   1
                    for (int j = 0; j < datalist1.Count; j++)
                    {
                        lines.Add("ROW NR." + toString(j + geoObj[1].p_minY, 4) + "  FIRST PIXEL:" + toString(datalist1[j].FirstPixel, 4) + "  NUMBER OF AREAS:" + toString(datalist1[j].datas.Count, 3));                        
                        var tmp = "";
                        foreach (var data in datalist1[j].datas)
                            tmp += toString(data.EndPixel, 5) + toString(data.medium, 4);
                        lines.Add(tmp);
                    }
                }
            }
            var filename = $"out{holds}.txt";
            File.WriteAllLines(Path.Combine(AppPath, filename), lines);
        }

        private void draw1()
        {
            pictureBox2.Refresh();

            //create a new Bitmap object
            Bitmap map = (Bitmap)pictureBox2.Image;
            //create a graphics object
            Graphics g = pictureBox2.CreateGraphics();

            for (int i = 0; i < datalist1.Count; i++)
            {
                Point st = new Point(datalist1[i].FirstPixel, i + 1);
                //create a pen object and setting the color and width for the pen

                //draw line between  point p1 and p2
                for (int j = 0; j < datalist1[i].datas.Count; j++)
                {
                    Point en = new Point(datalist1[i].datas[j].EndPixel, i + 1);

                    Pen p = new Pen(getColor1(datalist1[i].datas[j].medium), 1);
                    g.DrawLine(p, st, en);
                    st = en;
                }
                //pictureBox2.Image = map;
                //dispose pen and graphics object

            }


        }

    }
}
