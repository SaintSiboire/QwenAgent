using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using QwenAgent.Core.Models;

namespace QwenAgent.Core;

public class QwenClient
{
    private readonly HttpClient _http;
    private readonly ModelConfig _model;

    public QwenClient(ModelConfig model)
    {
        _model = model;
        _http = new HttpClient { BaseAddress = new Uri(model.BaseUrl) };
    }

    public async Task<string> GetDiffAsync(string userPrompt, string projectContext, string azureContext)
    {
        var body = new
        {
            model = _model.Name,
            messages = new[]
            {
                new { role = "system", content = "Tu es un agent DevOps + Debug + CodeFix. Tu dois diagnostiquer, corréler code + Azure + logs, et renvoyer UNIQUEMENT un patch unifié (diff -u), sans explication autour." },
                new { role = "user", content = $"{userPrompt}\n\n### CONTEXTE PROJET\n{projectContext}\n\n### CONTEXTE AZURE\n{azureContext}" }
            },
            stream = false
        };

        var resp = await _http.PostAsJsonAsync("/api/chat", body);
        resp.EnsureSuccessStatusCode();
        var json = await resp.Content.ReadFromJsonAsync<dynamic>();
        string content = json.message.content;
        return content;
    }
}

