using System.Collections.Generic;

namespace MakeRuler
{
    abstract public class Object
    {
        public int SliceId { get; set; }
        public Point2D Min { get; set; }
        public Point2D Max { get; set; }
        public int Material { get; set; }

        public Object(int material)
        {
            Material = material;
        }

        abstract public SortedDictionary<int, Line> ToLines();
    }
}