namespace MakeRuler
{
    public class Line
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Material { get; set; }

        public Line(int start, int end, int material)
        {
            Start = start;
            End = end;
            Material = material;
        }
    }
}