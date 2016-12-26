using System.Collections.Generic;

namespace MakeRuler
{
    public class Object
    {
        public int SliceId { get; set; }
        public SortedDictionary<int, Line> Lines { get; set; }
        public Point2D Min { get; set; }
        public Point2D Max { get; set; }
        public int Material { get; set; }

        public Object(int material)
        {
            Material = material;
        }
    }
}