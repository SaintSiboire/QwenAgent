using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QwenAgent.Core.Models;

namespace QwenAgent.Core;

public static class ConfigLoader
{
    public static QwenConfig Load(string startDirectory)
    {
        var dir = new DirectoryInfo(startDirectory);

        while (dir != null)
        {
            var qwenDir = Path.Combine(dir.FullName, ".qwen");
            var configPath = Path.Combine(qwenDir, "config.json");
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                var cfg = JsonConvert.DeserializeObject<QwenConfig>(json);
                if (cfg != null)
                    return cfg;
            }

            dir = dir.Parent;
        }

        return new QwenConfig
        {
            Project = new ProjectConfig
            {
                Root = startDirectory
            }
        };
    }
}

