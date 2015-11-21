using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XmlToHtml.Commands
{
    class GeneratorCommandBase : CommandBase
    {
        private static readonly string TocReplacerPattern = @"<table id=""TocContainerTable"">.*?</table>";

        private readonly List<Func<string, string>> replacePatterns = new List<Func<string, string>>();

        private readonly List<string> replacePatternFiles = new List<string>();

        public override int Execute(string[] args)
        {
            var result = base.Execute(args);

            System.Console.WriteLine("Recherche de fichiers de remplacement");
            EnumerateFiles(replacePatternFiles, s => AppendReplacementPatterns(s, replacePatterns), "*.txt");

            return result;
        }

        protected override void ReadArgument(string parameterName, string parameterValue)
        {
            switch (parameterName)
            {
                case "r":
                    System.Console.WriteLine("Ajout remplaçement : {0}", parameterValue);
                    replacePatternFiles.Add(parameterValue);
                    break;

                default:
                    base.ReadArgument(parameterName, parameterValue);
                    break;
            }
        }

        protected string PrepareBody(string htmlBody)
        {
            htmlBody = htmlBody.Trim();

            // Suppression des tables de matières
            htmlBody = Regex.Replace(htmlBody, TocReplacerPattern, string.Empty, RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            // Exécution des expressions régulières
            foreach (var replacePattern in replacePatterns)
            {
                htmlBody = replacePattern(htmlBody);
            }

            // Nettoyage HTML
            // On supprime les <br /> du début du fichier
            while (htmlBody.StartsWith("<br", StringComparison.OrdinalIgnoreCase))
            {
                var end = htmlBody.IndexOf('>');

                if (end == -1)
                    break;

                htmlBody = htmlBody.Substring(end + 1).TrimStart();
            }

            return htmlBody;
        }
    }
}
