using System.Collections.Generic;

namespace Example1
{
    public class Row
    {
        public int Id { get; set; }
        public int FirstPixel { get; set; }
        public List<Area> Areas { get; set; }

        public Row()
        {
            Areas = new List<Area>();
        }

        public Row(int id, int firstPixel) :
            this()
        {
            Id = id;
            FirstPixel = firstPixel;
        }

        public Row(string id, string firstPixel) :
            this(
                int.Parse(id),
                int.Parse(firstPixel)
            )
        {}
    }
}