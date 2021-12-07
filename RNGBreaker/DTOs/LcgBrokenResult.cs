namespace RNGBreaker.DTOs
{
    public class LcgBrokenResult
    {
        public long A { get; set; }
        public long C { get; set; }
        public long M { get; set; }
        public int AccountBalance { get; set; }
        public int LastValue { get; set; }

        public int PredictNext()
        {
            return (int)((A * LastValue + C) % M);
        }
    }
}
