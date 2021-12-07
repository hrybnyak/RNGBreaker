using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGBreaker
{

    public static class LCGGenerator
    {
        private static int a = 1234561;
        private static int c = 654321;
        public static long m = (long) Math.Pow(2, 32);
        private static long seed = 13579;

        public static long Next()
        {
            seed = (a * seed + c) % m;
            return seed;
        }
    }
}
