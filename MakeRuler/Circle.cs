using System;
using System.Collections.Generic;
using MakeRuler.Extensions;

namespace Example1
{
    public class Circle : Object
    {
        public Point2D Center { get; set; }
        public double Radius { get; set; }
        
        public Circle(Point2D center, double radius, int medium) :
            base(medium)
        {
            Center = center;
            Radius = radius;
            Min = new Point2D(Center.X - Radius, Center.Y - Radius);
            Max = new Point2D(Center.X + Radius, Center.Y + Radius);
            Lines = ComputeLines();
        }

        public Circle(double x, double y, double radius, int medium) :
            this(new Point2D(x, y), radius, medium)
        {}

        public SortedDictionary<int, Line> ComputeLines()
        {
            var lines = new SortedDictionary<int, Line>();
            
            for (int y = Min.iY; y <= Max.Y; y++)
            {
                var dy = Center.Y - y;
                if (dy <= Radius)
                {
                    var dx = Math.Sqrt(Radius*Radius - dy*dy);
                    var x1 = Center.X - dx;
                    var x2 = Center.X + dx;
                    lines.Add(y, new Line(x1.Round(), x2.Round(), Medium));
                }
            }

            return lines;
        }
    }
}