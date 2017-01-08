using System.Collections.Generic;

namespace MakeRuler
{
    public class Object
    {
        public Point2D Min { get; set; }
        public Point2D Max { get; set; }
        public int Material { get; set; }

        public Object(int material)
        {
            Material = material;
        }

        public virtual SortedDictionary<int, Line> ToLines()
        {
            return new SortedDictionary<int, Line>();
        }
    }
}