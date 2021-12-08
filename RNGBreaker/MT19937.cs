using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGBreaker
{
    public class MT19937
    {
        public const int w = 32;
        public const int n = 624;
        public const int f = 1812433253;
        public const int m = 397;
        public const int r = 31;
        public const uint a = 0x9908B0DF;
        public const uint d = 0xFFFFFFFF;
        public const uint c = 0xEFC60000;
        public const uint b = 0x9D2C5680;
        public const int u = 11;
        public const int s = 7;
        public const int t = 15;
        public const int l = 18;
        private int[] X = new int[n];
        private int counter = 0;

        public MT19937(int seed)
        {
            X[0] = seed;
            for (int i = 1; i < n; i++)
            {
                X[i] = ((X[i - 1] ^ (X[i - 1] >> (w - 2))) + i);
            }
            Twist();
        }

        private void Twist()
        {
            for (int i = 0; i < n; i++)
            {
                unchecked
                {
                    var lowerMask = (1 << r) - 1;
                    var upperMask = (~lowerMask) & ((1 << w) - 1);
                    int temp = (X[i] & upperMask) + X[(i + 1) % n] & lowerMask;
                    int tmpA = temp >> 1;
                    if (temp % 2 != 0)
                    {
                        tmpA = tmpA ^ (int)a;
                    }
                    X[i] = (X[(i + m) % n] ^ tmpA);
                }
            }
            counter = 0;
        }

        public int Next()
        {
            if (counter == n)
            {
                Twist();
            }
            var y = X[counter];
            y = (int)(y ^ ((y >> u) & d));
            y = (int)(y ^ ((y << s) & b));
            y = (int)(y ^ ((y << t) & c));
            y = y ^ (y >> l);
            counter++;
            return y;
        }

    }
}
