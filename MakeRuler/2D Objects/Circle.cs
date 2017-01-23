using System;
using System.Collections.Generic;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Circle : Object
    {
        public Point2D Center { get; set; }
        public double Radius { get; set; }

        public Circle(Point2D center, double radius, Point2D min, Point2D max, int material) :
            base(material)
        {
            Center = center;
            Radius = radius;
            Min = min;
            Max = max;
        }

        public Circle(Point2D center, double radius, int material) :
            base(material)
        {
            Center = center;
            Radius = radius;
            Min = new Point2D(Center.X - Radius, Center.Y - Radius);
            Max = new Point2D(Center.X + Radius, Center.Y + Radius);
        }

        public Circle(double x, double y, double radius, int material) :
            this(new Point2D(x, y), radius, material)
        { }
        public Circle(double x, double y, double radius, double minX, double minY, double maxX, double maxY, int material) :
            this(new Point2D(x, y), radius, new Point2D(minX, minY), new Point2D(maxX, maxY),  material)
        { }

        public override SortedDictionary<int, Line> ToLines()
        {
            var lines = new SortedDictionary<int, Line>();
            
            for (int y = Min.minY + 1; y <= Max.maxY; ++y)
            {
                var dy = Center.Y + 0.5 - y;
                if (dy <= Radius)
                {
                    var dx = Math.Sqrt(Radius*Radius - dy*dy);
                    var x1 = Math.Max(Center.X - dx, Min.minX);
                    var x2 = Math.Min(Center.X + dx, Max.maxX);
                    var roundedX1 = x1.RoundMin();
                    var roundedX2 = x2.RoundMax();
                    if (roundedX2 > roundedX1)
                    {
                        lines.Add(y, new Line(roundedX1, roundedX2, Material));
                    }
                }
            }

            return lines;
        }
    }
}