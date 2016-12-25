namespace MakeRuler
{
    public class Area
    {
        public int EndPixel { get; set; }
        public int Material { get; set; }

        public Area()
        {}

        public Area(int endPixel, int material)
        {
            EndPixel = endPixel;
            Material = material;
        }

        public Area(string endPixel, string material) :
            this(
                int.Parse(endPixel),
                int.Parse(material)
            )
        {}

    }
}