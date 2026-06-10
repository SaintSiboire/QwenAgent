namespace QwenAgent.Core.Models;
public class QwenConfig
{
    public AzureConfig Azure { get; set; } = new();
    public ProjectConfig Project { get; set; } = new();
    public ModelConfig Model { get; set; } = new();
}

public class AzureConfig
{
    public string? ResourceGroup { get; set; }
    public string? AppName { get; set; }
}

public class ProjectConfig
{
    public string Root { get; set; } = ".";
    public List<string> Ignore { get; set; } = new()
    {
        "bin", "obj", "node_modules", ".git", "dist"
    };
}

public class ModelConfig
{
    public string Name { get; set; } = "qwen2.5-coder:7b";
    public string BaseUrl { get; set; } = "http://localhost:11434";
}
