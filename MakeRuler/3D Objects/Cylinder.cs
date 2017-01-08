namespace MakeRuler
{
    public class Cylinder : Object3D
    {
        public Circle Top { get; set; }
        public Circle Bottom { get; set; }

        public Cylinder(Circle bottom, Circle top, double z1, double z2)
        {
            Bottom = bottom;
            Top = top;
            Z1 = z1;
            Z2 = z2;
        }

        public Cylinder(Circle circle, double z1, double z2) :
            this(circle, circle, z1, z2)
        { }

        public override Object GetObject(double height, double xyScale = 1.0)
        {
            if (height < Z1 || height > Z2)
            {
                return new Object(Constants.AirMaterial);
            }

            var h = (height - Z1) * (Z2 - Z1);
            return new Circle(
                xyScale * (Bottom.Center + h * (Top.Center - Bottom.Center)),
                xyScale * (Bottom.Radius + h * (Top.Radius - Bottom.Radius)),
                xyScale * (Bottom.Min + h * (Top.Min - Bottom.Min)),
                xyScale * (Bottom.Max + h * (Top.Max - Bottom.Max)),
                Bottom.Material
            );
        }
    }
}