using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using api.Helpers;

namespace api.Service
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public NewsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<object>> GetNewsForCompaniesAsync(List<string> companyNames)
        {
            var newsResults = new List<object>();
            var date = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");
            var apiKey = _config["NewsKey"];

            foreach (var name in companyNames)
            {
                var keyword = SearchHelper.ExtractSearchKeyword(name);

                var url = $"https://newsapi.org/v2/everything?q={Uri.EscapeDataString(keyword)}&from={date}&sortBy=popularity&apiKey={apiKey}";

                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("User-Agent", "Mozilla/5.0");

                    var response = await _httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[❌] HTTP {response.StatusCode} - {url}");
                        continue;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    dynamic newsObj = JsonConvert.DeserializeObject<dynamic>(json);

                    var articles = newsObj?.articles;
                    if (articles == null) continue;

                    var topArticles = new List<object>();
                    int count = 0;

                    foreach (var article in articles)
                    {
                        if (count++ >= 3) break;

                        string title = article.title;
                        string urlArticle = article.url;
                        string description = article.description;
                        string content = article.content;

                        topArticles.Add(new
                        {
                            Title = title?.ToString(),
                            Url = urlArticle?.ToString(),
                            Description = description?.ToString(),
                            Content = content?.ToString()
                        });
                    }

                    newsResults.Add(new
                    {
                        Company = name,
                        News = topArticles
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {name}: {ex.Message}");
                    continue;
                }
            }

            return newsResults;
        }
    }
}