using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public int minX {
            get { return X.RoundMin(); }
        }

        public int minY {
            get { return Y.RoundMin(); }
        }

        public int maxX {
            get { return X.RoundMax(); }
        }

        public int maxY {
            get { return Y.RoundMax(); }
        }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator -(Point2D point)
        {
            return new Point2D(-point.X , -point.Y);
        }

        public static Point2D operator +(Point2D first, Point2D second)
        {
            return new Point2D(first.X + second.X, first.Y + second.Y);
        }

        public static Point2D operator -(Point2D first, Point2D second)
        {
            return first + (-second);
        }

        public static Point2D operator *(Point2D point, double scale)
        {
            return new Point2D(scale * point.X, scale * point.Y);
        }

        public static Point2D operator *(double scale, Point2D point)
        {
            return point * scale;
        }

        public static Point2D operator /(Point2D point, double reduce)
        {
            return (1.0 / reduce) * point;
        }
    }
}