using System;

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

            var h = GetRelativeHeight(height, Z1, Z2);
            var center = Bottom.Center + h * (Top.Center - Bottom.Center);
            var radius = Bottom.Radius + h * (Top.Radius - Bottom.Radius);
            var min = Bottom.Min + h * (Top.Min - Bottom.Min);
            var max = Bottom.Max + h * (Top.Max - Bottom.Max);

            return new Circle(
                xyScale * center,
                xyScale * radius,
                xyScale * min,
                xyScale * max,
                Bottom.Material
            );
        }
    }
}