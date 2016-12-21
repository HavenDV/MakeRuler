namespace Example1
{
    public class Datas
    {
        public int EndPixel { get; set; }
        public int medium { get; set; }
        public Datas()
        {

        }
        public Datas(int EndPixel, int medium)
        {
            this.EndPixel = EndPixel;
            this.medium = medium;
        }
        public Datas(string EndPixel, string medium)
        {
            this.EndPixel = int.Parse(EndPixel);
            this.medium = int.Parse(medium);
        }

    }
}