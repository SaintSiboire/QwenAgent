using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QwenAgent.Core
{
    public static class FileScanner
    {
        public static string ScanProject(string rootPath)
        {
            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".cs") || f.EndsWith(".json") || f.EndsWith(".csproj"));

            return string.Join("\n", files.Select(f => Path.GetRelativePath(rootPath, f)));
        }
    }
}

