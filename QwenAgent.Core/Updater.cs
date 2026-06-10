using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public static class Updater
    {
        private static readonly HttpClient _http = new HttpClient();

        public static async Task RunUpdateAsync(string installPath)
        {
            Console.WriteLine("Recherche de la dernière version...");

            var apiUrl = "https://api.github.com/repos/SaintSiboire/QwenAgent/releases/latest";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            request.Headers.Add("User-Agent", "QwenAgent-Updater");

            var response = await _http.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var tag = root.GetProperty("tag_name").GetString();
            var asset = root.GetProperty("assets")[0];
            var downloadUrl = asset.GetProperty("browser_download_url").GetString();

            Console.WriteLine($"Dernière version trouvée : {tag}");
            Console.WriteLine("Téléchargement...");

            var zipPath = Path.Combine(Path.GetTempPath(), "QwenAgent_Update.zip");
            var zipBytes = await _http.GetByteArrayAsync(downloadUrl);
            await File.WriteAllBytesAsync(zipPath, zipBytes);

            Console.WriteLine("Extraction...");

            ZipFile.ExtractToDirectory(zipPath, installPath, overwriteFiles: true);

            File.WriteAllText(Path.Combine(installPath, "version.txt"), tag);

            Console.WriteLine($"Mise à jour terminée. Version installée : {tag}");
        }
    }
}
