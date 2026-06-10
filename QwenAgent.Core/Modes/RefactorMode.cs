using System.Threading.Tasks;

namespace QwenAgent.Core.Modes
{
    public class RefactorMode : IAgentMode
    {
        private readonly QwenClient _client;

        public RefactorMode(QwenClient client)
        {
            _client = client;
        }

        public async Task RunAsync(string prompt, ProjectContext context)
        {
            var project = context.ToJson();
            var azure = AzureDetector.DetectContext(context.RootPath);

            var fullPrompt =
                $"Refactorise ce projet selon les meilleures pratiques.\n\n{prompt}";

            var diff = await _client.GetDiffAsync(fullPrompt, project, azure);

            DiffApplier.Apply(diff, context.RootPath);
        }
    }
}
