using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNGBreaker
{
    public class MT19937
    {
        public const uint w = 32;
        public const uint n = 624;
        public const uint m = 397;
        public const uint r = 31;
        public const uint a = 0x9908B0DF;
        public const uint d = 0xFFFFFFFF;
        public const uint c = 0xEFC60000;
        public const uint b = 0x9D2C5680;
        public const int u = 11;
        public const int s = 7;
        public const int t = 15;
        public const int l = 18;
        public const uint f = 1812433253U;
        private uint[] X = new uint[n];
        private uint counter = 0;

        public MT19937(uint seed)
        {
            X[0] = seed;
            for (uint i = 1; i < n; i++)
            {
                X[i] = f * (X[i - 1] ^ (X[i - 1] >> 30)) + i;
            }
            Twist();
        }

        private void Twist()
        {
            for (int i = 0; i < n; i++)
            {
                unchecked
                {
                    var lowerMask = 2147483647;
                    var upperMask = 0x80000000;
                    var temp = (X[i] & upperMask) | (X[(i + 1) % n] & lowerMask);
                    var tmpA = temp >> 1;
                    if (temp % 2 != 0)
                    {
                        tmpA = tmpA ^ a;
                    }
                    X[i] = (uint) (X[(i + m) % n] ^ tmpA);
                }
            }
            counter = 0;
        }

        public uint Next()
        {
            if (counter >= n)
            {
                Twist();
            }
            var y = X[counter];
            y = y ^ (y >> u);
            y = (y ^ ((y << s) & b));
            y = (y ^ ((y << t) & c));
            y = y ^ (y >> l);
            counter++;
            return y;
        }

    }
}
