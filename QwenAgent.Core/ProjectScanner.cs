using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using QwenAgent.Core.Models;

namespace QwenAgent.Core;

public static class ProjectScanner
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".cshtml", ".js", ".ts", ".tsx", ".json", ".html", ".css"
    };

    public static string BuildContext(string projectRoot, ProjectConfig projectConfig)
    {
        var sb = new StringBuilder();
        var root = new DirectoryInfo(projectRoot);
        var ignore = new HashSet<string>(projectConfig.Ignore, StringComparer.OrdinalIgnoreCase);

        foreach (var file in root.EnumerateFiles("*.*", SearchOption.AllDirectories))
        {
            if (ignore.Any(ig => file.FullName.Contains(Path.DirectorySeparatorChar + ig + Path.DirectorySeparatorChar)))
                continue;

            if (!AllowedExtensions.Contains(file.Extension))
                continue;

            var relPath = Path.GetRelativePath(projectRoot, file.FullName);
            var content = File.ReadAllText(file.FullName);

            sb.AppendLine($"### FILE: {relPath}");
            sb.AppendLine(content);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

