using System;
using System.Threading;
using System.Threading.Tasks;

namespace RNGBreaker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var casinoClient = new CasinoHttpClient();
            //var rngBreaker = new RNGBreaker(casinoClient);
            //await rngBreaker.BreakLcg();
            var start = 50;
            var end = 150;
            Random rand = new Random();
            Thread.Sleep(rand.Next(start, end));
            var time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var mt = new MT19937(time);
            Thread.Sleep(rand.Next(start, end));
            var value = mt.Next();
            var timeNow = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            for (int i = timeNow - start; i < timeNow + end; i++)
            {
                var generator = new MT19937(i);
                var number = generator.Next();
                if (number == value)
                {
                    Console.WriteLine("Seed: " + number);
                }
            }

        }
    }
}
