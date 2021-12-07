using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGBreaker
{
    public static class LongExtensions
    {
        public static long ModInversion(this long a, long p)
        {
            var result = EGCD(a, p);
            if (result.Item1 == 1)
            {
                return result.Item2 % p;
            }
            else
            {
                throw new InvalidOperationException("Numbers should be coprime");
            }
        }

        private static Tuple<long, long, long> EGCD(long a, long p)
        {
            if (a == 0)
            {
                return new (p, 0, 1);
            }
            else
            {
                var result = EGCD(p % a, a);
                return new(
                    result.Item1,
                    result.Item3 - (p / a) * result.Item2,
                    result.Item2);
            }
        }
    }
}