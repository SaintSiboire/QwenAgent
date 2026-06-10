using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var overview = context.GenerateSolutionOverview();
            var response = await _client.SendChatAsync(
                $"Analyse cette solution et donne un rapport détaillé:\n{overview}"
            );

            Console.WriteLine(response);
        }
    }
}
