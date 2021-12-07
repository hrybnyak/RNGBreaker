using System;
using System.Collections.Generic;

namespace RNGBreaker
{
    public static class LcgBreaker
    {
        public static readonly long M = (long)Math.Pow(2, 32);

        public static Tuple<long, long> Break (List<long> values)
        {
            var a = (values[2] - values[1]) * ((values[1] - values[0]).ModInversion(M)) % M;
            var b = values[1] - (values[0] * a % M);
            return new (a, b);
        }
    }
}
