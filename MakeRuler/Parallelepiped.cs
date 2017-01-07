namespace MakeRuler
{
    public class Parallelepiped : IObject3D
    {
        public Rect Top { get; set; }
        public Rect Bottom { get; set; }

        public Parallelepiped(Rect bottom, Rect top)
        {
            Bottom = bottom;
            Top = top;
        }

        public Parallelepiped(Rect rect)
        {
            Bottom = rect;
            Top = rect;
        }

        public Object GetObject(double h, double xyScale = 1.0)
        {
            return new Rect(
                xyScale * (Bottom.Min + h * (Top.Min - Bottom.Min)),
                xyScale * (Bottom.Max + h * (Top.Max - Bottom.Max)),
                Bottom.Material
            );
        }
    }
}