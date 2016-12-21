using System.Collections.Generic;

namespace Example1
{
    public class GeometryData
    {
        public int RowID { get; set; }
        public int FirstPixel { get; set; }
        public int NumberOfAreas { get; set; }
        public GeometryData() { }
        public List<Datas> datas;
        public GeometryData(int RowID, int FirstPixel, int NumberOfAreas)
        {
            this.RowID = RowID;
            this.FirstPixel = FirstPixel;
            this.NumberOfAreas = NumberOfAreas;
            this.datas = new List<Datas>();
        }
        public GeometryData(string RowID, string FirstPixel, string NumberOfAreas)
        {
            this.RowID = int.Parse(RowID);
            this.FirstPixel = int.Parse(FirstPixel);
            this.NumberOfAreas = int.Parse(NumberOfAreas);
            this.datas = new List<Datas>();
        }
    }
}