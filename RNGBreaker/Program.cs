using System;
using System.Threading;
using System.Threading.Tasks;

namespace RNGBreaker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //var casinoClient = new CasinoHttpClient();
                //var rngBreaker = new RNGBreaker(casinoClient);
                //await rngBreaker.BreakLcg();
                //await rngBreaker.BreakMt();
                var mt = new MT19937((uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                var values = new uint[624];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = mt.Next();
                }
                var broken = BetterMTBreaker.BreakBetterMt(values);
                var next = mt.Next();
                var nextBroken = broken.Next();
                Console.WriteLine(nextBroken == next);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
