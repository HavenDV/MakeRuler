using System.Collections.Generic;

namespace Example1
{
    public class GeometryObject
    {
        public int m_centerX { get; set; }
        public int m_centerY { get; set; }

        public double m_minY { get; set; }
        public double m_maxY { get; set; }
        public int p_minY { get; set; }
        public int p_maxY { get; set; }
        public List<Line> Lines { get; set; }

        public GeometryObject()
        {
            Lines = new List<Line>();
        }
        public static int ConvertToPixelX(double mx){
            double xL = 0.5;
            return (int)(mx/xL)+1;
        }
    }
}