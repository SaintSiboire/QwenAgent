using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var diff = await _client.GetDiffAsync(
                prompt,
                context.ToJson(),
                "" // azure context si tu l'utilises
            );

            DiffApplier.Apply(diff, context.RootPath);
        }
    }
}
