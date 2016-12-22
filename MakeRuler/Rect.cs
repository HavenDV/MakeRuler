using System.Collections.Generic;

namespace Example1
{
    public class Rect : Object
    {
        public Point2D Center { get; set; }

        public Rect(Point2D min, Point2D max)
        {
            Min = min;
            Max = max;
            Center = new Point2D((Min.X + Max.X)/2, (Min.Y + Max.Y)/2);
            Lines = ComputeLines();
        }

        public Rect(int x1, int y1, int x2, int y2) :
            this(new Point2D(x1, y1), new Point2D(x2, y2))
        { }

        public Dictionary<int, Line> ComputeLines()
        {
            var lines = new Dictionary<int, Line>();

            for (int y = Min.Y; y <= Max.Y; y++)
            {
                lines.Add(y, new Line(Min.X, Max.X));
            }

            return lines;
        }
    }
}