using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public class QwenClient
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;

        public QwenClient()
        {
            _http = new HttpClient();
            _endpoint = "http://localhost:11434/api/chat";
        }

        private async Task<string> SendToOllamaAsync(string model, string prompt)
        {
            var payload = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await _http.PostAsync(
                _endpoint,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            return await response.Content.ReadAsStringAsync();
        }

        // MODE CHAT / ANALYSE / RESUME
        public Task<string> SendChatAsync(string prompt)
        {
            return SendToOllamaAsync("qwen2.5:14b-instruct", prompt);
        }

        // MODE DIFF
        public Task<string> GetDiffAsync(string prompt, string projectContext, string azureContext)
        {
            var fullPrompt =
                "Tu es un agent de génération de diff. " +
                "Réponds UNIQUEMENT avec un patch diff valide.\n\n" +
                "Instruction : " + prompt + "\n\n" +
                "Contexte du projet : " + projectContext;

            return SendToOllamaAsync("qwen2.5-coder:7b", fullPrompt);
        }
    }
}
