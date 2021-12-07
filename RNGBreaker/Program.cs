using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RNGBreaker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var casinoClient = new CasinoHttpClient();
            var rngBreaker = new RNGBreaker(casinoClient);
            await rngBreaker.BreakLcg();
        }
    }
}
