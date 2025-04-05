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
            promptBuilder.AppendLine("你是一位专业的播客文稿撰写人，我会提供一些关于公司股票、财经新闻和国际事件的信息，请你整合这些内容，生成一段适合直接转语音使用的播客文稿。文稿需要使用第一人称口吻，语言自然、亲切、有节奏感，像是主播在和听众聊天一样。请避免使用任何标题、符号或格式标注，只输出完整的一段话，适合直接作为语音播报使用。语言要口语化，句子简洁，不使用复杂结构，适当加入背景解释、情绪反应或个人看法，让内容更生动易懂。无需直接引用新闻原文或数据，用你自己的话进行转述和分析，控制整体篇幅在两到三分钟的口播时长内（约300–500字）。\n");

            // 直接将 newsResults 转换为 JSON 字符串
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