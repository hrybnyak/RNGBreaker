using Newtonsoft.Json;
using RNGBreaker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RNGBreaker
{
    public class CasinoHttpClient
    {
        private readonly HttpClient _httpClient;

        private const string CreateAccountAddress = "createacc?id=";

        public CasinoHttpClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(@"http://95.217.177.249/casino/");
        }

        public async Task<AccountResponse> CreateAccount()
        {
            int counter = 0;
            while (true)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, CreateAccountAddress + counter);
                var result = await _httpClient.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var account = JsonConvert.DeserializeObject<AccountResponse>(content);
                    return account;
                }
                else
                {
                    counter++;
                }
            }
        }

        public async Task<BetResponse> MakeABet(Mode mode, BetRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, BuildBetRequestAddress(mode, request));
            var result = await _httpClient.SendAsync(httpRequest);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var betResponse = JsonConvert.DeserializeObject<BetResponse>(content);
                return betResponse;
            }
            else throw new InvalidOperationException($"The request returned {result.StatusCode}");
        }

        private string BuildBetRequestAddress(Mode mode, BetRequest request) =>
            $"play{mode}?id={request.PlayerId}&bet={request.Money}&number={request.Number}";
    }
}
