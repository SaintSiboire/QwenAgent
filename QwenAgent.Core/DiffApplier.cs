using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QwenAgent.Core
{
    public static class DiffApplier
    {
        public static void Apply(string diff, string rootPath)
        {
            // Format attendu :
            // --- path/to/file.cs
            // +++ path/to/file.cs
            // @@ ... @@
            // - ancienne ligne
            // + nouvelle ligne

            var patches = diff.Split("--- ");

            foreach (var patch in patches)
            {
                if (!patch.Contains("+++")) continue;

                var lines = patch.Split('\n');
                var filePath = lines[0].Trim();

                var fullPath = Path.Combine(rootPath, filePath);

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"[WARN] Fichier introuvable : {fullPath}");
                    continue;
                }

                File.WriteAllText(fullPath, ExtractNewContent(patch));
                Console.WriteLine($"[OK] Modifié : {filePath}");
            }
        }

        private static string ExtractNewContent(string patch)
        {
            // Pour l'instant : retourne tout le contenu après +++
            var idx = patch.IndexOf("+++");
            return patch.Substring(idx + 3);
        }
    }
}
