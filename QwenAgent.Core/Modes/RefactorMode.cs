using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var overview = context.GenerateSolutionOverview();

            var diff = await _client.GetDiffAsync(
                $"Refactorise ce projet selon les meilleures pratiques:\n{overview}",
                context.ToJson(),
                ""
            );

            DiffApplier.Apply(diff, context.RootPath);
        }
    }
}
