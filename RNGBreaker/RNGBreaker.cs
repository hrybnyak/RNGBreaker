using RNGBreaker.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RNGBreaker
{
    public class RNGBreaker
    {
        private readonly CasinoHttpClient _casinoHttpClient;
        public RNGBreaker(CasinoHttpClient casinoHttpClient)
        {
            _casinoHttpClient = casinoHttpClient;
        }

        public async Task BreakLcg()
        {
            var account = await _casinoHttpClient.CreateAccount();
            var broken = await BreakLcg(account);
            Console.WriteLine(broken.A);
            Console.WriteLine(broken.C);
            await WinLcg(account, broken);
        }

        private async Task<LcgBrokenResult> BreakLcg(AccountResponse account)
        {
            while (true)
            {
                try
                {
                    var values = new List<long>();
                    var accountBalance = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        var betRequest = new BetRequest
                        {
                            PlayerId = account.Id,
                            Money = 1,
                            Number = 1
                        };
                        var result = await _casinoHttpClient.MakeABet(Mode.Lcg, betRequest);
                        accountBalance = result.Account.Money;
                        values.Add(result.RealNumber);
                    }
                    var broken = LcgBreaker.Break(values);
                    return new LcgBrokenResult
                    {
                        A = broken.Item1,
                        C = broken.Item2,
                        M = LcgBreaker.M,
                        LastValue = (int)values[2],
                        AccountBalance = accountBalance
                    };
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
            }
        }

        private async Task WinLcg (AccountResponse account, LcgBrokenResult lcgBrokenResult)
        {
            await Win(Mode.Lcg, account, () => lcgBrokenResult.PredictNext(), lcgBrokenResult.AccountBalance);
        }

        public async Task BreakMt()
        {
            var startTime = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var account = await _casinoHttpClient.CreateAccount();
            var result = await BreakMt(account, startTime);
            if (result == null)
            {
                throw new ArgumentException("Couldn't find rigth number");
            }
            await WinMt(account, result);
        }

        public async Task<MT19937> BreakMt(AccountResponse account, uint start)
        {
            var betRequest = new BetRequest
            {
                PlayerId = account.Id,
                Money = 1,
                Number = 1
            };
            var result = await _casinoHttpClient.MakeABet(Mode.Mt, betRequest);
            var end = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            for (uint i = start; i < end; i++)
            {
                var mt = new MT19937(i);
                for (int j = 0; j < 626; j++)
                {
                    var number = mt.Next();
                    if (number == result.RealNumber)
                    {
                        return mt;
                    }
                }
            }
            return null;
        }

        public async Task WinMt (AccountResponse account, MT19937 mtGenerator)
        {
            await Win(Mode.Mt, account, () => mtGenerator.Next(), account.Money - 1);
        }

        public async Task Win (Mode mode, AccountResponse account, Func<long> numberGenerator, int accountBalance)
        {
            var predict = numberGenerator.Invoke();
            var betRequest = new BetRequest
            {
                Money = accountBalance / 2,
                Number = predict,
                PlayerId = account.Id
            };
            var betResult = await _casinoHttpClient.MakeABet(mode, betRequest);
            if (betResult.Account.Money > 1000000)
            {
                Console.WriteLine(betResult.Message);
                return;
            }
            else
            {
                await Win(mode, account, numberGenerator, betResult.Account.Money);
            }
        }
    }
}
