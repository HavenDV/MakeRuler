using System.Collections.Generic;

namespace Example1
{
    public class Object
    {
        public int Layer { get; set; }
        public Dictionary<int, Line> Lines { get; set; }
        public Point2D Min { get; set; }
        public Point2D Max { get; set; }
    }
}