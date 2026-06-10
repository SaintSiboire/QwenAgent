using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public class ProjectContext
    {
        public string RootPath { get; }
        public List<string> Files { get; }

        public ProjectContext(string rootPath)
        {
            RootPath = rootPath;
            Files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".cs") || f.EndsWith(".json") || f.EndsWith(".csproj"))
                .ToList();
        }

        public string GenerateSolutionOverview()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Fichiers du projet ===");

            foreach (var file in Files)
                sb.AppendLine("- " + Path.GetRelativePath(RootPath, file));

            return sb.ToString();
        }

        public string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(Files);
        }

        public static ProjectContext Load(string path)
        {
            return new ProjectContext(path);
        }
    }
}
