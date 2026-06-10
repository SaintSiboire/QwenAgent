using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using QwenAgent.Core.Models;
using System.Diagnostics;

namespace QwenAgent.Core;

public static class Doctor
{
    private static bool CommandExists(string cmd)
    {
        try
        {
            var psi = new ProcessStartInfo(cmd, "--version")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using var p = Process.Start(psi)!;
            p.WaitForExit(3000);
            return p.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public static void Run(string projectRoot, QwenConfig config)
    {
        AnsiConsole.MarkupLine("[bold cyan]QwenAgent Doctor[/]");

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("Check");
        table.AddColumn("Status");
        table.AddColumn("Details");

        table.AddRow("Projet", "[green]OK[/]", projectRoot);

        table.AddRow("git",
            CommandExists("git") ? "[green]OK[/]" : "[red]Manquant[/]",
            "Requis pour appliquer les patchs (git apply)");

        table.AddRow("az",
            CommandExists("az") ? "[green]OK[/]" : "[yellow]Optionnel[/]",
            "Azure CLI pour scanner appsettings/logs");

        table.AddRow("ollama",
            CommandExists("ollama") ? "[green]OK[/]" : "[yellow]À vérifier[/]",
            "Requis si tu utilises Ollama en local");

        table.AddRow("Modèle",
            string.IsNullOrEmpty(config.Model.Name) ? "[red]Non défini[/]" : "[green]OK[/]",
            config.Model.Name);

        table.AddRow("Azure App",
            string.IsNullOrEmpty(config.Azure.AppName) ? "[yellow]Non détecté[/]" : "[green]OK[/]",
            config.Azure.AppName ?? "—");

        table.AddRow("Azure RG",
            string.IsNullOrEmpty(config.Azure.ResourceGroup) ? "[yellow]Non détecté[/]" : "[green]OK[/]",
            config.Azure.ResourceGroup ?? "—");

        AnsiConsole.Write(table);
    }
}

