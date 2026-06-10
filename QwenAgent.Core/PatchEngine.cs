using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QwenAgent.Core;

public static class PatchEngine
{
    public static void ApplyWithGit(string diff, string projectRoot)
    {
        var temp = Path.Combine(Path.GetTempPath(), $"qwen_patch_{Guid.NewGuid()}.diff");
        File.WriteAllText(temp, diff);

        var psi = new ProcessStartInfo("git", $"apply \"{temp}\"")
        {
            WorkingDirectory = projectRoot,
            UseShellExecute = false
        };
        using var p = Process.Start(psi)!;
        p.WaitForExit();
    }
}


