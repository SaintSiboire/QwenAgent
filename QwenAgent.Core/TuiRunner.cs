using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace QwenAgent.Core;

public static class TuiRunner
{
    public static bool ConfirmApply(string projectRoot, string azureSummary, string diffPreview)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold cyan]Qwen Agent – Diagnostic[/]");

        AnsiConsole.MarkupLine($"\n[bold]Projet:[/] {projectRoot}");
        AnsiConsole.MarkupLine($"\n[bold]Azure:[/]\n{azureSummary}");

        AnsiConsole.MarkupLine("\n[bold yellow]Aperçu du diff généré :[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine(diffPreview);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nAppliquer ce patch ?")
                .AddChoices("Oui, appliquer", "Non, annuler"));

        return choice.StartsWith("Oui");
    }
}

