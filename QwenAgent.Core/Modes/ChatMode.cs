using System.Threading.Tasks;

namespace QwenAgent.Core.Modes
{
    public class ChatMode : IAgentMode
    {
        private readonly QwenClient _client;

        public ChatMode(QwenClient client)
        {
            _client = client;
        }

        public async Task RunAsync(string prompt, ProjectContext context)
        {
            var project = context.GenerateSolutionOverview();
            var azure = AzureDetector.DetectContext(context.RootPath);

            await _client.SendChatAsync(prompt, project, azure);
        }
    }
}
