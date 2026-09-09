using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Services.ToxicBertService;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services.ToxicBert
{
    public class ToxicBertService : IToxicBertService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public ToxicBertService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ToxicScoreDto>> ToxicScore(string text)
        {
            var apiKey = _config["ToxicBert:Token"] ?? throw new InvalidOperationException("ToxicBert:Token ayarı `appsettings.json` dosyasında bulunamadı.");
            var url = _config["ToxicBert:URL"] ?? throw new InvalidOperationException("ToxicBert:URL ayarı `appsettings.json` dosyasında bulunamadı.");

            try
            {
                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var responseMessage = new
                {
                    inputs = text
                };

                var json = JsonConvert.SerializeObject(responseMessage);

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, stringContent);

                if (!response.IsSuccessStatusCode) return null;

                var file = await response.Content.ReadAsStringAsync();

                List<ToxicScoreDto> toxicScores = new List<ToxicScoreDto>();

                if (file.TrimStart().StartsWith("["))
                {
                    var doc = JsonDocument.Parse(file);

                    foreach (var item in doc.RootElement[0].EnumerateArray())
                    {
                        var toxicScore = new ToxicScoreDto
                        {
                            Score = item.GetProperty("score").GetDouble(),
                            Label = item.GetProperty("label").GetString()
                        };

                        toxicScores.Add(toxicScore);
                    }

                    return toxicScores;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }
    }
}
