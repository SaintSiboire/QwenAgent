using System.Threading.Tasks;

namespace QwenAgent.Core.Modes
{
    public class ResumeMode : IAgentMode
    {
        private readonly QwenClient _client;

        public ResumeMode(QwenClient client)
        {
            _client = client;
        }

        public async Task RunAsync(string prompt, ProjectContext context)
        {
            var project = context.GenerateSolutionOverview();
            var azure = AzureDetector.DetectContext(context.RootPath);

            await _client.SendChatAsync(
                $"Fais un résumé clair et concis de ce projet.\n\n{prompt}",
                project,
                azure
            );
        }
    }
}
