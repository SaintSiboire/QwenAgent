using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using QwenAgent.Core.Models;

namespace QwenAgent.Core;

public static class AzureDetector
{
    public static void EnrichAzureConfig(QwenConfig config, string projectRoot)
    {
        if (!string.IsNullOrEmpty(config.Azure.AppName) &&
            !string.IsNullOrEmpty(config.Azure.ResourceGroup))
            return;

        foreach (var file in Directory.GetFiles(projectRoot, "appsettings*.json", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(file);

            if (string.IsNullOrEmpty(config.Azure.AppName))
            {
                var m = Regex.Match(content, @"app-[a-zA-Z0-9\-]+", RegexOptions.IgnoreCase);
                if (m.Success)
                    config.Azure.AppName = m.Value;
            }
        }

        foreach (var file in Directory.GetFiles(projectRoot, "*.yml", SearchOption.AllDirectories)
                     .Concat(Directory.GetFiles(projectRoot, "*.yaml", SearchOption.AllDirectories)))
        {
            var content = File.ReadAllText(file);

            if (string.IsNullOrEmpty(config.Azure.ResourceGroup))
            {
                var m = Regex.Match(content, @"resourceGroup:\s*(?<rg>[A-Za-z0-9\-_]+)", RegexOptions.IgnoreCase);
                if (m.Success)
                    config.Azure.ResourceGroup = m.Groups["rg"].Value;
            }
        }
    }

    public static string DetectContext(string projectRoot)
    {
        if (!Directory.Exists(projectRoot))
            return "Aucun contexte Azure détecté.";

        var sb = new StringBuilder();

        sb.AppendLine("=== Azure Context Detected ===");

        // 1. appsettings.json
        var appsettings = Directory.GetFiles(projectRoot, "appsettings*.json", SearchOption.AllDirectories);
        if (appsettings.Length > 0)
        {
            sb.AppendLine("\nFichiers appsettings détectés :");
            foreach (var file in appsettings)
            {
                sb.AppendLine($"- {file}");
                sb.AppendLine(File.ReadAllText(file));
            }
        }

        // 2. YAML (pipelines, infra)
        var yamls = Directory.GetFiles(projectRoot, "*.yml", SearchOption.AllDirectories)
            .Concat(Directory.GetFiles(projectRoot, "*.yaml", SearchOption.AllDirectories))
            .ToList();

        if (yamls.Count > 0)
        {
            sb.AppendLine("\nFichiers YAML Azure détectés :");
            foreach (var file in yamls)
            {
                sb.AppendLine($"- {file}");
                sb.AppendLine(File.ReadAllText(file));
            }
        }

        // 3. Fichiers ARM/Bicep
        var bicep = Directory.GetFiles(projectRoot, "*.bicep", SearchOption.AllDirectories);
        if (bicep.Length > 0)
        {
            sb.AppendLine("\nFichiers Bicep détectés :");
            foreach (var file in bicep)
            {
                sb.AppendLine($"- {file}");
                sb.AppendLine(File.ReadAllText(file));
            }
        }

        return sb.ToString();
    }
}


