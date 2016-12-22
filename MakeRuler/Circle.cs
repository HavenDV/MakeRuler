using System;
using System.Collections.Generic;

namespace Example1
{
    public class Circle
    {
        public Point2D Center { get; set; }
        public int Radius { get; set; }

        public double m_minY { get; set; }
        public double m_maxY { get; set; }
        public int p_minY { get; set; }
        public int p_maxY { get; set; }
        public Dictionary<int, Line> Lines { get; set; }
        
        public Circle(Point2D center, int radius)
        {
            Center = center;
            Radius = radius;
            p_minY = Center.Y - Radius + 1;
            p_maxY = Center.Y + Radius - 1;
            Lines = ComputeLines();
        }

        public Circle(int x, int y, int radius) :
            this(new Point2D(x, y), radius)
        {}

        public static int ConvertToPixelX(double mx){
            double xL = 0.5;
            return (int)(mx/xL)+1;
        }

        public Dictionary<int, Line> ComputeLines()
        {
            var lines = new Dictionary<int, Line>();
            
            for (int i = p_minY; i <= p_maxY; i++)
            {
                double y = Math.Abs(Center.Y - i);
                double dy = y;//geoObj[1].m_centerY - y;
                double x = Math.Sqrt(Radius * Radius - dy * dy);
                double x1 = Center.X - Math.Abs(x / 2.0);
                double x2 = Center.X + Math.Abs(x / 2.0);
                int px1 = ConvertToPixelX(x1);
                int px2 = ConvertToPixelX(x2);
                lines.Add(i,new Line(px1, px2));
            }

            return lines;
        }
    }
}