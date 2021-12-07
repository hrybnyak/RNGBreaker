﻿using RNGBreaker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var predict = lcgBrokenResult.PredictNext();
            var betRequest = new BetRequest
            {
                Money = lcgBrokenResult.AccountBalance - 1,
                Number = predict,
                PlayerId = account.Id
            };
            var betResult = await _casinoHttpClient.MakeABet(Mode.Lcg, betRequest);
            if (betResult.Account.Money > 1000000)
            {
                Console.WriteLine(betResult.Message);
                return;
            }
            else
            {
                lcgBrokenResult.AccountBalance = betResult.Account.Money;
                lcgBrokenResult.LastValue = betResult.RealNumber;
                await WinLcg(account, lcgBrokenResult);
            }
        }

    }
}