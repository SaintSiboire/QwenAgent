using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using QwenAgent.Core.Models;

namespace QwenAgent.Core;

public static class AzureScanner
{
    private static string SafeExec(string args)
    {
        try
        {
            var psi = new ProcessStartInfo("az", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var p = Process.Start(psi)!;
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit(5000);
            return output;
        }
        catch (Exception ex)
        {
            return $"Erreur lors de 'az {args}': {ex.Message}";
        }
    }

    public static string BuildAzureContext(AzureConfig azure)
    {
        if (string.IsNullOrEmpty(azure.AppName) || string.IsNullOrEmpty(azure.ResourceGroup))
            return "Azure non configuré (appName ou resourceGroup manquant).";

        var sb = new StringBuilder();

        sb.AppendLine("## App Settings");
        sb.AppendLine(SafeExec($"webapp config appsettings list -g {azure.ResourceGroup} -n {azure.AppName}"));

        sb.AppendLine("## Config");
        sb.AppendLine(SafeExec($"webapp config show -g {azure.ResourceGroup} -n {azure.AppName}"));

        sb.AppendLine("## Logs (tail 5s)");
        sb.AppendLine(SafeExec($"webapp log tail -g {azure.ResourceGroup} -n {azure.AppName} --timeout 5"));

        return sb.ToString();
    }
}


