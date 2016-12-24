namespace Example1
{
    public class Line
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Medium { get; set; }

        public Line(int start, int end, int medium)
        {
            Start = start;
            End = end;
            Medium = medium;
        }
    }
}