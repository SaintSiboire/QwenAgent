using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public static class ModeDetector
    {
        public static string Detect(string prompt)
        {
            var p = prompt.ToLower();

            if (p.Contains("résumé") || p.Contains("resume") || p.Contains("summary"))
                return "resume";

            if (p.Contains("analyse") || p.Contains("analyze") || p.Contains("analyser"))
                return "analyse";

            if (p.Contains("refactor") || p.Contains("optimise") || p.Contains("optimiser"))
                return "refactor";

            // Chat libre si aucune intention de modifier du code
            if (!p.Contains("corrige") &&
                !p.Contains("modifie") &&
                !p.Contains("change") &&
                !p.Contains("ajoute") &&
                !p.Contains("supprime") &&
                !p.Contains("fix") &&
                !p.Contains("diff"))
                return "chat";

            return "diff";
        }
    }
}
