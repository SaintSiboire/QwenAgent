using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public interface IAgentMode
    {
        Task RunAsync(string prompt, ProjectContext context);
    }
}

