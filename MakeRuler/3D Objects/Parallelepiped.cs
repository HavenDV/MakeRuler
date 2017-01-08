using System.Runtime.Remoting;

namespace MakeRuler
{
    public class Parallelepiped : Object3D
    {
        public Rect Top { get; set; }
        public Rect Bottom { get; set; }

        public Parallelepiped(Rect bottom, Rect top, double z1, double z2)
        {
            Bottom = bottom;
            Top = top;
            Z1 = z1;
            Z2 = z2;
        }

        public Parallelepiped(Rect rect, double z1, double z2) :
            this( rect, rect, z1, z2 )
        {}

        public override Object GetObject(double height, double xyScale = 1.0)
        {
            if (height < Z1 || height > Z2)
            {
                return new Object(Constants.AirMaterial);
            }

            var h = (height - Z1) * (Z2 - Z1);
            return new Rect(
                xyScale * (Bottom.Min + h * (Top.Min - Bottom.Min)),
                xyScale * (Bottom.Max + h * (Top.Max - Bottom.Max)),
                Bottom.Material
            );
        }
    }
}