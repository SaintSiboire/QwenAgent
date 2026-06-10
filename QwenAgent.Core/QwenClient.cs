using System;
using System.IO;
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

        private async Task StreamToConsoleAsync(string prompt)
        {
            var payload = new
            {
                model = "qwen2.5-coder:7b",
                stream = true,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var request = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _http.PostAsync(_endpoint, request);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n──────────────────────────────────────────────");
            Console.WriteLine(" Réponse de Qwen");
            Console.WriteLine("──────────────────────────────────────────────\n");
            Console.ResetColor();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var jsonObj = JsonDocument.Parse(line);

                    if (jsonObj.RootElement.TryGetProperty("message", out var msg))
                    {
                        if (msg.TryGetProperty("content", out var content))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(content.GetString());
                            Console.ResetColor();
                        }
                    }
                }
                catch
                {
                    // Ignore fragments JSON incomplets
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n──────────────────────────────────────────────");
            Console.WriteLine(" Fin de la réponse");
            Console.WriteLine("──────────────────────────────────────────────\n");
            Console.ResetColor();
        }

        public async Task<string> SendChatAsync(string prompt, string projectContext, string azureContext)
        {
            var fullPrompt =
                "Tu es un assistant expert en .NET, Azure, debugging et architecture.\n" +
                "Analyse la question en tenant compte du contexte suivant.\n\n" +
                "=== CONTEXTE DU PROJET ===\n" +
                projectContext + "\n\n" +
                "=== CONTEXTE AZURE ===\n" +
                azureContext + "\n\n" +
                "=== QUESTION ===\n" +
                prompt;

            await StreamToConsoleAsync(fullPrompt);
            return "";
        }

        public async Task<string> GetDiffAsync(string prompt, string projectContext, string azureContext)
        {
            var fullPrompt =
                "Tu es un agent spécialisé en génération de patchs DIFF.\n" +
                "Réponds UNIQUEMENT avec un patch diff valide.\n\n" +
                "=== CONTEXTE DU PROJET ===\n" +
                projectContext + "\n\n" +
                "=== CONTEXTE AZURE ===\n" +
                azureContext + "\n\n" +
                "=== INSTRUCTION ===\n" +
                prompt;

            await StreamToConsoleAsync(fullPrompt);
            return "";
        }
    }
}
