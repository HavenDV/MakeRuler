using System;
using System.Collections.Generic;

namespace Example1
{
    public class Circle : Object
    {
        public Point2D Center { get; set; }
        public int Radius { get; set; }
        
        public Circle(Point2D center, int radius)
        {
            Center = center;
            Radius = radius;
            Min = new Point2D(Center.X - Radius + 1, Center.Y - Radius + 1);
            Max = new Point2D(Center.X + Radius - 1, Center.Y + Radius - 1);
            Lines = ComputeLines();
        }

        public Circle(int x, int y, int radius) :
            this(new Point2D(x, y), radius)
        {}

        public Dictionary<int, Line> ComputeLines()
        {
            var lines = new Dictionary<int, Line>();
            
            for (int y = Min.Y; y <= Max.Y; y++)
            {
                double dy = Math.Abs(Center.Y - y);
                double dx = Math.Sqrt(Radius * Radius - dy * dy);
                int x1 = (int)Math.Abs(Center.X - dx);
                int x2 = (int)Math.Abs(Center.X + dx);
                lines.Add(y,new Line(x1, x2));
            }

            return lines;
        }
    }
}