using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using QwenAgent.Core.Models;
using System.Text.Json;

namespace QwenAgent.Core
{
    public class QwenClient
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;

        public QwenClient()
        {
            _http = new HttpClient();

            // IMPORTANT : mets ici ton vrai endpoint Qwen
            _endpoint = "https://api-inference.qwen.ai/v1/chat/completions";
        }

        // MODE CHAT LIBRE
        public async Task<string> SendChatAsync(string prompt)
        {
            var payload = new
            {
                model = "qwen2.5-coder",
                input = prompt
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await _http.PostAsync(
                _endpoint,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            return await response.Content.ReadAsStringAsync();
        }

        // MODE DIFF (TON MODE ACTUEL)
        public async Task<string> GetDiffAsync(string prompt, string projectContext, string azureContext)
        {
            var payload = new
            {
                model = "qwen2.5-coder",
                input = new
                {
                    instruction = prompt,
                    project = projectContext,
                    azure = azureContext
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await _http.PostAsync(
                _endpoint,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var content = await response.Content.ReadAsStringAsync();

            // On parse proprement
            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.TryGetProperty("diff", out var diffProp))
                    return diffProp.GetString();

                // fallback : renvoyer tout le texte
                return content;
            }
            catch
            {
                return content;
            }
        }
    }
}


