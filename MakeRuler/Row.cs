using System.Collections.Generic;

namespace Example1
{
    public class Row
    {
        public int Id { get; set; }
        public int FirstPixel { get; set; }
        public List<Area> Areas { get; set; }

        public Row()
        {}

        public Row(int id, int firstPixel)
        {
            Id = id;
            FirstPixel = firstPixel;
            Areas = new List<Area>();
        }

        public Row(string id, string firstPixel) :
            this(
                int.Parse(id),
                int.Parse(firstPixel)
            )
        {}
    }
}