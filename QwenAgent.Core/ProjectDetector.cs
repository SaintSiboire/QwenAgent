using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace QwenAgent.Core;

public static class ProjectDetector
{
    public static string DetectProjectRoot(string startDirectory)
    {
        try
        {
            var repoPath = Repository.Discover(startDirectory);
            if (repoPath != null)
            {
                using var repo = new Repository(repoPath);
                return repo.Info.WorkingDirectory;
            }
        }
        catch
        {
        }

        var dir = new DirectoryInfo(startDirectory);
        while (dir != null)
        {
            if (dir.GetFiles("*.sln").Any() || dir.GetFiles("*.csproj").Any())
                return dir.FullName;

            dir = dir.Parent;
        }

        return Path.GetFullPath(startDirectory);
    }
}


