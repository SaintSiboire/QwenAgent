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
            // ignore
        }

        return Path.GetFullPath(startDirectory);
    }
}

