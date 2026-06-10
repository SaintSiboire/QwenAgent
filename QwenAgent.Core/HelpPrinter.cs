using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QwenAgent.Core
{
    public static class HelpPrinter
    {
        public static void Print()
        {
            Console.WriteLine("QwenAgent - Commandes disponibles :\n");

            Console.WriteLine("qwen-fix \"Corrige ce fichier\"        → Mode diff (modification de code)");
            Console.WriteLine("qwen-fix \"Fais-moi un résumé\"        → Mode résumé");
            Console.WriteLine("qwen-fix \"Analyse ma solution\"       → Mode analyse");
            Console.WriteLine("qwen-fix \"Optimise tout le projet\"   → Mode refactor");
            Console.WriteLine("qwen-fix \"Crée un cahier des charges\"→ Mode analyse/chat");
            Console.WriteLine("qwen-fix --update                     → Met à jour QwenAgent depuis GitHub");
            Console.WriteLine("qwen-fix help                         → Affiche cette aide\n");
        }
    }
}
