using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace api.Service
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> GeneratePodcastAsync(List<object>? newsResults)
        {
            var apiKey = _config["DeepSeekKey"];
            var apiUrl = "https://api.deepseek.com/v1/chat/completions";

            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("You are a professional podcast script writer. I will provide you with some information about company stocks, financial news, and international events. Your task is to integrate this information and generate a script suitable for direct voice recording. The script should be written in the first person, using a natural, friendly, and rhythmic tone, as if the host is casually chatting with the audience. Please avoid using any titles, symbols, or formatting marks, and only output a complete paragraph that is ready for voice narration. The language should be conversational, with short, simple sentences and no complex structures. Feel free to add background explanations, emotional reactions, or personal opinions to make the content more lively and engaging. Do not directly quote news articles or data; instead, paraphrase and analyze the information in your own words. Keep the total length suitable for a two to three-minute podcast (around 300–500 words).\n");

            string newsJson = JsonConvert.SerializeObject(newsResults);
            promptBuilder.AppendLine(newsJson);

            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
                    new { role = "user", content = promptBuilder.ToString() }
                },
                temperature = 0.7
            };

            var requestJson = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[AIService ❌] HTTP {response.StatusCode}");
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject<dynamic>(responseString);
                string output = result.choices[0].message.content;
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AIService ERROR] {ex.Message}");
                return null;
            }
        }
    }
}