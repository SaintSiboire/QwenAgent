using QwenAgent.Core;
using Spectre.Console;

var startDir = Environment.CurrentDirectory;

// 1. Détection du projet
var projectRoot = ProjectDetector.DetectProjectRoot(startDir);

// 2. Chargement config (avec fallback)
var config = ConfigLoader.Load(projectRoot);

// 3. Enrichissement Azure auto
AzureDetector.EnrichAzureConfig(config, projectRoot);

// 4. Prompt utilisateur
var userPrompt = AnsiConsole.Ask<string>("Décris le problème à diagnostiquer :");

// 5. Scan projet
AnsiConsole.MarkupLine("[cyan]Scan du projet...[/]");
var projectContext = ProjectScanner.BuildContext(projectRoot, config.Project);

// 6. Scan Azure
AnsiConsole.MarkupLine("[cyan]Scan Azure...[/]");
var azureContext = AzureScanner.BuildAzureContext(config.Azure);

// 7. Appel Qwen
AnsiConsole.MarkupLine("[cyan]Appel à Qwen...[/]");
var client = new QwenClient(config.Model);
var diff = await client.GetDiffAsync(userPrompt, projectContext, azureContext);

// 8. TUI : aperçu + confirmation
var preview = string.Join(Environment.NewLine, diff.Split('\n').Take(80));
var azureSummary = string.IsNullOrWhiteSpace(config.Azure.AppName)
    ? "Aucune config Azure fiable détectée."
    : $"App: {config.Azure.AppName}, RG: {config.Azure.ResourceGroup}";

var apply = TuiRunner.ConfirmApply(projectRoot, azureSummary, preview);

if (!apply)
{
    AnsiConsole.MarkupLine("[red]Patch annulé.[/]");
    return;
}

// 9. Application du patch
AnsiConsole.MarkupLine("[yellow]Application du patch via git apply...[/]");
PatchEngine.ApplyWithGit(diff, projectRoot);
AnsiConsole.MarkupLine("[green]Terminé.[/]");
