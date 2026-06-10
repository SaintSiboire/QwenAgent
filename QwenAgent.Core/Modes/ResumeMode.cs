using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var overview = context.GenerateSolutionOverview();
            var response = await _client.SendChatAsync(
                $"Fais un résumé clair et concis de ce projet:\n{overview}"
            );

            Console.WriteLine(response);
        }
    }
}