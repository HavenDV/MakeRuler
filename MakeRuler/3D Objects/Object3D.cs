using MakeRuler.Extensions;

namespace MakeRuler
{
    abstract public class Object3D
    {
        public double Z1 { get; set; }
        public double Z2 { get; set; }

        public int minZ1 {
            get { return Z1.RoundMin(); }
        }

        public int maxZ1 {
            get { return Z1.RoundMax(); }
        }
        public int minZ2 {
            get { return Z2.RoundMin(); }
        }

        public int maxZ2 {
            get { return Z2.RoundMax(); }
        }

        abstract public Object GetObject(double h, double xyScale = 1.0);
    }
}