namespace MakeRuler
{
    public class Cylinder : IObject3D
    {
        public Circle Top { get; set; }
        public Circle Bottom { get; set; }

        public Cylinder(Circle bottom, Circle top)
        {
            Bottom = bottom;
            Top = top;
        }

        public Object GetObject(double h, double xyScale = 1.0)
        {
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