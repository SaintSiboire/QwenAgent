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
    var installPath = AppContext.BaseDirectory;
    await Updater.RunUpdateAsync(installPath);
    return;
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
