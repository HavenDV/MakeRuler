using System.Collections.Generic;
using System.Data;
using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Rect : Object
    {
        public Point2D Center
        {
            get { return new Point2D((Min.X + Max.X)/2, (Min.Y + Max.Y)/2); }
        }

        public Rect(Point2D min, Point2D max, int medium) :
            base(medium)
        {
            Min = min;
            Max = max;
            Lines = ComputeLines();
        }

        public Rect(double x1, double y1, double x2, double y2, int medium) :
            this(new Point2D(x1, y1), new Point2D(x2, y2), medium)
        { }

        public SortedDictionary<int, Line> ComputeLines()
        {
            var lines = new SortedDictionary<int, Line>();

            for (int y = Min.minY + 1; y <= Max.maxY; ++y)
            {
                lines.Add(y, new Line(Min.minX, Max.maxX, Medium));
            }

            return lines;
        }
    }
}