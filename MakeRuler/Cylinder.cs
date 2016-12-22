namespace Example1
{
    public class Cylinder
    {
        public Circle Top { get; set; }
        public Circle Bottom { get; set; }

        public Cylinder(Circle top, Circle bottom)
        {
            Top = top;
            Bottom = bottom;
        }
    }
}