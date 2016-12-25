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
    }
}