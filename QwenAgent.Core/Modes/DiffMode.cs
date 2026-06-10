using System.Threading.Tasks;

namespace QwenAgent.Core.Modes
{
    public class DiffMode : IAgentMode
    {
        private readonly QwenClient _client;

        public DiffMode(QwenClient client)
        {
            _client = client;
        }

        public async Task RunAsync(string prompt, ProjectContext context)
        {
            var project = context.ToJson();
            var azure = AzureDetector.DetectContext(context.RootPath);

            var diff = await _client.GetDiffAsync(prompt, project, azure);

            DiffApplier.Apply(diff, context.RootPath);
        }
    }
}
