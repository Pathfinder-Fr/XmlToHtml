using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Text;

namespace PathfinderFr.XmlToHtml.Commands
{
    abstract class CommandBase
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(WikiPage));

        private readonly List<string> inputs = new List<string>();

        protected CommandBase()
        {
        }

        protected static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        protected List<string> Inputs
        {
            get { return this.inputs; }
        }

        protected void ReadArgs(string[] args)
        {
            Console.WriteLine("Lecture des {0} paramètres", args.Length);

            foreach (var arg in args)
            {
                if (arg[0] == '-')
                {
                    var i = arg.IndexOf(':');
                    string argName;
                    string argValue;
                    if (i != -1)
                    {
                        argName = arg.Substring(1, i - 1).ToLowerInvariant();
                        argValue = arg.Substring(i + 1);
                    }
                    else
                    {
                        argName = arg.Substring(1).ToLowerInvariant();
                        argValue = string.Empty;
                    }

                    this.ReadArgument(argName, argValue);
                }
                else
                {
                    inputs.Add(arg);
                }
            }

        }

        public virtual int Execute(string[] args)
        {
            this.ReadArgs(args);

            return 0;
        }

        protected virtual void ReadArgument(string parameterName, string parameterValue)
        {
            if(!string.IsNullOrWhiteSpace(parameterValue))
                System.Console.WriteLine("Paramètre inconnu : /{0}:{1}", parameterName, parameterValue);
            else
                System.Console.WriteLine("Paramètre inconnu : /{0}", parameterName);
        }

        protected void EnumerateInputs(Action<string> action, string pattern = "*.xml")
        {
            EnumerateFiles(this.inputs, action, pattern);
        }

        protected static void EnumerateFiles(IEnumerable<string> items, Action<string> action, string pattern = "*.xml")
        {
            foreach (var item in items)
            {
                if (Directory.Exists(item))
                {
                    // La source est un dossier
                    foreach (var file in Directory.GetFiles(item, pattern))
                    {
                        action(file);
                    }
                }
                else if (File.Exists(item))
                {
                    // La source est un fichier
                    action(item);
                }
                else
                {
                    // Le fichier n'existe pas, on l'ignore
                    System.Console.Write("fichier {0} ignoré : INEXISTANT", item);
                }
            }
        }

        protected static void AppendReplacementPatterns(string inputFile, IList<Func<string, string>> collection)
        {
            System.Console.WriteLine("Ajout expressions de remplacement du fichier {0}", inputFile);
            var lines = File.ReadAllLines(inputFile, Encoding.Default);
            string pattern = string.Empty;
            for (int i = 0; i < lines.Length; i++)
            {
                switch (i % 3)
                {
                    case 0:
                        // Commentaire
                        break;

                    case 1:
                        // PAttern
                        pattern = lines[i].Trim();
                        break;

                    case 2:
                        // Replacement
                        var replacement = lines[i];
                        if (!string.IsNullOrEmpty(pattern))
                        {
                            System.Console.WriteLine("Ajout expression de remplacement \"{0}\" => \"{1}\"", pattern, replacement);
                            collection.Add(s => Regex.Replace(s, pattern, replacement, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));
                        }
                        break;
                }
            }
        }
    }
}
