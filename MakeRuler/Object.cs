using System.Collections.Generic;

namespace Example1
{
    public class Object
    {
        public int Layer { get; set; }
        public SortedDictionary<int, Line> Lines { get; set; }
        public Point2D Min { get; set; }
        public Point2D Max { get; set; }
        public int Medium { get; set; }

        public Object(int medium)
        {
            Medium = medium;
        }
    }
}