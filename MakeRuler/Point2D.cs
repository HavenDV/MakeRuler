using MakeRuler.Extensions;

namespace MakeRuler
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int iX {
            get { return X.Round(); }
        }
        public int iY {
            get { return Y.Round(); }
        }
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}