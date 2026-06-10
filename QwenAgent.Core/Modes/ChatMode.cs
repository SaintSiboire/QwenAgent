using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var response = await _client.SendChatAsync(prompt);
            Console.WriteLine(response);
        }
    }
}
