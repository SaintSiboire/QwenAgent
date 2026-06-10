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

        // appsettings*.json
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

        // pipelines YAML
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
}

