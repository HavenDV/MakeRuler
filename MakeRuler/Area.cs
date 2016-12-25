namespace MakeRuler
{
    public class Area
    {
        public int EndPixel { get; set; }
        public int Medium { get; set; }

        public Area()
        {}

        public Area(int endPixel, int medium)
        {
            EndPixel = endPixel;
            Medium = medium;
        }

        public Area(string endPixel, string medium) :
            this(
                int.Parse(endPixel),
                int.Parse(medium)
            )
        {}

    }
}