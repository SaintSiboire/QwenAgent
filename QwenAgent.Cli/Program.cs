using System.Diagnostics;
using QwenAgent.Core;
using Spectre.Console;

var argsList = args.ToList();
var startDir = Environment.CurrentDirectory;

// Commandes :
// qwen-fix                 -> mode interactif
// qwen-fix doctor          -> diagnostic
// qwen-fix update          -> mise à jour
// qwen-fix "prompt..."     -> prompt direct

if (argsList.Count > 0 && argsList[0].Equals("doctor", StringComparison.OrdinalIgnoreCase))
{
    var projectRootDoctor = ProjectDetector.DetectProjectRoot(startDir);
    var configDoctor = ConfigLoader.Load(projectRootDoctor);
    AzureDetector.EnrichAzureConfig(configDoctor, projectRootDoctor);
    Doctor.Run(projectRootDoctor, configDoctor);
    return;
}

if (argsList.Count > 0 && argsList[0].Equals("update", StringComparison.OrdinalIgnoreCase))
{
    var scriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "installer", "update.ps1");
    if (!File.Exists(scriptPath))
    {
        AnsiConsole.MarkupLine("[red]update.ps1 introuvable. Vérifie le dossier installer/.[/]");
        return;
    }

    Process.Start(new ProcessStartInfo("powershell",
        $"-ExecutionPolicy Bypass -File \"{scriptPath}\"")
    {
        UseShellExecute = true
    });
    return;
}

var projectRoot = ProjectDetector.DetectProjectRoot(startDir);
var config = ConfigLoader.Load(projectRoot);
AzureDetector.EnrichAzureConfig(config, projectRoot);

string userPrompt;
if (argsList.Count > 0)
{
    userPrompt = string.Join(" ", argsList);
}
else
{
    userPrompt = AnsiConsole.Ask<string>("Décris le problème à diagnostiquer :");
}

AnsiConsole.MarkupLine("[cyan]Scan du projet...[/]");
var projectContext = ProjectScanner.BuildContext(projectRoot, config.Project);

AnsiConsole.MarkupLine("[cyan]Scan Azure...[/]");
var azureContext = AzureScanner.BuildAzureContext(config.Azure);

AnsiConsole.MarkupLine("[cyan]Appel à Qwen...[/]");
var client = new QwenClient(config.Model);
var diff = await client.GetDiffAsync(userPrompt, projectContext, azureContext);

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

AnsiConsole.MarkupLine("[yellow]Application du patch via git apply...[/]");
PatchEngine.ApplyWithGit(diff, projectRoot);
AnsiConsole.MarkupLine("[green]Terminé.[/]");
