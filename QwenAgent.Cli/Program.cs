using QwenAgent.Core;
using QwenAgent.Core.Modes;

var argsList = args.ToList();

if (argsList.Count == 0)
{
    HelpPrinter.Print();
    return;
}

var prompt = string.Join(" ", argsList);

// Commande help
if (prompt == "help" || prompt == "--help" || prompt == "-h")
{
    HelpPrinter.Print();
    return;
}

// Commande update
if (prompt == "--update")
{
    // Détection automatique du dossier d'installation
    var exeDir = AppContext.BaseDirectory;
    var installPathFile = Path.Combine(exeDir, "install_path.txt");

    string installPath;

    if (File.Exists(installPathFile))
    {
        installPath = File.ReadAllText(installPathFile).Trim();
    }
    else
    {
        // Fallback : utiliser le dossier courant
        installPath = exeDir;
    }

    var updaterExe = Path.Combine(installPath, "QwenAgent.Updater.exe");

    Console.WriteLine("Lancement de l'updater externe...");

    System.Diagnostics.Process.Start(updaterExe, $"\"{installPath}\"");

    Environment.Exit(0);
}


// Détection automatique du mode
var modeName = ModeDetector.Detect(prompt);
Console.WriteLine($"Mode détecté : {modeName}");

var client = new QwenClient();
var context = ProjectContext.Load(Environment.CurrentDirectory);

IAgentMode mode = modeName switch
{
    "chat" => new ChatMode(client),
    "analyse" => new AnalyseMode(client),
    "resume" => new ResumeMode(client),
    "refactor" => new RefactorMode(client),
    _ => new DiffMode(client)
};

await mode.RunAsync(prompt, context);
