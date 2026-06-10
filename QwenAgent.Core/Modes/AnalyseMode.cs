using System.Threading.Tasks;

namespace QwenAgent.Core.Modes
{
    public class AnalyseMode : IAgentMode
    {
        private readonly QwenClient _client;

        public AnalyseMode(QwenClient client)
        {
            _client = client;
        }

        public async Task RunAsync(string prompt, ProjectContext context)
        {
            var project = context.GenerateSolutionOverview();
            var azure = AzureDetector.DetectContext(context.RootPath);

            await _client.SendChatAsync(
                $"Analyse cette solution et donne un rapport détaillé.\n\n{prompt}",
                project,
                azure
            );
        }
    }
}
