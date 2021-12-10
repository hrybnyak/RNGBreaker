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
                var casinoClient = new CasinoHttpClient();
                var rngBreaker = new RNGBreaker(casinoClient);
                //await rngBreaker.BreakLcg();
                await rngBreaker.BreakMt();
                await rngBreaker.BreakBetterMt();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
